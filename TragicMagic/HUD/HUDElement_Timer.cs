using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Leap;

// Matthew Cormack @johnjoemcbob
// 20/02/2015
// A HUD element which is shown during game rounds which contains the time until the
// round is over
// Depends on: HUDElement, ClampedSpeedValue

namespace TragicMagic
{
	class HUDElement_TimerClass : HUDElementClass
	{
		// Defines
		private const float FADE_SPEED = 0.03f;
		private const float FLASHING_VALUE = 10;

		// The value of the game round timer
		public float Value = 0;

		// The text image displaying the time left in the round
		private Otter.Text Text_Time;

		// The clamped value of the fade amount of the images
		private ClampedSpeedValueClass Alpha;

		// The flag for fading out this element when removed
		private bool FadeOut = false;

		// Constructor for this HUD element, hold a reference to the scene and setup positioning
		// IN: (scene_current) Reference to the current scene, (x) The x position of the element,
		//     (y) The y position of the element
		// OUT: N/A
		public HUDElement_TimerClass( Scene scene_current, float x = 0, float y = 0 )
			: base( scene_current )
		{
			X = x;
			Y = y;
		}

		public override void Added()
		{
			base.Added();

			Text_Time = new Otter.Text( "", 32 );
			{
				Text_Time.X = X;
				Text_Time.Y = Y;
				Text_Time.CenterOrigin();
				Text_Time.OutlineColor = Color.Black;
				Text_Time.OutlineThickness = 2;
			}
			Parent.AddGraphic( Text_Time );

			// Initialize the cable offset
			Alpha = new ClampedSpeedValueClass();
			{
				Alpha.Value = 0;
				Alpha.Minimum = 0;
				Alpha.Maximum = 1;
				Alpha.Speed = FADE_SPEED;
			}

			// Update the images to have this initial alpha value
			foreach ( Graphic graphic in Parent.Graphics )
			{
				graphic.Alpha = Alpha.Value;
			}
		}

		public override void Update()
		{
			base.Update();

			// Flash colours when near the end of the round
			float flashtimeoffset = 0.5f; // Stops the flashing from starting prematurely and changing too quickly
			if ( Value <= ( FLASHING_VALUE - flashtimeoffset ) )
			{
				if ( Math.Floor( Value * 2 ) % 2 == 0 ) // Red, bold
				{
					Text_Time.Color = new Color( "FF0000" );
					Text_Time.TextStyle = TextStyle.Bold;
				}
				else // White, regular
				{
					Text_Time.Color = new Color( "FFFFFF" );
					Text_Time.TextStyle = TextStyle.Regular;
				}
			}

			if ( FadeOut ) // Fade out at the end of the animation
			{
				Alpha.Update();

				// Update the images to have this new alpha value
				foreach ( Graphic graphic in Parent.Graphics )
				{
					graphic.Alpha = Alpha.Value;
				}

				// Remove from scene when done
				if ( Alpha.Value <= 0 )
				{
					CurrentScene.Remove( this );
				}
			}
			else // Fade in at the start of the animation
			{
				if ( Parent.Graphic.Alpha < 1 ) // Still fading in
				{
					Alpha.Update();

					// Update the images to have this new alpha value
					foreach ( Graphic graphic in Parent.Graphics )
					{
						graphic.Alpha = Alpha.Value;
					}
				}
			}
		}

		// Return whether or not the element should actually be removed at this point
		// NOTE: Useful for fade out animations, etc
		// IN: N/A
		// OUT: (bool) True to remove from scene
		public override bool Remove()
		{
			Alpha.Value = 1; // This element was giving trouble by fading twice on exit, alpha wasn't set right for some reason
			FadeOut = true;
			return false;
		}

		// Set the value of the timer, update the text
		// IN: (value) The new timer value
		// OUT: N/A
		public void SetValue( float value )
		{
			// Store the current timer value for flashing colours at round end
			Value = value;

			// Update the text to display
			Text_Time.String = "Time: " + Math.Ceiling( value );
			Text_Time.CenterOrigin(); // Recenter the timer text's origin
		}
	}
}