using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Leap;

// Matthew Cormack @johnjoemcbob
// 13/02/2015
// A coloured circle which pulses between white and black
// If a hand is detected, the circle will change to a shade of red
// Depends on: ClampedSpeedValue

namespace TragicMagic
{
	class HUDElement_LeapClass : Entity
	{
		// The Leap Motion Controller image to display
		private Otter.Image Image_Leap;
		private Otter.Image Image_LeapCable;

		// The clamped value of the cable offset from the Leap device
		private ClampedSpeedValueClass Cable;

		public HUDElement_LeapClass()
			: base()
		{
			X = 250;
			Y = 250;
		}

		public HUDElement_LeapClass( float x, float y, float speed = 1 )
			: base()
		{
			X = x;
			Y = y;
		}

		public override void Added()
		{
			base.Added();

			// Initialize the Leap cable image
			Image_LeapCable = new Otter.Image( "../../resources/leap/leapcable.png" );
			{
				Image_LeapCable.X = X;
				Image_LeapCable.Y = Y;
				Image_LeapCable.CenterOrigin();
				Image_LeapCable.OriginX = Image_LeapCable.Width + 32;
			}
			AddGraphic( Image_LeapCable );

			// Initialize the Leap controller image
			Image_Leap = new Otter.Image( "../../resources/leap/leapmotion.png" );
			{
				Image_Leap.X = X;
				Image_Leap.Y = Y;
				Image_Leap.CenterOrigin();
				Image_Leap.OriginX = 0;
			}
			AddGraphic( Image_Leap );

			// Initialize the cable offset
			Cable = new ClampedSpeedValueClass();
			Cable.Value = 32;
			Cable.Minimum = -6;
			Cable.Maximum = 32;
		}

		public override void Update()
		{
			base.Update();

			Cable.Update();
			Image_LeapCable.OriginX = Image_LeapCable.Width + Cable.Value;
		}
	}
}