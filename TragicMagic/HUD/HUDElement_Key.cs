using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Leap;

// Matthew Cormack @johnjoemcbob
// 04/03/2015
// A HUD element which shows a key on a keyboard
// Depends on: HUDElement

namespace TragicMagic
{
	class HUDElement_KeyClass : HUDElementClass
	{
		// Defines
		

		// The key image to display
		private Otter.Image Image_Key;

		// The character to display on this key
		private string Key;
		private Text KeyCharacter;

		// Constructor for this HUD element, hold a reference to the scene and setup positioning
		// IN: (scene_current) Reference to the current scene, (x) The x position of the element,
		//     (y) The y position of the element, (key) The string to display on the button
		// OUT: N/A
		public HUDElement_KeyClass( Scene scene_current, float x = 0, float y = 0, string key = "" )
			: base( scene_current )
		{
			// Position
			X = x;
			Y = y;

			// The key to display on the button
			Key = key;
		}

		public override void Added()
		{
			base.Added();

			// Initialize the key graphic
			Image_Key = new Otter.Image( "../../resources/key.png" ); // Initialize to element type
			{
				Image_Key.CenterOrigin();
				Image_Key.X = X;
				Image_Key.Y = Y;
			}
			AddGraphic( Image_Key );

			// Initialize this key's character text display
			KeyCharacter = new Text( Key, 48 );
			{
				KeyCharacter.CenterOrigin();
				KeyCharacter.OriginY = Image_Key.HalfHeight;
				KeyCharacter.X = X;
				KeyCharacter.Y = Y;
				KeyCharacter.OutlineThickness = 4;
				KeyCharacter.OutlineQuality = TextOutlineQuality.Absurd;
			}
			AddGraphic( KeyCharacter );
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
			return true;
		}
	}
}