using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Leap;

// Matthew Cormack @johnjoemcbob
// 07/03/2015
// A HUD element which appears when there is no Leap Motion Controller device found,
// with instructions for the user to plug the device in
// Depends on: HUDElement, ClampedSpeedValue

namespace TragicMagic
{
	class HUDElement_ToolClass : HUDElementClass
	{
		// Defines
		private const float FADE_SPEED = 0.03f;

		// The warning message to display
		private Otter.Text Text_Warning;

		// The clamped value of the fade amount of the images
		private ClampedSpeedValueClass Alpha;

		// The flag for fading out this element when removed
		private bool FadeOut = false;

		// Store a reference to the GameWands interface for the Leap
		public GameWandsClass GameWands;
		
		// Constructor for this HUD element, hold a reference to the scene and setup positioning
		// IN: (scene_current) Reference to the current scene, (wizard) The ID of the wizard this
		//     HUD element belongs to, (x) The x position of the element,
		//     (y) The y position of the element
		// OUT: N/A
		public HUDElement_ToolClass( Scene scene_current, short wizard = 0, float x = 0, float y = 0 )
			: base( scene_current, wizard )
		{
			X = x;
			Y = y;
		}

		public override void Added()
		{
			base.Added();

			// Get reference to GameWands interface for handling Leap Tools
			GameWands = ( (Scene_GameClass) Scene ).GameWands;

			Text_Warning = new Otter.Text( "", 32 );
			{
				Text_Warning.X = X;
				Text_Warning.Y = Y;
				Text_Warning.CenterOrigin();
				Text_Warning.OutlineThickness = 2;
			}
			Parent.AddGraphic( Text_Warning );

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

			bool wanddetected = ( ( Game.Instance.Timer - GameWands.Wand[Wizard].Time_Recorded ) < 10 );
			bool wandclose = ( Math.Abs( GameWands.Wand[Wizard].Position.Y ) < 40 );

			// Display warning about wand missing
			if ( !wanddetected )
			{
				Text_Warning.String = "Hold your wand over the sensor!";
			}
			else if ( wandclose )
			{
				Text_Warning.String = "Hold your wand closer to your body!";
			}
			else
			{
				Text_Warning.String = "Hold your wand higher up!";
			}
			Text_Warning.CenterOrigin();

			// Display warning about wand height
			if ( ( !wanddetected ) || wandclose || ( GameWands.Wand[Wizard].Height < 100 ) ) // Fade in at the start of the animation
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
			else if ( Parent.Graphic.Alpha > 0 ) // Fade out at the end of the animation
			{
				Alpha.Update();

				// Update the images to have this new alpha value
				foreach ( Graphic graphic in Parent.Graphics )
				{
					graphic.Alpha = Alpha.Value;
				}
			}
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