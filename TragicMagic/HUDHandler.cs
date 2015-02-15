using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Matthew Cormack @johnjoemcbob
// 14/02/15
// Handle HUDs for both players, each element added to this will be added to both HUDs &
// can be looked up from this class
// Depends on: Scene_Game, HUD

namespace TragicMagic
{
	struct TeamMemberStruct
	{
		public string Name;
		public string Username;
		public string Role;
		public string Website;
	}

	class HUDHandlerClass : Entity
	{
		// Defines
		private const short HUDS = 2;
		private const short TEAM_MEMBERS = 5;

		// Hold a reference to the current scene in order to add new entities to it
		public Scene_GameClass Scene_Game;

		// Store the two wizard HUDs
		public HUDClass[] HUD;

		// Store the Leap Motion Controller warning HUD entities
		public HUDElementClass[] HUDElement_Leap;

		// Store the Team Member credit HUD entities
		public HUDElementClass[][] HUDElement_Team;

		// Store the Team Member information
		public TeamMemberStruct[] TeamMember;

		public HUDHandlerClass( Scene_GameClass scene_game )
			: base()
		{
			// Reference to the current scene
			Scene_Game = scene_game;

			// Parent HUD objects
			HUD = new HUDClass[HUDS];
			{
				HUD[0] = new HUDClass( Scene_Game, 90 );
				HUD[1] = new HUDClass( Scene_Game, -90 );
			}

			// Leap Motion Controller warning objects
			HUDElement_Leap = new HUDElementClass[HUDS];
			HUDElement_Team = new HUDElementClass[TEAM_MEMBERS][];
			{
				// Initialize each team member to have 2 HUD elements, one for each side
				for ( short member = 0; member < TEAM_MEMBERS; member++ )
				{
					HUDElement_Team[member] = new HUDElementClass[HUDS];
				}
			}

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
			}
		}

		public override void Added()
		{
			base.Added();

			AddTeam();
		}

		public override void Update()
		{
			base.Update();
		}

		// Add the Leap Motion Controller missing warning to the HUDs
		// IN: N/A
		// OUT: N/A
		public void AddLeapWarning()
		{
			if ( HUDElement_Leap[0] != null ) { return; }; // Already added

			for ( short hud = 0; hud < HUDS; hud++ )
			{
				HUDElement_Leap[hud] = new HUDElement_LeapClass(
					Scene_Game, // Reference to the current scene
					Game.Instance.HalfHeight / 2, // Position X
					25 // Position Y
				);
			}

			Add( HUDElement_Leap[0], HUDElement_Leap[1] );
		}

		// Remove the Leap Motion Controller missing warning from the HUDs
		// IN: N/A
		// OUT: N/A
		public void RemoveLeapWarning()
		{
			if ( HUDElement_Leap[0] == null ) { return; }; // Already removed

			Remove( HUDElement_Leap[0], HUDElement_Leap[1] );

			// Flag both HUD elements for garbage collection
			for ( short hud = 0; hud < HUDS; hud++ )
			{
				HUDElement_Leap[hud] = null;
			}
		}

		// Add the Leap Motion Controller missing warning to the HUDs
		// IN: N/A
		// OUT: N/A
		public void AddTeam()
		{
			if ( HUDElement_Team[0][0] != null ) { return; }; // Already added

			for ( short member = 0; member < TEAM_MEMBERS; member++ )
			{
				for ( short hud = 0; hud < HUDS; hud++ )
				{
					HUDElement_Team[member][hud] = new HUDElement_TeamMemberClass(
							Scene_Game,
							TeamMember[member].Name,
							TeamMember[member].Username,
							TeamMember[member].Role,
							TeamMember[member].Website,
							Game.Instance.HalfHeight / ( TEAM_MEMBERS + 1 ) * ( member + 1 ),
							150 + ( ( member % 2 ) * 150 )
					);
				}

				Add( HUDElement_Team[member][0], HUDElement_Team[member][1] );
			}
		}

		// Remove the Leap Motion Controller missing warning from the HUDs
		// IN: N/A
		// OUT: N/A
		public void RemoveTeam()
		{
			if ( HUDElement_Team[0][0] == null ) { return; }; // Already removed

			for ( short member = 0; member < TEAM_MEMBERS; member++ )
			{
				Remove( HUDElement_Team[member][0], HUDElement_Team[member][1] );

				// Flag both HUD elements for garbage collection
				for ( short hud = 0; hud < HUDS; hud++ )
				{
					HUDElement_Team[member][hud] = null;
				}
			}
		}

		// Add an entity to both HUDs
		// IN: (entity1) The entity representing the first HUD element, (entity2) The entity representing the second HUD element
		// OUT: N/A
		private void Add( HUDElementClass entity1, HUDElementClass entity2 )
		{
			HUD[0].Add( entity1 );
			HUD[1].Add( entity2 );
		}

		// Remove an entity from both HUDs
		// IN: (entity1) The entity representing the first HUD element, (entity2) The entity representing the second HUD element
		// OUT: N/A
		private void Remove( HUDElementClass entity1, HUDElementClass entity2 )
		{
			HUD[0].Remove( entity1 );
			HUD[1].Remove( entity2 );
		}
	}
}