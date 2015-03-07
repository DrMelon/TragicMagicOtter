using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Leap;
using TragicMagic;

// Matthew Cormack @johnjoemcbob
// 14/02/15
// Handle movement, combos & attacking of a player
// Depends on: GameWands, ComboSystem

// Sean Thurmond @_inu_
// 21/02/15
// GamePad Support

// J. Brown @DrMelon
// 07/03/15
// Camera Shaking on Wizard Hurt

// TODO:
// Movement functions

namespace TragicMagic
{
	enum WizardTypeStruct
	{
		WIZARD_LIGHT,
		WIZARD_DARK
	}

	class WizardClass : Entity
	{
		// Defines
		private static Vector2 WAND_OFFSET = new Vector2( -0.3f, -0.3f ); // Default is light (right hand) wizard
		private const float SPRITE_SCALE = 0.2f; // Scale down the wizard body & wand sprites
		private const float COLLISION_SCALE = 0.4f; // Scale down the wizard collision for (ONLY) clamping onscreen
		private const float HITBOX_SCALE = 0.5f; // Scale down the wizard hitbox
		private const short COMBO_MAX = 10; // The maximum string of button presses held in a combo
		private const short SOUND_HURT = 5; // The number of sounds the wizard can choose from when hurt

		// Store a reference to the current scene
		public Scene_GameClass CurrentScene;

		// Store a reference to the GameWands to query tool positions
		public GameWandsClass GameWands;

		// Store a reference to an Otter player session, so we can read controls and input.
		public Session LinkedSession;
		// Store control inputs here
		public String ComboInputs = "";

		// The wizard type (light or dark)
		public WizardTypeStruct WizardType;

		// The ID of this wizard
		public int ID = 0;

		// The wizard position on screen
		public Vector2 Position;
		public Vector2 Destination;
		public Vector2 WandOffset;

		// The wizard angle (0 points straight up) on screen
		public float Angle = 0;
		public float WandAngle = 0;
		public float WandAngleDirection = 1; // Light & dark wands rotate differently

		// The speed for the wizard to move at
		public float Speed = 20;

		// The current score of this wizard for this round
		public float Score = 0;

		// The wand direction as identified by GameWandsClass
		public Vector2 WandDirection;

		// The toggleable ability of the player to move the wizard
		public bool CanMove = false;

		// The toggleable functionality of the player to run most logic
		public bool Pause = false;

		// The wizard body sprite
		private Otter.Image Body;

		// The wizard wand sprite
		private Otter.Image Wand;

		// The linear interpolation component for moving the wizard onscreen at round start
		private ClampedSpeedValueClass ClampedPosition_X = new ClampedSpeedValueClass();
		private ClampedSpeedValueClass ClampedPosition_Y = new ClampedSpeedValueClass();

		// The shader to render the wizard with (TEST)
		private Shader TestShader;

		// The clamped position of this wizard
		private ClampedValueClass ClampedX = new ClampedValueClass();
		private ClampedValueClass ClampedY = new ClampedValueClass();

        // The forward & right facing vectors of this wizard upon creation
        private Vector2 Forward = new Vector2();
        private Vector2 Right = new Vector2();
		private Vector2 WandForward = new Vector2();

		// The array of sounds which can be played when the wizard is hurt
		private Sound[] Hurt;

		// Warn the player if their
		private Text test = new Text( 48 );

		// If using this constructor you must afterwards set the public variable GameWands
		// IN: N/A
		// OUT: N/A
		public WizardClass()
			: base()
		{
			WizardType = WizardTypeStruct.WIZARD_LIGHT;
		}

		// Pass in reference to the GameWands system & setup values unique to each wizard
		// IN: (gamewands) Reference to the Leap tool game state handler, (wizardtype) Light or dark wizard,
		//     (wizard) The ID of this wizard, (angle) The rotation of this wizard from 0 facing upwards
		// OUT: N/A
		public WizardClass( Session playsession, GameWandsClass gamewands, WizardTypeStruct wizardtype, int wizard, float angle )
			: base()
		{
			LinkedSession = playsession;
			GameWands = gamewands;
			WizardType = wizardtype;
			ID = wizard;
			Angle = angle;
		}

		~WizardClass()
		{
			Cleanup();
		}

		public override void Added()
		{
			base.Added();

			// Draw wizard on top of all but the HUD & particles
			Layer = 8 + ID;

			// Initialize the wizard's shader
			//TestShader = new Shader( "../../shaders/video.fs" );

			// Initialize the position interpolation objects
			ClampedPosition_X.Speed = 5;
			ClampedPosition_Y.Speed = 5;

			// Initialize the position of the wizard
			Position = new Vector2( 0, 0 );
			Destination = new Vector2( 0, 0 );

			// Initialize the wizard type unique variables
			string wizardtypeimage = "light";
			WandOffset = WAND_OFFSET;
			{
				if ( WizardType == WizardTypeStruct.WIZARD_DARK )
				{
					wizardtypeimage = "dark";
					WandOffset *= -1;
					WandAngleDirection *= -1;

				}
			}

			// Initialize the wizard's body sprite
			Body = new Otter.Image( "../../resources/wizard_" + wizardtypeimage + ".png" );
			{
				Body.ScaleX = SPRITE_SCALE;
				Body.ScaleY = SPRITE_SCALE;
				Body.Angle = Angle;
				Body.CenterOrigin();
			}
			// Body graphic is added later after wand

			// Initialize the wizard's wand sprite
			WandOffset *= new Vector2( Body.Width * SPRITE_SCALE, Body.Height * SPRITE_SCALE ); // Multiply offset by the size of the wizard body

			Wand = new Otter.Image( "../../resources/wand_" + wizardtypeimage + ".png" );
			{
				Wand.ScaleX = SPRITE_SCALE;
				Wand.ScaleY = SPRITE_SCALE;
				Wand.Angle = Angle + WandAngle;
				Wand.CenterOrigin();
				Wand.OriginY = Wand.Height;
				//Wand.Shader = TestShader;
			}

			// Add the graphics to the scene
			AddGraphic( Wand ); // Add wand below wizard robes
			AddGraphic( Body );
			test.OutlineThickness = 4;
			AddGraphic( test );

			// Add a hitbox to the wizard
			int size = (int) Math.Floor( Body.Width * Body.ScaleX * HITBOX_SCALE );
            SetHitbox( size, size, ( (int) ColliderType.Wizard ) + ID );
			Hitbox.CenterOrigin();

            // Calculate the forward & right vectors of this wizard
            double radangle = Math.PI * Angle / 180;
            double sin = Math.Sin( radangle );
            double cos = Math.Cos( radangle );

            Vector2 origin = new Vector2( 0, 1 ); // Default is facing upwards
            Forward = new Vector2(
				(float) ( ( (double) origin.X * cos ) - ( (double) origin.Y * sin ) ),
				(float) ( ( (double) origin.X * sin ) + ( (double) origin.Y * cos ) )
			);
			Right = new Vector2(
				Forward.Y,
				Forward.X
			);

			// Initialize the clamped position of the wizard (for player movement)
			{
				// Calculate the side of the screen the player is on
				float side = Math.Sign( Forward.X ); // -1, 0, 1
				float onleft = Math.Max( 0, side ); // 0 or 1 boolean number
				float onright = Math.Max( 0, -side ); // 0 or 1 boolean number

				// X
				float halfwidth = Body.Width * Body.ScaleX * COLLISION_SCALE; // Image is centered, ensure the whole thing stays onscreen
				ClampedX.Minimum = ( Game.Instance.HalfWidth * onright ) + halfwidth;
				ClampedX.Maximum = Game.Instance.Width - ( Game.Instance.HalfWidth * onleft ) - halfwidth;

				// Y
				float halfheight = Body.Height * Body.ScaleY * COLLISION_SCALE; // Image is centered, ensure the whole thing stays onscreen
				ClampedY.Minimum = halfheight;
				ClampedY.Maximum = Game.Instance.Height - halfheight;
			}

			// Initialize the hurt sound effects
			Hurt = new Sound[SOUND_HURT];
			{
				for ( short effect = 0; effect < SOUND_HURT; effect++ )
				{
					Hurt[effect] = new Sound( "../../resources/audio/hurt" + ( effect + 1 ) + ".wav" );
					Hurt[effect].Attenuation = 0.1f;
				}
			}
		}

		public override void Update()
		{
			base.Update();

			if ( Pause ) { return; }

			// Update the time parameter of the wizard's shader
			//TestShader.SetParameter( "Time", Game.Instance.Timer );

			// Keep body & wand attached to each other
			Body.SetPosition( Position.X, Position.Y );
			Wand.SetPosition( Position.X + WandOffset.X, Position.Y + WandOffset.Y );
			Wand.Angle = Angle + ( WandAngle * WandAngleDirection );
			test.String = GameWands.Wand[ID].ID.ToString();

			// Combo input
			CheckControls();

			// Move the wizard towards its destination
			UpdateDestination();
		}

		public void CheckControls()
		{
			if ( LinkedSession != null )
			{
				// Element Buttons (Just Pressed)
				if ( LinkedSession.GetController<ControllerXbox360>().A.Pressed)
				{
					ComboInputs += "A";
				}
                if (LinkedSession.GetController<ControllerXbox360>().B.Pressed)
				{
					ComboInputs += "B";
				}
                if (LinkedSession.GetController<ControllerXbox360>().X.Pressed)
				{
					ComboInputs += "X";
				}
                if (LinkedSession.GetController<ControllerXbox360>().Y.Pressed)
				{
					ComboInputs += "Y";
				}

				// Movement (While Held)
				if ( CanMove )
				{
					Vector2 move = new Vector2( 0, 0 );
					{
                        if (LinkedSession.GetController<ControllerXbox360>().Left.Down || LinkedSession.GetController<ControllerXbox360>().LeftStick.X < -0.2)
						{
							move.X += ( -Right.X * Game.Instance.DeltaTime * Speed );
							move.Y += ( -Right.Y * Game.Instance.DeltaTime * Speed );
						}
                        if (LinkedSession.GetController<ControllerXbox360>().Right.Down || LinkedSession.GetController<ControllerXbox360>().LeftStick.X > 0.2)
						{
							move.X += ( Right.X * Game.Instance.DeltaTime * Speed );
							move.Y += ( Right.Y * Game.Instance.DeltaTime * Speed );
						}
                        if (LinkedSession.GetController<ControllerXbox360>().Up.Down || LinkedSession.GetController<ControllerXbox360>().LeftStick.Y < -0.2)
						{
							move.X += ( Forward.X * Game.Instance.DeltaTime * Speed );
							move.Y += ( Forward.Y * Game.Instance.DeltaTime * Speed );
						}
                        if (LinkedSession.GetController<ControllerXbox360>().RightStick.Y < 0 || LinkedSession.GetController<ControllerXbox360>().RB.Pressed ) // Flick right stick up
                        {
                            //DEBUG: Using this to cast a spell
                            TryToCastSpell();
                        }

                        else if (LinkedSession.GetController<ControllerXbox360>().Down.Down || LinkedSession.GetController<ControllerXbox360>().LeftStick.Y > 0.2)
                        {
                            move.X += (-Forward.X * Game.Instance.DeltaTime * Speed);
                            move.Y += (-Forward.Y * Game.Instance.DeltaTime * Speed);
                        }
					}
					// Only run logic if user has input
					if ( ( move.X != 0 ) || ( move.Y != 0 ) )
					{
						// Only set destination if the user has actually input, otherwise continue on course
						Destination.X = X + move.X;
						Destination.Y = Y + move.Y;

						// Only clamp to the map edges if the wizard has moved
						ClampedX.Value = Destination.X; // Initial destination
						ClampedX.Update(); // Update to clamp it
						Destination.X = ClampedX.Value; // Final clamped destination

						ClampedY.Value = Destination.Y; // Initial destination
						ClampedY.Update(); // Update to clamp it
						Destination.Y = ClampedY.Value; // Final clamped destination
					}
				}

				// Update Input strings to only be 10 in length; trim off the leading characters
				if ( ComboInputs.Length > COMBO_MAX )
				{
					ComboInputs = ComboInputs.Substring( 1, COMBO_MAX );
				}
			}
		}

		// Move the wizard towards its target destination
		// NOTE: Called from main Update every frame
		// IN: N/A
		// OUT: N/A
		public void UpdateDestination()
		{
			// Unlock from the intro walk-in animation, relock if still moving towards destination
			CanMove = true;

			// Position X
			float min = Destination.X; // Default destination is smaller
			float max = X;
			short dir = -1;
			{
				if ( Destination.X > X ) // Destination bigger, invert values
				{
					min = X;
					max = Destination.X;
					dir = 1;
				}
			}
			ClampedPosition_X.Value = X;
			ClampedPosition_X.Minimum = min;
			ClampedPosition_X.Maximum = max;
			ClampedPosition_X.Direction = dir;
			if ( Math.Abs( min - max ) > 2 ) // Small range in which the wizard gets rounded to the target point
			{
				ClampedPosition_X.Update();
				X = ClampedPosition_X.Value;

				CanMove = false; // Lock into the intro walk-in animation
			}

			// Position Y
			min = Destination.Y; // Default destination is smaller
			max = Y;
			dir = -1;
			{
				if ( Destination.Y > Y ) // Destination bigger, invert values
				{
					min = Y;
					max = Destination.Y;
					dir = 1;
				}
			}
			ClampedPosition_Y.Value = Y;
			ClampedPosition_Y.Minimum = min;
			ClampedPosition_Y.Maximum = max;
			ClampedPosition_Y.Direction = dir;
			if ( Math.Abs( min - max ) > 2 ) // Small range in which the wizard gets rounded to the target point
			{
				ClampedPosition_Y.Update();
				Y = ClampedPosition_Y.Value;

				CanMove = false; // Lock into the intro walk-in animation
			}
		}

		public void TryToCastSpell()
		{
			// Check for allowed movement inside to prevent casting using the Leap wand after game ends
			if ( CanMove )
			{
				// Player tried to cast a spell; send their current combo to the spell-checker (lol)
				SpellInformation spell_description = ComboSystem.Instance.CheckSpell( ComboInputs );

				// If we actually cast a spell, do something with it!
				if ( spell_description != null )
				{
					// Recalculate the forward vector of the wizard's wand only when required
					double radangle = Math.PI * ( 180 - Wand.Angle ) / 180; // Invert to face other wizard
					double sin = Math.Sin( radangle );
					double cos = Math.Cos( radangle );

					Vector2 origin = new Vector2( 0, 1 ); // Default is facing upwards
					WandForward = new Vector2(
						(float) ( ( (double) origin.X * cos ) - ( (double) origin.Y * sin ) ),
						(float) ( ( (double) origin.X * sin ) + ( (double) origin.Y * cos ) )
					);

					// Spell fire
					Type spelltype = Type.GetType( "TragicMagic." + spell_description.spellType );
					SpellClass spell = (SpellClass) Activator.CreateInstance( spelltype );
					{
						spell.ID = ID;
						spell.X = X + Wand.X + ( WandForward.X * Wand.ScaledHeight); // Offset from wizard to wand base, then to wand tip
						spell.Y = Y + Wand.Y + ( WandForward.Y * Wand.ScaledHeight );
						spell.Direction = WandForward;
						spell.Angle = 180 - Wand.Angle;
						spell.SetSpeed( spell_description.spellSpeed );
                        spell.spellInformation = spell_description;
						CurrentScene.Add( spell );
					}
					CurrentScene.Projectile.Add( spell );

                    // Create text
                    HUDHandlerClass hudHandler = this.Scene.GetEntity<HUDHandlerClass>();
                    if(hudHandler != null)
                    {
                        // Create new score text
                        HUDElement_SpellCastText newText = new HUDElement_SpellCastText(this.Scene, (Game.Instance.HalfHeight / 2), (Game.Instance.HalfWidth / 8), spell_description);
                        hudHandler.HUD[ID].Add(newText);
                    }
				}
                else
                {

                    // Create text
                    HUDHandlerClass hudHandler = this.Scene.GetEntity<HUDHandlerClass>();
                    if (hudHandler != null)
                    {
                        // Create new score text
                        HUDElement_SpellCastText newText = new HUDElement_SpellCastText(this.Scene, (Game.Instance.HalfHeight / 2), (Game.Instance.HalfWidth / 8), spell_description);
                        hudHandler.HUD[ID].Add(newText);
                    }
                }

				// Now blank out the combo inputs, ready for another go.
				ComboInputs = "";
			}
		}

		// Cleanup any objects belonging solely to this wizard
		// IN: N/A
		// OUT: N/A
		public void Cleanup()
		{
			// Remove the wizard body sprite
			if ( Body != null )
			{
				RemoveGraphic( Body );
				Body = null;
			}
		}

		// Called when this wizard is hit with a spell
		// IN: N/A
		// OUT: N/A
		public void Hit()
		{
			// Play random hit sound effect
			int effect = Rand.Int( 0, SOUND_HURT );
			Hurt[effect].X = ( X - Game.Instance.HalfWidth ) / Game.Instance.Width * 10; // Should have a function to handle this, it is also used inside Spell.cs
			Hurt[effect].Play();

            // Screen shake!
            TragicStateManagerClass theStateManager = this.Scene.GetEntity<TragicStateManagerClass>();
            if(theStateManager != null)
            {
                theStateManager.ScreenShake.ShakeCamera(20, 5);
            }

		}
	}
}