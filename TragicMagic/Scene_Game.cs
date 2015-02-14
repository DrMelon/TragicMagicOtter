using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Leap;

namespace TragicMagic
{
	class Scene_Game : Scene
	{
		public Leap.Controller LeapController;

		private PulseSphere LeapWarning = null;

		public Scene_Game()
		{
		}

		~Scene_Game()
		{
		}

		public void Initialize()
		{
			int radius = 6;
			int size = 16;
			int offsetx = Game.Instance.HalfWidth;
			int offsety = Game.Instance.HalfHeight;
			for ( int x = -radius; x < radius; x++ )
			{
				for ( int y = -radius; y < radius; y++ )
				{
					float speed = ( x * y ) / radius;
					PulseSphere pulser = new PulseSphere( ( x * size ) + offsetx, ( y * size ) + offsety, speed );
					{
						pulser.LeapController = LeapController;
					}
					Add( pulser );
				}
			}
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
					LeapWarning = new PulseSphere( 250, 250, 100 ); // Placeholder for actual warning graphic
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