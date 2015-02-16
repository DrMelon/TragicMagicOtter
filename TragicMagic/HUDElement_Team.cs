using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Leap;

// Matthew Cormack @johnjoemcbob
// 14/02/2015
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
		private const short TEAM_MEMBERS = 5;

		// Store the Team Member credit HUD entities
		public HUDElementClass[] HUDElement_TeamMember;

		// Store the Team Member information
		public TeamMemberStruct[] TeamMember;

		// Constructor for this HUD element, hold a reference to the scene and setup positioning
		// IN: (scene_game) Reference to the current scene, (x) The x position of the element,
		//     (y) The y position of the element
		public HUDElement_TeamClass( Scene_GameClass scene_game, float x = 0, float y = 0 )
			: base( scene_game )
		{
			// Position
			X = x;
			Y = y;

			// Team Member display objects
			HUDElement_TeamMember = new HUDElementClass[TEAM_MEMBERS];

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
				TeamMember[member].Role = "Designer";
				TeamMember[member].Website = "www.maxwrighton.com";
				member++;

				// Pip
				TeamMember[member].Name = "Pip Snaith";
				TeamMember[member].Username = "pipsnaith";
				TeamMember[member].Role = "Artist";
				TeamMember[member].Website = "";
				member++;

				// Levie
			}
		}

		public override void Added()
		{
			base.Added();

			for ( short member = 0; member < TEAM_MEMBERS; member++ )
			{
				HUDElement_TeamMember[member] = new HUDElement_TeamMemberClass(
						Scene_Game, // Reference to the current scene
						TeamMember[member].Name, // Member name
						TeamMember[member].Username, // Member username
						TeamMember[member].Role, // Member role
						TeamMember[member].Website, // Member website
						150 + ( ( member % 2 ) * 200 ) + X, // Y, Offset even team members for spacing
						( Game.Instance.Height / ( TEAM_MEMBERS + 1 ) * ( member + 1 ) ) + Y // X
				);
				HUDElement_TeamMember[member].Parent = this;
				Scene_Game.Add( HUDElement_TeamMember[member] );
			}
		}

		public override void Update()
		{
			base.Update();
		}

		// Return whether or not the element should actually be removed at this point
		// NOTE: Useful for fade out animations, etc
		// IN: N/A
		// OUT: (bool) True to remove from scene
		public override bool Remove()
		{
			for ( short member = 0; member < TEAM_MEMBERS; member++ )
			{
				HUDElement_TeamMember[member].Remove();
			}

			return true;
		}
	}
}