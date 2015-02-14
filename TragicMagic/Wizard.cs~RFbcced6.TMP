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
// Fix multiple rendering wizards

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
		private static Vector2 WANDOFFSET = new Vector2( -0.3f, -0.3f ); // Default is light (right hand) wizard
		private const float SPRITESCALE = 0.2f; // Scale down the wizard body & wand sprites

		// Store a reference to the GameWands to query tool positions
		public GameWandsClass GameWands;

		// The wizard type (light or dark)
		public WizardTypeStruct WizardType;

		// The wizard position on screen
		public Vector2 Position;
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

		// If using this constructor you must afterwards set the public variable GameWands
		// IN: N/A
		// OUT: N/A
		public WizardClass() : base()
		{
			WizardType = WizardTypeStruct.WIZARD_LIGHT;
		}

		// Pass in reference to the GameWands system & setup values unique to each wizard
		// IN: (gamewands) Reference to the Leap tool game state handler, (wizardtype) Light or dark wizard,
		//     (position) The position of this wizard on screen
		// OUT: N/A
		public WizardClass( GameWandsClass gamewands, WizardTypeStruct wizardtype, Vector2 position, float angle )
			: base()
		{
			GameWands = gamewands;
			WizardType = wizardtype;
			Position = position;
			Angle = angle;
		}

		~WizardClass()
		{
			Cleanup();
		}

		public override void Added()
		{
			base.Added();

			// Initialize the wizard type unique variables
			string wizardtypeimage = "light";
			WandOffset = WANDOFFSET;
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
				Body.ScaleX = SPRITESCALE;
				Body.ScaleY = SPRITESCALE;
				Body.Angle = Angle;
				Body.SetPosition( Position.X, Position.Y );
				Body.CenterOrigin();
			}
			// Body graphic is added later after wand

			// Initialize the wizard's wand sprite
			WandOffset *= new Vector2( Body.Width * SPRITESCALE, Body.Height * SPRITESCALE ); // Multiply offset by the size of the wizard body

			Wand = new Otter.Image( "../../resources/wand_" + wizardtypeimage + ".png" );
			{
				Wand.ScaleX = SPRITESCALE;
				Wand.ScaleY = SPRITESCALE;
				Wand.Angle = Angle + WandAngle;
				Wand.SetPosition( Position.X + WandOffset.X, Position.Y + WandOffset.Y );
				Wand.CenterOrigin();
				Wand.OriginY = Wand.Height;
			}

			// Add the graphics to the scene
			AddGraphic( Wand ); // Add wand below wizard robes
			AddGraphic( Body );
		}

		public override void Update()
		{
			base.Update();

			//Position.X += Rand.Float( -10, 10 );
			//Position.Y += Rand.Float( -10, 10 );

			// TODO: Fix multiple rendering (tried UpdateLast & UpdateFirst - no change)
			// Keep body & wand attached to each other
			Body.SetPosition( Position.X, Position.Y );
			Wand.SetPosition( Position.X + WandOffset.X, Position.Y + WandOffset.Y );
			Wand.Angle = Angle + ( WandAngle * WandAngleDirection );
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