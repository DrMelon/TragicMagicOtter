using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Leap;

// Matthew Cormack @johnjoemcbob
// 20/02/2015
// A HUD element which is shown after game rounds which shows the score of the
// current player, and whether they won or lost
// Depends on: HUDElement, ClampedSpeedValue

namespace TragicMagic
{
	class HUDElement_OutcomeClass : HUDElementClass
	{
		// Defines
		private const float FADE_SPEED = 0.03f;

		// The final value of this player's score
		public float Score = 0;

		// Whether or not this player won the match
		public bool Winner = false;

		// The text image displaying the time left in the round
		private Otter.Text Text_Outcome;
		private Otter.Text Text_Score;

		// The clamped value of the fade amount of the images
		private ClampedSpeedValueClass Alpha;

		// The flag for fading out this element when removed
		private bool FadeOut = false;

		// Constructor for this HUD element, hold a reference to the scene and setup positioning
		// IN: (scene_current) Reference to the current scene, (x) The x position of the element,
		//     (y) The y position of the element, (score) The final score of this player,
		//     (winner) Whether or not this player won
		// OUT: N/A
		public HUDElement_OutcomeClass( Scene scene_current, float x = 0, float y = 0, float score = 0, bool winner = false )
			: base( scene_current )
		{
			X = x;
			Y = y;
			Score = score;
			Winner = winner;
		}

		public override void Added()
		{
			base.Added();

			string text = "Loser!";
			{
				if ( Winner )
				{
					text = "Winner!";
				}
			}
			Text_Outcome = new Otter.Text( text, 96 );
			{
				Text_Outcome.X = X;
				Text_Outcome.Y = Y;
				Text_Outcome.CenterOrigin();
				Text_Outcome.OriginY = Text_Outcome.Height;
			}
			Parent.AddGraphic( Text_Outcome );

			Text_Score = new Otter.Text( Score.ToString(), 96 );
			{
				Text_Score.X = X;
				Text_Score.Y = Y;
				Text_Score.CenterOrigin();
				Text_Score.OriginY = 0;
			}
			Parent.AddGraphic( Text_Score );

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
	}
}