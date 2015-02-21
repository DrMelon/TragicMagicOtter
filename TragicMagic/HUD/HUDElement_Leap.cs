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
// Depends on: HUDElement, ClampedSpeedValue

namespace TragicMagic
{
	class HUDElement_LeapClass : HUDElementClass
	{
		// Defines
		private const float FADE_SPEED = 0.03f;

		// The Leap Motion Controller image to display
		private Otter.Image Image_LeapCable_Background;
		private Otter.Image Image_LeapCable;
		private Otter.Image Image_Leap;
		private Otter.Text Text_Warning;

		// The clamped value of the cable offset from the Leap device
		private ClampedSpeedValueClass Cable;

		// The clamped value of the fade amount of the images
		private ClampedSpeedValueClass Alpha;

		// The flag for fading out this element when removed
		private bool FadeOut = false;

		// Constructor for this HUD element, hold a reference to the scene and setup positioning
		// IN: (scene_current) Reference to the current scene, (x) The x position of the element,
		//     (y) The y position of the element
		// OUT: N/A
		public HUDElement_LeapClass( Scene scene_current, float x = 0, float y = 0 )
			: base( scene_current )
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
			Parent.AddGraphic( Image_LeapCable_Background );

			// Initialize the Leap cable image
			Image_LeapCable = new Otter.Image( "../../resources/leap/leapcable.png" );
			{
				Image_LeapCable.X = X;
				Image_LeapCable.Y = Y;
				Image_LeapCable.CenterOrigin();
				Image_LeapCable.OriginX = Image_LeapCable.Width + 32;
			}
			Parent.AddGraphic( Image_LeapCable );

			// Initialize the Leap controller image
			Image_Leap = new Otter.Image( "../../resources/leap/leapmotion.png" );
			{
				Image_Leap.X = X;
				Image_Leap.Y = Y;
				Image_Leap.CenterOrigin();
				Image_Leap.OriginX = 0;
			}
			Parent.AddGraphic( Image_Leap );

			Text_Warning = new Otter.Text( "Plug in Leap Motion Controller device", 16 );
			{
				Text_Warning.X = X + 55;
				Text_Warning.Y = Y;
				Text_Warning.CenterOrigin();
			}
			Parent.AddGraphic( Text_Warning );

			// Initialize the cable offset
			Cable = new ClampedSpeedValueClass();
			{
				Cable.Value = 32;
				Cable.Minimum = -6;
				Cable.Maximum = 32;
			}

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
                // graphic.Angle works here
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