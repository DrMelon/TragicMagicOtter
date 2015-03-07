using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Leap;

// Matthew Cormack @johnjoemcbob
// 20/02/2015
// A HUD element which is shown during game rounds which contains the score of the
// current player
// Depends on: HUDElement, ClampedSpeedValue

namespace TragicMagic
{
	class HUDElement_ScoreClass : HUDElementClass
	{
		// Defines
		private const float FADE_SPEED = 0.03f;

		// The value of the game round timer
		public float Value = 0;

		// The text image displaying the time left in the round
		private Otter.Text Text_Score;

		// The clamped value of the fade amount of the images
		private ClampedSpeedValueClass Alpha;

		// The flag for fading out this element when removed
		private bool FadeOut = false;

		// Constructor for this HUD element, hold a reference to the scene and setup positioning
		// IN: (scene_current) Reference to the current scene, (x) The x position of the element,
		//     (y) The y position of the element
		// OUT: N/A
		public HUDElement_ScoreClass( Scene scene_current, float x = 0, float y = 0 )
			: base( scene_current )
		{
			X = x;
			Y = y;
		}

		public override void Added()
		{
			base.Added();

			Text_Score = new Otter.Text( "", 48 );
			{
				Text_Score.X = X;
				Text_Score.Y = Y;
				Text_Score.CenterOrigin();
				Text_Score.OutlineColor = Color.Black;
				Text_Score.OutlineThickness = 2;
				Text_Score.Angle = 20;
			}
			Parent.AddGraphic( Text_Score );
            
			// Initialize the fade in/out
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

		// Set the value of the score, update the text
		// IN: (value) The new timer value
		// OUT: N/A
		public void SetValue( float value )
		{
			// Store the current score value
			Value = value;

			// Update the text to display
			Text_Score.String = "Score: " + Math.Ceiling( value );
			Text_Score.CenterOrigin(); // Recenter the score text's origin
			Text_Score.OriginX = 0;
		}
	}
}