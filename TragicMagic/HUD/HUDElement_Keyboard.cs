using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Leap;

// Matthew Cormack @johnjoemcbob
// 04/03/2015
// A HUD element which shows a virtual keyboard & allows for interaction
// with it
// Depends on: HUDElement

namespace TragicMagic
{
	class HUDElement_KeyboardClass : HUDElementClass
	{
		// Defines
		private const float CONTROLLER_STICK_DEADZONE = 0.2f;
		private const short KEYS = 44;
		private const short KEYS_ROW = 11;
		private const float KEY_SIZE = 72 / 2;
		private const float KEY_OFFSET = 4;

		// The array of keys to add to the keyboard
		private string[] KeyCharacter;

		// The key currently selected
		private short Key = 0;

		// Store a reference to the player's controller session
		private short Player;
		private Session ControllerSession;

		// The user's currently entered string
		public string UserString = "";

		// The maximum length of the entered string
		public short MaxLength = 15;

		// The text display of the user's string
		private Text Text_UserString;

		// Constructor for this HUD element, hold a reference to the scene and setup positioning
		// IN: (scene_current) Reference to the current scene, (x) The x position of the element,
		//     (y) The y position of the element, (player) The index of the keyboard's owner
		// OUT: N/A
		public HUDElement_KeyboardClass( Scene scene_current, float x = 0, float y = 0, short player = 0 )
			: base( scene_current )
		{
			// Position
			X = x;
			Y = y;

			// The index of the player the keyboard belongs to
			Player = player;
		}

		public override void Added()
		{
			base.Added();

			// Initialize entered text display
			Text_UserString = new Text( "@", 48 );
			{
				Text_UserString.CenterOrigin();
				Text_UserString.X = X + ( KEY_SIZE * 2 );
				Text_UserString.Y = Y;
				Text_UserString.OutlineThickness = 4;
				Text_UserString.OutlineQuality = TextOutlineQuality.Absurd;
			}
			AddGraphic( Text_UserString );

			// Initialie the array of key character strings
			KeyCharacter = new string[KEYS];
			{
				// Start key index at 0 to enable key++ in one line
				short key = 0;

				// Row 1
				KeyCharacter[key++] = "1";
				KeyCharacter[key++] = "2";
				KeyCharacter[key++] = "3";
				KeyCharacter[key++] = "4";
				KeyCharacter[key++] = "5";
				KeyCharacter[key++] = "6";
				KeyCharacter[key++] = "7";
				KeyCharacter[key++] = "8";
				KeyCharacter[key++] = "9";
				KeyCharacter[key++] = "0";
				KeyCharacter[key++] = "_";

				// Row 2
				KeyCharacter[key++] = "Q";
				KeyCharacter[key++] = "W";
				KeyCharacter[key++] = "E";
				KeyCharacter[key++] = "R";
				KeyCharacter[key++] = "T";
				KeyCharacter[key++] = "Y";
				KeyCharacter[key++] = "U";
				KeyCharacter[key++] = "I";
				KeyCharacter[key++] = "O";
				KeyCharacter[key++] = "P";
				KeyCharacter[key++] = "";

				// Row 3
				KeyCharacter[key++] = "A";
				KeyCharacter[key++] = "S";
				KeyCharacter[key++] = "D";
				KeyCharacter[key++] = "F";
				KeyCharacter[key++] = "G";
				KeyCharacter[key++] = "H";
				KeyCharacter[key++] = "J";
				KeyCharacter[key++] = "K";
				KeyCharacter[key++] = "L";
				KeyCharacter[key++] = "";
				KeyCharacter[key++] = "";

				// Row 4
				KeyCharacter[key++] = "Z";
				KeyCharacter[key++] = "X";
				KeyCharacter[key++] = "C";
				KeyCharacter[key++] = "V";
				KeyCharacter[key++] = "B";
				KeyCharacter[key++] = "N";
				KeyCharacter[key++] = "M";
				KeyCharacter[key++] = "";
				KeyCharacter[key++] = "";
				KeyCharacter[key++] = "";
				KeyCharacter[key++] = "";
			}

			// Initialize the array of keys
			HUDElement_Child = new HUDElementClass[KEYS];
			{
				// Initialize key positions
				short column = 0;
				short row = 0;

				for ( short key = 0; key < KEYS; key++ )
				{
					// Check if the key exists before drawing it
					if ( KeyCharacter[key] != "" )
					{
						// Center the key row on the keyboard
						float startx = -( ( KEYS_ROW - 1 ) * ( KEY_SIZE + KEY_OFFSET ) / 2 ) + ( row * ( KEY_SIZE + KEY_OFFSET ) / 2 );
						// Offset the key by the current column
						float keyx = ( column * ( KEY_SIZE + KEY_OFFSET ) );
						// Offset the key by the current row
						float keyy = -( row * ( KEY_SIZE + KEY_OFFSET ) );

						HUDElement_Child[key] = new HUDElement_KeyClass(
							CurrentScene, // Reference to the current scene
							startx + keyx + X, // Position X
							keyy + Y, // Position Y
							KeyCharacter[key] // The key string to display
						);
						HUDElement_Child[key].Parent = this;
						CurrentScene.Add( HUDElement_Child[key] );
					}

					// Offset the button to the correct row and column
					column++;
					if ( column >= KEYS_ROW )
					{
						row++;
						column = 0;
					}
				}
			}

			// Initialize as parent to Key elements
			IsParent = true;

			// Initialzie the controller session
			ControllerSession = Game.Instance.Sessions[Player];
		}

		public override void Update()
		{
			base.Update();

			// Move cursor left
			if (
				ControllerSession.GetController<ControllerXbox360>().Left.Pressed ||
				( ControllerSession.GetController<ControllerXbox360>().LeftStick.X < -CONTROLLER_STICK_DEADZONE )
			)
			{
				UpdateKeySelected( -1 );
			}
			// Move cursor right
			if (
				ControllerSession.GetController<ControllerXbox360>().Right.Pressed ||
				( ControllerSession.GetController<ControllerXbox360>().LeftStick.X > CONTROLLER_STICK_DEADZONE )
			)
			{
				UpdateKeySelected( 1 );
			}
			// Move cursor up
			if (
				ControllerSession.GetController<ControllerXbox360>().Up.Pressed ||
				( ControllerSession.GetController<ControllerXbox360>().LeftStick.Y < -CONTROLLER_STICK_DEADZONE )
			)
			{
				UpdateKeySelected( 0, -1 );
			}
			// Move cursor down
			if (
				ControllerSession.GetController<ControllerXbox360>().Down.Pressed ||
				( ControllerSession.GetController<ControllerXbox360>().LeftStick.Y > CONTROLLER_STICK_DEADZONE )
			)
			{
				UpdateKeySelected( 0, 1 );
			}

			// Flag for text being altered
			bool altered = false;
			{
				// Enter the selected key into the current string
				if ( ControllerSession.GetController<ControllerXbox360>().A.Pressed )
				{
					// Hasn't reached the max
					if ( UserString.Length < MaxLength )
					{
						UserString += KeyCharacter[Key];
						altered = true;
					}
				}

				// Enter the selected key into the current string
				if ( ControllerSession.GetController<ControllerXbox360>().B.Pressed )
				{
					// Still has entered data
					if ( UserString.Length > 1 )
					{
						// Remove last character
						UserString = UserString.Remove( UserString.Length - 1 );
						altered = true;
					}
					else if ( UserString.Length == 1 )
					{
						UserString = "";
						altered = true;
					}
				}
			}
			if ( altered )
			{
				Text_UserString.String = "@" + UserString;
				Text_UserString.CenterOrigin();
			}
		}

		// Update the key selected and make its button a different colour
		// IN: (x) The direction the key was updated in on the x axis,
		//     (y) The direction the key was updated in on the y axis
		// OUT: N/A
		private void UpdateKeySelected( short x = 0, short y = 0 )
		{
			// Ensure the key selected exists graphically
			while( true )
			{
				Key += x;
				Key += (short) ( y * KEYS_ROW );

				// If the key selected is off the front
				if ( Key < 0 )
				{
					// Moved left
					if ( x < 0 )
					{
						// Loop around to right hand side of first row
						Key = KEYS_ROW - 1;
					}
					// Moved up
					if ( y < 0 )
					{
						// Loop around to bottom side of column
						Key = (short) ( KEYS + Key );
					}
				}
				// If the key selected is off the back
				else if ( Key >= KEYS )
				{
					// Moved right
					if ( x > 0 )
					{
						// Loop around to right hand side of first row
						Key -= KEYS_ROW;
					}
					// Moved down
					if ( y > 0 )
					{
						// Loop around to bottom side of column
						Key = (short) ( Key - KEYS );
					}
				}
				// If the key selected is on a new row left
				else if ( ( Key != 0 ) && ( ( Key % ( KEYS_ROW - 1 ) ) == 0 ) && ( x < 0 ) )
				{
					Key += KEYS_ROW - 1;
				}
				// If the key selected is on a new row right
				else if ( ( Key != 0 ) && ( ( Key % KEYS_ROW ) == 0 ) && ( x > 0 ) )
				{
					Key -= KEYS_ROW;
				}

				// Ensure the key is within the array bounds
				Key = Math.Max( (short) 0, Math.Min( (short) ( KEYS - 1 ), (short) Key ) );

				// If selected key hasn't got a graphic, keep moving in the current direction
				if ( KeyCharacter[Key] != "" )
				{
					break;
				}
			}

			// Update graphics
			for ( short key = 0; key < KEYS; key++ )
			{
				// Check if the key exists before drawing it
				if ( KeyCharacter[key] != "" )
				{
					HUDElement_Child[key].Graphic.Color = Color.White;

					// Is selected, change colour
					if ( key == Key )
					{
						HUDElement_Child[key].Graphic.Color = Color.Blue;
					}
				}
			}
		}

		// Return whether or not the element should actually be removed at this point
		// NOTE: Useful for fade out animations, etc
		// IN: N/A
		// OUT: (bool) True to remove from scene
		public override bool Remove()
		{
			for ( short key = 0; key < KEYS; key++ )
			{
				if ( HUDElement_Child[key] != null )
				{
					HUDElement_Child[key].Remove();
					CurrentScene.Remove( HUDElement_Child[key] );
				}
			}

			return true;
		}
	}
}