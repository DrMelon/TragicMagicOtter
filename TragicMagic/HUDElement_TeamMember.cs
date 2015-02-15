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
	class HUDElement_TeamMemberClass : HUDElementClass
	{
		// Defines
		private const float IMAGE_SCALE = 0.25f;
		private const float IMAGE_OFFSET_TEXT = 0.65f;
		private const float TEXT_OFFSET_SCALE = 1.2f;
		private const int FONT_SIZE = 24;
		private const float FADE_SPEED = 0.08f;

		// The username of this team member
		public string Username = "";

		// The role of this team member
		public string Role = "";

		// The website url of this team member
		public string Website = "";

		// The Leap Motion Controller image to display
		private Otter.Image Image_Avatar;
		private Otter.Text Text_Name;
		private Otter.Text Text_Username;
		private Otter.Text Text_Role;
		private Otter.Text Text_Website;

		// The clamped value of the cable offset from the Leap device
		private ClampedSpeedValueClass Cable;

		// The clamped value of the fade amount of the images
		private ClampedSpeedValueClass Alpha;

		// The flag for fading out this element when removed
		private bool FadeOut = false;

		// Constructor for this HUD element, hold a reference to the scene and setup positioning
		// IN: (scene_game) Reference to the current scene, (name) The real name of this member,
		//     (username) The twitter username of this member, (role) The role of this member,
		//     (website) The website of this member,
		//     (x) The x position of the element, (y) The y position of the element
		public HUDElement_TeamMemberClass( Scene_GameClass scene_game, string name, string username, string role, string website, float x = 0, float y = 0 )
			: base( scene_game )
		{
			// Unique information
			Name = name; // Overwrite's Otter.Entity Name
			Username = username;
			Role = role;
			Website = website;

			// Position
			X = x;
			Y = y;
		}

		public override void Added()
		{
			base.Added();

			// Initialize the Team Member's image
			Image_Avatar = new Otter.Image( "../../resources/team/" + Username + ".png" );
			{
				Image_Avatar.Scale = IMAGE_SCALE;
				Image_Avatar.X = X;
				Image_Avatar.Y = Y;
				Image_Avatar.CenterOrigin();
				Image_Avatar.OriginY += IMAGE_OFFSET_TEXT;
			}
			AddGraphic( Image_Avatar );

			// Store the y offset for text elements
			float offsety = ( IMAGE_OFFSET_TEXT * Image_Avatar.Width * Image_Avatar.ScaleX );

			// Initialize the Team Member's twitter username
			Text_Name = new Otter.Text( Name, FONT_SIZE );
			{
				Text_Name.X = X - offsety;
				Text_Name.Y = Y;
				Text_Name.CenterOrigin();

				// Add to the offset for the next text
				offsety += Text_Name.Height * TEXT_OFFSET_SCALE;
			}
			AddGraphic( Text_Name );

			// Initialize the Team Member's twitter username
			Text_Username = new Otter.Text( "@" + Username, FONT_SIZE );
			{
				Text_Username.X = X - offsety;
				Text_Username.Y = Y;
				Text_Username.CenterOrigin();

				// Add to the offset for the next text
				offsety += Text_Username.Height * TEXT_OFFSET_SCALE;
			}
			AddGraphic( Text_Username );

			// Initialize the Team Member's role
			Text_Role = new Otter.Text( Role, FONT_SIZE );
			{
				Text_Role.X = X - offsety;
				Text_Role.Y = Y;
				Text_Role.CenterOrigin();

				// Add to the offset for the next text
				offsety += Text_Role.Height * TEXT_OFFSET_SCALE;
			}
			AddGraphic( Text_Role );

			// Initialize the Team Member's website url
			Text_Website = new Otter.Text( Website, FONT_SIZE );
			{
				Text_Website.X = X - offsety;
				Text_Website.Y = Y;
				Text_Website.CenterOrigin();

				// Add to the offset for the next text
				offsety += Text_Website.Height * TEXT_OFFSET_SCALE;
			}
			AddGraphic( Text_Website );

			// Initialize the cable offset
			Alpha = new ClampedSpeedValueClass();
			{
				Alpha.Value = 0;
				Alpha.Minimum = 0;
				Alpha.Maximum = 1;
				Alpha.Speed = FADE_SPEED;
			}

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
				if ( Graphic.Alpha < 1 ) // Still fading in
				{
					Alpha.Update();

					// Update the images to have this new alpha value
					foreach ( Graphic graphic in Graphics )
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
			FadeOut = true;
			return false;
		}
	}
}