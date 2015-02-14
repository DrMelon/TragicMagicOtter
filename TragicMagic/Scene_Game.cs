using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Leap;

// Matthew Cormack @johnjoemcbob
// 13/02/15
// Main game scene, contains gameplay
// Depends on: Leap.Controller, GameWands, PulseCircle, Wizard

namespace TragicMagic
{
	class Scene_GameClass : Scene
	{
		// Defines
		private const short WIZARDS = 2;
		private const float WAND_ROTATE_MAX = 45;

		// Store a reference to the LeapController object to pass to children
		public Leap.Controller LeapController;

		// Store a reference to the GameWands to query tool positions
		public GameWandsClass GameWands = new GameWandsClass();

		private PulseCircleClass LeapWarning = null;

		// Store the two wizard players
		private WizardClass[] Wizard;

		// Store the two wizard HUDs
		//private HUDClass[] HUD;

		public Scene_GameClass()
		{
			Initialize();
		}

		~Scene_GameClass()
		{
		}

		public void Initialize()
		{
			// Set the reference to LeapController within the GameWands class & add to game update
			GameWands.LeapController = LeapController;
			Add( GameWands );

			// Initialize the two wizard players
			float wizardoffset = 256;

			int wizards = 1; // This is just for fun
			Wizard = new WizardClass[WIZARDS * wizards];
			{
				for ( int wizard = 0; wizard < wizards; wizard++ )
				{
					int id = wizard * WIZARDS;
					// Light wizard on the right
					Wizard[id] = new WizardClass(
						GameWands,
						WizardTypeStruct.WIZARD_LIGHT,
						new Vector2( Game.Instance.Width - wizardoffset, Game.Instance.HalfHeight ),
						90
					);
					Add( Wizard[id] );

					id++;
					// Dark wizard on the left
					Wizard[id] = new WizardClass(
						GameWands,
						WizardTypeStruct.WIZARD_DARK,
						new Vector2( wizardoffset, Game.Instance.HalfHeight ),
						-90
					);
					Add( Wizard[id] );
				}
			}

			// Initialize grid of pulsing circles
			//int radius = 6;
			//int size = 16;
			//int offsetx = Game.Instance.HalfWidth;
			//int offsety = Game.Instance.HalfHeight;
			//for ( int x = -radius; x < radius; x++ )
			//{
			//	for ( int y = -radius; y < radius; y++ )
			//	{
			//		float speed = ( x * y ) / radius;
			//		PulseCircleClass pulser = new PulseCircleClass( ( x * size ) + offsetx, ( y * size ) + offsety, speed );
			//		{
			//			pulser.LeapController = LeapController;
			//		}
			//		Add( pulser );
			//	}
			//}
		}

		public override void Update()
		{
			base.Update();

			// Update the rotation of the wand based on Leap tool tracking
			for ( short wizard = 0; wizard < WIZARDS; wizard++ )
			{
				Wizard[wizard].WandDirection = GameWands.Wand[wizard].Direction;
				Wizard[wizard].WandAngle = GameWands.Wand[wizard].Direction.X * WAND_ROTATE_MAX;
			}

			// Display errors if the Leap device is missing
			Update_CheckLeap();
		}

		// Check the count of Leap Motion Devices & display a warning if there are none
		// NOTE: Called from main game update every frame
		// IN: N/A
		// OUT: N/A
		private void Update_CheckLeap()
		{
			if ( ( LeapController == null ) || ( LeapController.Devices.Count == 0 ) ) // No devices connected
			{
				if ( LeapWarning == null ) // Does not exist yet
				{
					LeapWarning = new PulseCircleClass( 250, 250, 100 ); // Placeholder for actual warning graphic
					Add( LeapWarning );
				}
			}
			else // Atleast 1 device connected
			{
				if ( LeapWarning != null ) // Not already disposed of
				{
					Remove( LeapWarning );
					LeapWarning = null; // Disregard reference to pulser, flag for garbage collect
				}
			}
		}
	}
}