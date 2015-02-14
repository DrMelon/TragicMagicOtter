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
// Depends on: ClampedValue, Leap.Controller

namespace TragicMagic
{
	class PulseCircleClass : Entity
	{
		// Clamped colour values
		public ClampedValueClass R = new ClampedValueClass();
		public ClampedValueClass G = new ClampedValueClass();
		public ClampedValueClass B = new ClampedValueClass();

		// Colour fade directions
		public float R_Direction = 1;
		public float G_Direction = 1;
		public float B_Direction = 1;

		// Speed of colour fading
		public float Speed = 1;

		// Store a reference to the LeapController to query for hands
		public Leap.Controller LeapController;

		// The coloured circle image to display
		Otter.Image Image_Circle = Otter.Image.CreateCircle( 7 );

		public PulseCircleClass() : base()
		{
			Image_Circle.SetPosition( 250, 250 );
		}

		public PulseCircleClass( float x, float y, float speed = 1 )
			: base()
		{
			Image_Circle.SetPosition( x, y );
			Speed = speed;
		}

		public override void Added()
		{
			base.Added();

			// Initialize circle
			SetGraphic( Image_Circle );
			Image_Circle.CenterOrigin();

			// Initialize clamped colours
			R.Maximum = 100;
			R.OnMinimum = DirectionR;
			R.OnMaximum = DirectionR;

			G.Maximum = 100;
			G.OnMinimum = DirectionG;
			G.OnMaximum = DirectionG;

			B.Maximum = 100;
			B.OnMinimum = DirectionB;
			B.OnMaximum = DirectionB;
		}

		public override void Update()
		{
			base.Update();

			// Increase colour values
			R.Value += R_Direction * Game.Instance.DeltaTime * Speed;
			G.Value += G_Direction * Game.Instance.DeltaTime * Speed;
			B.Value += B_Direction * Game.Instance.DeltaTime * Speed;

			// Clamp colour values to range
			R.Update();
			G.Update();
			B.Update();

			// Set image to clamped colour values
			Image_Circle.Color.R = Util.ScaleClamp( R.Value, 0, 100, 1, 0 );
			Image_Circle.Color.G = Util.ScaleClamp( G.Value, 0, 100, 1, 0 );
			Image_Circle.Color.B = Util.ScaleClamp( B.Value, 0, 100, 1, 0 );

			// If there is a Leap hand present, turn to a shade of red
			if ( LeapController != null )
			{
				Leap.Frame frame = LeapController.Frame();
				{
					if ( frame.Hands.Count > 0 )
					{
						Image_Circle.Color.R = 1;
					}
				}
				frame.Dispose();
			}
		}

		// Function to change the direction of fading when the red value is clamped
		// IN: N/A
		// OUT: (int) Meaningless
		private int DirectionR()
		{
			R_Direction *= -1;
			return 0;
		}

		// Function to change the direction of fading when the green value is clamped
		// IN: N/A
		// OUT: (int) Meaningless
		private int DirectionG()
		{
			G_Direction *= -1;
			return 0;
		}

		// Function to change the direction of fading when the blue value is clamped
		// IN: N/A
		// OUT: (int) Meaningless
		private int DirectionB()
		{
			B_Direction *= -1;
			return 0;
		}
	}
}