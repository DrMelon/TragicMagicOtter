using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Leap;

// Matthew Cormack @johnjoemcbob
// 16/02/2015
// A HUD element which shows information about all team members,
// and displays their images
// Depends on: HUDElement, HUDElement_TeamMember

namespace TragicMagic
{
	struct TeamMemberStruct
	{
		public string Name;
		public string Username;
		public string Role;
		public string Website;
	}

	class HUDElement_TeamClass : HUDElementClass
	{
		// Defines
		private const short TEAM_MEMBERS = 5; // The number of team members to display

		// Store the Team Member information
		public TeamMemberStruct[] TeamMember;

		// FadeOut Team Members when removed
		private bool FadeOut = false;

		// Constructor for this HUD element, hold a reference to the scene and setup positioning
		// IN: (scene_current) Reference to the current scene, (x) The x position of the element,
		//     (y) The y position of the element
		// OUT: N/A
		public HUDElement_TeamClass( Scene scene_current, float x = 0, float y = 0 )
			: base( scene_current )
		{
			// Position
			X = x;
			Y = y;

			// Team Member display objects
			HUDElement_Child = new HUDElementClass[TEAM_MEMBERS];

			// Initialize Team Member information
			TeamMember = new TeamMemberStruct[TEAM_MEMBERS];
			{
				short member = 0;

				// Matthew
				TeamMember[member].Name = "Matthew Cormack";
				TeamMember[member].Username = "johnjoemcbob";
				TeamMember[member].Role = "Programmer";
				TeamMember[member].Website = "www.johnjoemcbob.com";
				member++;

				// Jordan
				TeamMember[member].Name = "Jordan Brown";
				TeamMember[member].Username = "DrMelon";
				TeamMember[member].Role = "Programmer";
				TeamMember[member].Website = "www.doctor-melon.com";
				member++;

				// Sean
				TeamMember[member].Name = "Sean Thurmond";
				TeamMember[member].Username = "_Inu_";
				TeamMember[member].Role = "Programmer";
				TeamMember[member].Website = "www.carpevenatus.com";
				member++;

				// Max
				TeamMember[member].Name = "Max Wrighton";
				TeamMember[member].Username = "MaxWrighton";
				TeamMember[member].Role = "Engineer";
				TeamMember[member].Website = "www.maxwrighton.com";
				member++;

				// Pip
				TeamMember[member].Name = "Pip Snaith";
				TeamMember[member].Username = "pipsnaith";
				TeamMember[member].Role = "Artist";
				TeamMember[member].Website = "";
				member++;
			}

			// Initialize as parent to TeamMember elements
			IsParent = true;
		}

		public override void Added()
		{
			base.Added();

			// Add each team member to the screen
			for ( short member = 0; member < TEAM_MEMBERS; member++ )
			{
				HUDElement_Child[member] = new HUDElement_TeamMemberClass(
						CurrentScene, // Reference to the current scene
						TeamMember[member].Name, // Member name
						TeamMember[member].Username, // Member username
						TeamMember[member].Role, // Member role
						TeamMember[member].Website, // Member website
						( Game.Instance.HalfHeight / ( TEAM_MEMBERS + 1 ) * ( member + 1 ) ) + X, // X
						150 + ( ( member % 2 ) * 100 ) + Y // Y, Offset even team members for spacing
				);
				HUDElement_Child[member].Parent = this;
				CurrentScene.Add( HUDElement_Child[member] );
			}

			// Add the title text to the credits
			Text title = new Text( "TRAGIC MAGIC", 128 );
			{
				title.Y = Game.Instance.HalfHeight / 2;
			}
			AddGraphic( title );
		}

		public override void Update()
		{
			base.Update();

			// Fade out individual team members when removed, then ensure they are deleted
			if ( FadeOut )
			{
				// Check the first member's first graphic (they all fade the same)
				if ( ( HUDElement_Child[0] != null ) && ( HUDElement_Child[0].Graphic.Alpha <= 0 ) )
				{
					// Then cleanup
					for ( short member = 0; member < TEAM_MEMBERS; member++ )
					{
						HUDElement_Child[member] = null;
					}

					// Remove any extra graphics
					for ( short graphic = 0; graphic < Graphics.Count; graphic++ )
					{
						RemoveGraphic( Graphics[graphic] );
					}
				}
			}

			// Fade in/out the title text based on the team member elements
			foreach( Graphic graphic in Graphics )
			{
				graphic.Alpha = HUDElement_Child[0].Graphic.Alpha;
			}
		}

		// Return whether or not the element should actually be removed at this point
		// NOTE: Useful for fade out animations, etc
		// IN: N/A
		// OUT: (bool) True to remove from scene
		public override bool Remove()
		{
			for ( short member = 0; member < TEAM_MEMBERS; member++ )
			{
				HUDElement_Child[member].Remove();

				// Ensure fade out starts at alpha 1 and goes to alpha 0
				HUDElement_TeamMemberClass teammember = (HUDElement_TeamMemberClass) HUDElement_Child[member];
				teammember.Alpha.Value = 1;
			}
			FadeOut = true;

			return false;
		}
	}
}