using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Matthew Cormack @johnjoemcbob
// 14/02/15
// Handle the HUD for a single player rotates & transforms elements accordingly
// Depends on: Scene_Game, HUDElement_Leap

namespace TragicMagic
{
	class HUDClass : Entity
	{
		// Hold a reference to the current scene in order to add new entities to it
		public Scene CurrentScene;

		// The rotation of this HUD
		public float Rotation = 0;

		// Setup the rotation of the HUD according to which wizard it belongs to
		// IN: (scene_current) The current scene, (rotation) The rotation of this HUD (90 or -90)
		// OUT: N/A
		public HUDClass( Scene scene_current, float rotation )
			: base()
		{
			CurrentScene = scene_current;
			Rotation = rotation;
		}

		public override void Added()
		{
			base.Added();
		}

		public override void Update()
		{
			base.Update();
		}

		// Add an entity to this HUD, with rotation and translation offsets
		// IN: (entity) The entity representing the HUD element
		// OUT: N/A
		public void Add( HUDElementClass entity )
		{
			// Must be added to the scene before the graphics exist
			CurrentScene.Add( entity );

			// Move each graphic in relation to where this HUD is positioned
			float x = entity.X;
			float y = entity.Y;
			{
				if ( Rotation < 0 ) // Invert x/y & place on other side of screen
				{
					entity.X = Game.Instance.HalfWidth - y;
					entity.Y = Game.Instance.HalfHeight - x;
				}
				else // Invert x/y
				{
					entity.X = y;
					entity.Y = x;
				}
			}
			foreach ( Graphic graphic in entity.Graphics ) // Update each graphic
			{
				float offx = graphic.X - x;
				float offy = graphic.Y - y;
				{
					if ( Rotation < 0 ) // Place on other side of screen
					{
						offx *= -1;
						offy *= -1;
					}
				}
				graphic.SetPosition( entity.X + offx, entity.Y + offy );
				graphic.Angle += -Rotation;
			}
		}

		// Remove an entity from this HUD
		// IN: (entity) The entity representing the HUD element
		// OUT: N/A
		public void Remove( HUDElementClass entity )
		{
			bool removenow = entity.Remove(); // Perform individual element preremove functionality
			if ( removenow )
			{
				CurrentScene.Remove( entity );
			}
		}
	}
}