using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Leap;

// Matthew Cormack @johnjoemcbob
// 14/02/15
// Handle movement, combos & attacking of a player
// Depends on: GameWands, ComboSystem

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
		private const float HITBOX_SCALE = 0.5f; // Scale down the wizard hitbox
		private const short COMBO_MAX = 10; // The maximum string of button presses held in a combo

		// Store a reference to the GameWands to query tool positions
		public GameWandsClass GameWands;

		// Store a reference to an Otter player session, so we can read controls and input.
		public Session LinkedSession;
		// Store control inputs here
		public String ComboInputs = "";

		// The wizard type (light or dark)
		public WizardTypeStruct WizardType;

		// The wizard position on screen
		public Vector2 Position;
		public Vector2 Destination;
		public Vector2 WandOffset;

		// The wizard angle (0 points straight up) on screen
		public float Angle = 0;
		public float WandAngle = 0;
		public float WandAngleDirection = 1; // Light & dark wands rotate differently

		// The wand direction as identified by GameWandsClass
		public Vector2 WandDirection;

		// The wizard body sprite
		private Otter.Image Body;

		// The wizard wand sprite
		private Otter.Image Wand;

		// The linear interpolation component for moving the wizard onscreen at round start
		private ClampedSpeedValueClass ClampedPosition_X = new ClampedSpeedValueClass();
		private ClampedSpeedValueClass ClampedPosition_Y = new ClampedSpeedValueClass();

		// The shader to render the wizard with (TEST)
		private Shader TestShader;

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
		//     (angle) The rotation of this wizard from 0 facing upwards
		// OUT: N/A
		public WizardClass( Session playsession, GameWandsClass gamewands, WizardTypeStruct wizardtype, float angle )
			: base()
		{
			LinkedSession = playsession;
			GameWands = gamewands;
			WizardType = wizardtype;
			Angle = angle;
		}

		~WizardClass()
		{
			Cleanup();
		}

		public override void Added()
		{
			base.Added();

			// Initialize the wizard's shader
			TestShader = new Shader( "../../shaders/video.fs" );

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
				Wand.Shader = TestShader;
			}

			// Add the graphics to the scene
			AddGraphic( Wand ); // Add wand below wizard robes
			AddGraphic( Body );

			// Add a hitbox to the wizard
			int size = (int) Math.Floor( Body.Width * Body.ScaleX * HITBOX_SCALE );
            SetHitbox( size, size );
			Hitbox.CenterOrigin();
		}

		public override void Update()
		{
			base.Update();

			// Update the time parameter of the wizard's shader
			TestShader.SetParameter( "Time", Game.Instance.Timer );

			// Keep body & wand attached to each other
			Body.SetPosition( Position.X, Position.Y );
			Wand.SetPosition( Position.X + WandOffset.X, Position.Y + WandOffset.Y );
			Wand.Angle = Angle + ( WandAngle * WandAngleDirection );

			// Combo input
			CheckControls();

			// Move the wizard towards its destination
			UpdateDestination();
		}

		public override void Render()
		{
			base.Render();
			Hitbox.Render(); // Debug draw the hitbox
		}

		public void CheckControls()
		{
			if ( LinkedSession != null )
			{
				// Element Buttons (Just Pressed)
				if ( LinkedSession.Controller.A.Pressed )
				{
					ComboInputs += "A";
				}
				if ( LinkedSession.Controller.B.Pressed )
				{
					ComboInputs += "B";
				}
				if ( LinkedSession.Controller.X.Pressed )
				{
					ComboInputs += "X";
				}
				if ( LinkedSession.Controller.Y.Pressed )
				{
					ComboInputs += "Y";
				}

				// Movement (While Held)
				if ( LinkedSession.Controller.Left.Down )
				{
					//TODO: Move Wizard Left. Movement funcs not implemented yet.
				}
				if ( LinkedSession.Controller.Right.Down )
				{
					//TODO: Move Wizard Right. Movement funcs not implemented yet.
				}
				if ( LinkedSession.Controller.Up.Down )
				{
					//TODO: Move Wizard Up. Movement funcs not implemented yet.
				}
				if ( LinkedSession.Controller.Down.Down )
				{
					//TODO: Move Wizard Down. Movement funcs not implemented yet.
					//DEBUG: Using this to cast a spell
					TryToCastSpell();
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
			if ( Math.Abs( min - max ) > 5 )
			{
				ClampedPosition_X.Update();
				X = ClampedPosition_X.Value;
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
			if ( Math.Abs( min - max ) > 5 )
			{
				ClampedPosition_Y.Update();
				Y = ClampedPosition_Y.Value;
			}
		}

		public void TryToCastSpell()
		{
			// Player tried to cast a spell; send their current combo to the spell-checker (lol)
			SpellInformation whatSpell = ComboSystem.Instance.CheckSpell( ComboInputs );

			this.Game.Debugger.Log( "", LinkedSession.Name + ": Trying to cast with: " + ComboInputs );

			// If we actually cast a spell, do something with it!
			if ( whatSpell != null )
			{
				// Write into ingame debug console (open with @)

				this.Game.Debugger.Log( "", LinkedSession.Name + ": " + whatSpell.spellName + " just got cast!\n" );

				//TODO: Here we'd read the spell info and build a spell entity from it & add it to the scene.
			}

			// Now blank out the combo inputs, ready for another go.
			ComboInputs = "";
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
	}
}