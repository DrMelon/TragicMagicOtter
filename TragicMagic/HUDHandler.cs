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
		public Entity[] HUDElement_Leap;

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
			HUDElement_Leap = new Entity[HUDS];
		}

		public override void Added()
		{
			base.Added();
		}

		public override void Update()
		{
			base.Update();
		}

		// Add an entity to both HUDs
		// IN: (entity1) The entity representing the first HUD element, (entity2) The entity representing the second HUD element
		// OUT: N/A
		public void Add( Entity entity1, Entity entity2 )
		{
			HUD[0].Add( entity1 );
			HUD[1].Add( entity2 );
		}

		// Add the Leap Motion Controller missing warning to the HUDs
		// IN: N/A
		// OUT: N/A
		public void AddLeapWarning()
		{
			if ( HUDElement_Leap[0] != null ) { return; }; // Already added

			HUDElement_Leap[0] = new HUDElement_LeapClass( Game.Instance.HalfHeight / 2, 25 );
			HUDElement_Leap[1] = new HUDElement_LeapClass( Game.Instance.HalfHeight / 2, 25 );

			Add( (Entity) HUDElement_Leap[0], (Entity) HUDElement_Leap[1] );
		}

		// Remove an entity from both HUDs
		// IN: (entity1) The entity representing the first HUD element, (entity2) The entity representing the second HUD element
		// OUT: N/A
		public void Remove( Entity entity1, Entity entity2 )
		{
			HUD[0].Remove( entity1 );
			HUD[1].Remove( entity2 );
		}

		// Remove the Leap Motion Controller missing warning from the HUDs
		// IN: N/A
		// OUT: N/A
		public void RemoveLeapWarning()
		{
			if ( HUDElement_Leap[0] == null ) { return; }; // Already removed

			Remove( HUDElement_Leap[0], HUDElement_Leap[1] );

			HUDElement_Leap[0] = null;
			HUDElement_Leap[1] = null;
		}
	}
}