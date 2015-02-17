using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Leap;

// Matthew Cormack @johnjoemcbob
// 16/02/2015
// A HUD element which shows the currently entered combo elements
// Depends on: HUDElement

namespace TragicMagic
{
	class HUDElement_ComboBarClass : HUDElementClass
	{
		// Defines
		private const short COMBO_MAX = 10;
		private const float ELEMENT_OFFSET = 1.1f;

		// The element images to display
		private Otter.Image[] Image_Element;

		// Constructor for this HUD element, hold a reference to the scene and setup positioning
		// IN: (scene_current) Reference to the current scene, (x) The x position of the element,
		//     (y) The y position of the element
		// OUT: N/A
		public HUDElement_ComboBarClass( Scene scene_current, float x = 0, float y = 0 )
			: base( scene_current )
		{
			// Position
			X = x;
			Y = y;
		}

		public override void Added()
		{
			base.Added();

			// Initialize the array of element images
			Image_Element = new Otter.Image[COMBO_MAX];
			{
				for ( int element = 0; element < COMBO_MAX; element++ )
				{
					float offset = element - ( COMBO_MAX / 2 );

					string elementtype = "fire";
					Image_Element[element] = new Otter.Image( "../../resources/element/" + elementtype + ".png" ); // Initialize to element type
					{
						Image_Element[element].X = X;
						Image_Element[element].Y = Y + ( Image_Element[element].Width * ELEMENT_OFFSET * offset );
						Image_Element[element].Alpha = 0; // Don't display until a combo is entered
					}
					Parent.AddGraphic( Image_Element[element] );
				}
			}
		}

		public override void Update()
		{
			base.Update();
		}

		// Update the element combo HUD with the current combo entry
		// IN: (combo) The current combo
		// OUT: N/A
		public void UpdateElements( string combo )
		{
			for ( int character = 0; character < combo.Length; character++ )
			{
				if ( character == COMBO_MAX ) { break; }; // Stay within image array bounds

				string elementtype = ComboSystem.GetElement( combo[character] ); // Lookup element for this button entry in the combo
				if ( elementtype != "" ) // Element type exists
				{
					Image_Element[character].SetTexture( "../../resources/element/" + elementtype + ".png" );
					Image_Element[character].Alpha = 1; // Display when a combo is entered
				}
			}
			if ( combo.Length < COMBO_MAX ) // Hide excess elements
			{
				for ( int element = combo.Length; element < COMBO_MAX; element++ )
				{
					Image_Element[element].Alpha = 0; // Don't display until a combo is entered
				}
			}
		}
	}
}