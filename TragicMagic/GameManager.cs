using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TragicMagic
{
	class GameManager : Entity
	{
		public Scene OtterScene;
		public Leap.Controller LeapController;

		private PulseSphere LeapWarning = null;

		public GameManager() : base()
		{
			
		}

		public override void Added()
		{
			base.Added();
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
					OtterScene.Add( LeapWarning );
				}
			}
			else // Atleast 1 device connected
			{
				if ( LeapWarning != null ) // Not already disposed of
				{
					OtterScene.Remove( LeapWarning );
					LeapWarning = null; // Disregard reference to pulser, flag for garbage collect
				}
			}
		}
	}
}