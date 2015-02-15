using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Leap;

// Matthew Cormack @johnjoemcbob
// 15/02/2015
// The base HUD element
// Depends on: Scene_Game

namespace TragicMagic
{
	class HUDElementClass : Entity
	{
		// Reference to the current scene
		public Scene_GameClass Scene_Game;

		public HUDElementClass( Scene_GameClass scene_game )
			: base()
		{
			Scene_Game = scene_game;
		}

		// Return whether or not the element should actually be removed at this point
		// NOTE: Useful for fade out animations, etc
		// IN: N/A
		// OUT: (bool) True to remove from scene
		public virtual bool Remove()
		{
			return true;
		}
	}
}