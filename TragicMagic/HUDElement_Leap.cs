using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Leap;

// Matthew Cormack @johnjoemcbob
// 14/02/2015
// A HUD element which appears when there is no Leap Motion Controller device found,
// with instructions for the user to plug the device in
// Depends on: ClampedSpeedValue

namespace TragicMagic
{
	class HUDElement_LeapClass : HUDElementClass
	{
		// The Leap Motion Controller image to display
		private Otter.Image Image_LeapCable_Background;
		private Otter.Image Image_LeapCable;
		private Otter.Image Image_Leap;

		// The clamped value of the cable offset from the Leap device
		private ClampedSpeedValueClass Cable;

		// The clamped value of the fade amount of the images
		private ClampedSpeedValueClass Alpha;

		// The flag for fading out this element when removed
		private bool FadeOut = false;

		// Constructor for this HUD element, hold a reference to the scene and setup positioning
		// IN: (scene_game) Reference to the current scene, (x) The x position of the element,
		//     (y) The y position of the element, (speed) The speed for the Leap cable to move at
		public HUDElement_LeapClass( Scene_GameClass scene_game, float x = 0, float y = 0, float speed = 1 )
			: base( scene_game )
		{
			X = x;
			Y = y;
		}

		public override void Added()
		{
			base.Added();

			// Initialize the background Leap cable image
			Image_LeapCable_Background = new Otter.Image( "../../resources/leap/leapcableback.png" );
			{
				Image_LeapCable_Background.X = X;
				Image_LeapCable_Background.Y = Y;
				Image_LeapCable_Background.CenterOrigin();
				Image_LeapCable_Background.OriginX = Image_LeapCable_Background.Width + 32;
			}
			AddGraphic( Image_LeapCable_Background );

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

			// Initialize the cable offset
			Alpha = new ClampedSpeedValueClass();
			Alpha.Value = 0;
			Alpha.Minimum = 0;
			Alpha.Maximum = 1;
			Alpha.Speed = 0.03f;

			// Update the images to have this initial alpha value
			foreach ( Graphic graphic in Graphics )
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
				foreach ( Graphic graphic in Graphics )
				{
					graphic.Alpha = Alpha.Value;
				}

				// Remove from scene when done
				if ( Alpha.Value <= 0 )
				{
					Scene_Game.Remove( this );
				}
			}
			else // Fade in at the start of the animation
			{
				if ( Image_LeapCable_Background.Alpha < 1 ) // Still fading in
				{
					Alpha.Update();

					// Update the images to have this new alpha value
					foreach ( Graphic graphic in Graphics )
					{
						graphic.Alpha = Alpha.Value;
					}
				}
			}

			// Move the cable using the clamped moving value
			Cable.Update();
			Image_LeapCable.OriginX = Image_LeapCable.Width + Cable.Value;
		}

		// Return whether or not the element should actually be removed at this point
		// NOTE: Useful for fade out animations, etc
		// IN: N/A
		// OUT: (bool) True to remove from scene
		public override bool Remove()
		{
			FadeOut = true;
			return false;
		}
	}
}