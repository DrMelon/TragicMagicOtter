using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Leap;

namespace TragicMagic
{
	class Scene_GameClass : Scene
	{
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
			Wizard = new WizardClass[2*wizards];
			{
				for ( int wizard = 0; wizard < wizards; wizard++ )
				{
					int id = wizard * 2;
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