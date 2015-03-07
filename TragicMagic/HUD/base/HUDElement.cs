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
		public Scene CurrentScene;

		// Reference to the parent of this HUD element (which all graphics are added to)
		public HUDElementClass Parent = null;

		// Whether or not this element has child elements
		public bool IsParent = false;

		// Store the child elements
		public HUDElementClass[] HUDElement_Child;

		// The wizard this HUD element belongs to
		public short Wizard;

		public HUDElementClass( Scene scene_current, short wizard = 0 )
			: base()
		{
			CurrentScene = scene_current;
			Parent = this;
			Wizard = wizard;
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