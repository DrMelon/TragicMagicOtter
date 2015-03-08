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

		// The flag to update once after adding to scene, to ensure that the first key is highlighted
		private bool FirstUpdate;

		// The arcade button instruction graphics to display
		private Otter.Image Button_Fire;
		private Otter.Image Button_Lightning;
		private Otter.Image Button_Earth;

		// The instructional text to pair with the arcade button graphics
		private Text Text_Enter;
		private Text Text_Remove;
		private Text Text_Ready;

		// The screenshot graphic to display as part of the preview tweet
		private Otter.Image Screenshot;

		// The preview tweet text
		private Text Text_Tweet;

		// The ready flag for this keyboard entry
		public bool Ready;

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

			// The number of rows on the keyboard for positioning other elements
			short rows = 0;

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
						HUDElement_Child[key].Layer = 1;
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
				rows = row;
			}

			// Initialize instructional graphics
			float offsetx = ( rows * 2 * ( KEY_SIZE + KEY_OFFSET ) ) * 1.1f;

			// Fire, red, enter character
			Button_Fire = new Otter.Image( "../../resources/button.png" );
			{
				Button_Fire.Color = Color.Red;
				Button_Fire.CenterOrigin();
				Button_Fire.X = X - offsetx;
				Button_Fire.Y = Y;
			}
			AddGraphic( Button_Fire );

			Text_Enter = new Text( "Enter the selected character", 32 );
			{
				Text_Enter.OutlineThickness = 4;
				Text_Enter.CenterOrigin();
				Text_Enter.X = X - offsetx;
				Text_Enter.Y = Y;
			}
			AddGraphic( Text_Enter );

			// Move the two elements to be centered together
			{
				Button_Fire.OriginX = Button_Fire.HalfWidth * 1.1f + Text_Enter.HalfWidth;
				Text_Enter.OriginX = -Button_Fire.HalfWidth * 1.1f + Text_Enter.HalfWidth;
				Text_Enter.OriginY = Button_Fire.OriginY / 2;
			}
			offsetx += Button_Fire.Width * 1.1f;

			// Lightning, yellow, remove character
			Button_Lightning = new Otter.Image( "../../resources/button.png" );
			{
				Button_Lightning.Color = Color.Yellow;
				Button_Lightning.CenterOrigin();
				Button_Lightning.X = X - offsetx;
				Button_Lightning.Y = Y;
			}
			AddGraphic( Button_Lightning );

			Text_Remove = new Text( "Remove the last character", 32 );
			{
				Text_Remove.OutlineThickness = 4;
				Text_Remove.CenterOrigin();
				Text_Remove.X = X - offsetx;
				Text_Remove.Y = Y;
			}
			AddGraphic( Text_Remove );

			// Move the two elements to be centered together
			{
				Button_Lightning.OriginX = Button_Lightning.HalfWidth * 1.1f + Text_Remove.HalfWidth;
				Text_Remove.OriginX = -Button_Lightning.HalfWidth * 1.1f + Text_Remove.HalfWidth;
				Text_Remove.OriginY = Button_Lightning.OriginY / 2;
			}
			offsetx += Button_Lightning.Width * 1.1f;

			// Earth, green, toggle ready
			Button_Earth = new Otter.Image( "../../resources/button.png" );
			{
				Button_Earth.Color = Color.Green;
				Button_Earth.CenterOrigin();
				Button_Earth.X = X - offsetx;
				Button_Earth.Y = Y;
			}
			AddGraphic( Button_Earth );

			Text_Ready = new Text( "Ready to tweet", 32 );
			{
				Text_Ready.OutlineThickness = 4;
				Text_Ready.CenterOrigin();
				Text_Ready.X = X - offsetx;
				Text_Ready.Y = Y;
			}
			AddGraphic( Text_Ready );

			// Move the two elements to be centered together
			{
				Button_Earth.OriginX = Button_Earth.HalfWidth * 1.1f + Text_Ready.HalfWidth;
				Text_Ready.OriginX = -Button_Earth.HalfWidth * 1.1f + Text_Ready.HalfWidth;
				Text_Ready.OriginY = Button_Earth.OriginY / 2;
			}
			offsetx += Button_Earth.Width * 1.1f;

			// Initialize the screenshot image
			Screenshot = new Otter.Image( "roundcomplete.png" );
			{
				Screenshot.Scale = 0.4f;
				Screenshot.CenterOrigin();
				Screenshot.X = X;
				Screenshot.Y = Y;
				Screenshot.Alpha = 0;
			}
			AddGraphic( Screenshot );

			// Initialize the tweet text
			Text_Tweet = new Text( "roundcomplete.png", 16 );
			{
				Text_Tweet.X = X;
				Text_Tweet.Y = Y;
				Text_Tweet.Alpha = 0;
			}
			AddGraphic( Text_Tweet );

			// Initialize as parent to Key elements
			IsParent = true;

			// Initialzie the controller session
			ControllerSession = Game.Instance.Sessions[Player];

			// Flag an update to ensure that the first key is highlighted
			FirstUpdate = true;

			// Initialize unready
			Ready = false;

			// Draw the preview tweet a layer above the keyboard
			Layer = 0;
		}

		public override void Update()
		{
			base.Update();

			// Update to display the first highlighted key before input
			if ( FirstUpdate )
			{
				UpdateKeySelected();
				FirstUpdate = false;
			}
			// Only change text if the keyboard isn't in ready state
			if ( !Ready )
			{
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
					if ( ControllerSession.GetController<ControllerXbox360>().B.Pressed )
					{
						// Hasn't reached the max
						if ( UserString.Length < MaxLength )
						{
							UserString += KeyCharacter[Key];
							altered = true;
						}
					}

					// Remove the last character from the current string
					if ( ControllerSession.GetController<ControllerXbox360>().Y.Pressed )
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
			// Toggle the keyboard's ready status
			if ( ControllerSession.GetController<ControllerXbox360>().A.Pressed )
			{
				Ready = !Ready;

				// Update keyboard graphics to match
				if ( Ready )
				{
					// Gray out buttons
					for ( short key = 0; key < KEYS; key++ )
					{
						// Check if the key exists before drawing it
						if ( KeyCharacter[key] != "" )
						{
							// If the graphic has been added at this point
							if ( HUDElement_Child[key].Graphic != null )
							{
								HUDElement_Child[key].Graphic.Color = Color.Gray;
							}
						}
					}

					// Display preview tweet
					Screenshot.Alpha = 1;
					Text_Tweet.Alpha = 1;
				}
				else
				{
					// Colour buttons
					UpdateKeySelected();

					// Hide preview tweet
					Screenshot.Alpha = 0;
					Text_Tweet.Alpha = 0;
				}
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
					// If the graphic has been added at this point
					if ( HUDElement_Child[key].Graphic != null )
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