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
	class HUDHandlerClass : Entity
	{
		// Defines
		private const short HUDS = 2;

		// Hold a reference to the current scene in order to add new entities to it
		public Scene_GameClass Scene_Game;

		// Store the two wizard HUDs
		public HUDClass[] HUD;

		// Store the Leap Motion Controller warning HUD entities
		public HUDElementClass[] HUDElement_Leap;

		// Store the Team Member credit HUD entities
		public HUDElementClass[] HUDElement_Team;

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

			// Team display objects
			HUDElement_Team = new HUDElementClass[HUDS];
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
			if ( HUDElement_Team[0] != null ) { return; }; // Already added

			for ( short hud = 0; hud < HUDS; hud++ )
			{
				HUDElement_Team[hud] = new HUDElement_TeamClass(
					Scene_Game, // Reference to the current scene
					0, // Position X
					200 // Position Y
				);
			}

			Add( HUDElement_Team[0], HUDElement_Team[1] );
		}

		// Remove the Leap Motion Controller missing warning from the HUDs
		// IN: N/A
		// OUT: N/A
		public void RemoveTeam()
		{
			if ( HUDElement_Team[0] == null ) { return; }; // Already removed

			Remove( HUDElement_Team[0], HUDElement_Team[1] );

			// Flag both HUD elements for garbage collection
			for ( short hud = 0; hud < HUDS; hud++ )
			{
				HUDElement_Team[hud] = null;
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