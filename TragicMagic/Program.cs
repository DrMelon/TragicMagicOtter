using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Matthew Cormack @johnjoemcbob
// 13/02/15
// Main Otter2D program, initializes Leap Motion Controller & Scene_Game
// Depends on: Leap.Controller, Scene_Game

// Sean Thurmond @_inu_
// 21/02/15
// GamePad Support

namespace TragicMagic
{
	class Program
	{
		static void Main( string[] args )
		{
			// Game window
			Game game;

			// Leap Motion Controller
			Leap.Listener LeapListener;
			Leap.Controller LeapController;

			// Game scene
			Scene_GameClass Scene_Game; // We want to initialize this after the game is initialized

			// Initialize
			{
				// Game Window
				game = new Game(
					"Tragic Magic", // Window Title
					1920, 1080, // Window size
					60, // Target FPS
					false // Fullscreen
				);
				game.SetWindowAutoFullscreen( true ); // VSync & auto max resolution

				// Leap Motion Controller
				LeapListener = new Leap.Listener();
				LeapController = new Leap.Controller();
				LeapController.AddListener( LeapListener );

				// Initialize player sessions
				game.AddSession( "LightWizard" );
				game.AddSession( "DarkWizard" );

				// Setup controls
				game.Session( "DarkWizard" ).Controller = new ControllerXbox360();
				game.Session( "LightWizard" ).Controller = new ControllerXbox360();

				// Keyboard / IPAC Controls
				// Movement
				game.Session( "DarkWizard" ).GetController<ControllerXbox360>().Up.AddKey( Key.W );    // Up for Player 1
				game.Session( "DarkWizard" ).GetController<ControllerXbox360>().Down.AddKey( Key.S );  // Down for Player 1
				game.Session( "DarkWizard" ).GetController<ControllerXbox360>().Left.AddKey( Key.A );  // Left for Player 1
				game.Session( "DarkWizard" ).GetController<ControllerXbox360>().Right.AddKey( Key.D ); // Right for Player 1

				game.Session( "DarkWizard" ).GetController<ControllerXbox360>().Start.AddKey( Key.Space ); //Ready Up button(?) / Start

				game.Session( "LightWizard" ).GetController<ControllerXbox360>().Up.AddKey( Key.Up );    // Up for Player 2
				game.Session( "LightWizard" ).GetController<ControllerXbox360>().Down.AddKey( Key.Down );  // Down for Player 2
				game.Session( "LightWizard" ).GetController<ControllerXbox360>().Left.AddKey( Key.Left );  // Left for Player 2
				game.Session( "LightWizard" ).GetController<ControllerXbox360>().Right.AddKey( Key.Right ); // Right for Player 2

				game.Session( "LightWizard" ).GetController<ControllerXbox360>().Start.AddKey( Key.Num0 ); //Ready Up button(?) //Start


				// Elements
				// Keyboard
				game.Session( "DarkWizard" ).GetController<ControllerXbox360>().RB.AddKey( Key.E ); // Cast Spell

				game.Session( "DarkWizard" ).GetController<ControllerXbox360>().B.AddKey( Key.Num1 ); // Fire Element Key for Player 1
				game.Session( "DarkWizard" ).GetController<ControllerXbox360>().A.AddKey( Key.Num2 ); // Earth Element Key for Player 1
				game.Session( "DarkWizard" ).GetController<ControllerXbox360>().Y.AddKey( Key.Num3 ); // Lightning Element Key for Player 1
				game.Session( "DarkWizard" ).GetController<ControllerXbox360>().X.AddKey( Key.Num4 ); // Water Element Key for Player 1

				game.Session( "LightWizard" ).GetController<ControllerXbox360>().RB.AddKey( Key.PageDown ); // Cast spell

				game.Session( "LightWizard" ).GetController<ControllerXbox360>().B.AddKey( Key.Num7 ); // Fire Element Key for Player 2
				game.Session( "LightWizard" ).GetController<ControllerXbox360>().A.AddKey( Key.Num8 ); // Earth Element Key for Player 2
				game.Session( "LightWizard" ).GetController<ControllerXbox360>().Y.AddKey( Key.Num9 ); // Lightning Element Key for Player 2
				game.Session( "LightWizard" ).GetController<ControllerXbox360>().X.AddKey( Key.Num0 ); // Water Element Key for Player 2



				// Xbox / Playstation Controller Controls

				game.Session( "DarkWizard" ).GetController<ControllerXbox360>().Up.AddAxisButton( AxisButton.PovYMinus, 0 );    // Up for Player 1 / DPAD UP
				game.Session( "DarkWizard" ).GetController<ControllerXbox360>().Down.AddAxisButton( AxisButton.PovYPlus, 0 );  // Down for Player 1 / DPAD DOWN
				game.Session( "DarkWizard" ).GetController<ControllerXbox360>().Left.AddAxisButton( AxisButton.PovXMinus, 0 );  // Left for Player 1 / DPAD LEFT
				game.Session( "DarkWizard" ).GetController<ControllerXbox360>().Right.AddAxisButton( AxisButton.PovXPlus, 0 ); // Right for Player 1 / DPAD RIGHT

				game.Session( "DarkWizard" ).GetController<ControllerXbox360>().B.AddButton( 1, 0 ); // Fire Element Key for Player 1 / RIGHT FACE BUTTON
				game.Session( "DarkWizard" ).GetController<ControllerXbox360>().A.AddButton( 0, 0 ); // Earth Element Key for Player 1 / DOWN FACE BUTTON
				game.Session( "DarkWizard" ).GetController<ControllerXbox360>().Y.AddButton( 3, 0 );  // Lightning Element Key for Player 1 / UP FACE BUTTON
				game.Session( "DarkWizard" ).GetController<ControllerXbox360>().X.AddButton( 2, 0 ); // Water Element Key for Player 1 / RIGHT FACE BUTTON

				game.Session( "DarkWizard" ).GetController<ControllerXbox360>().Start.AddButton( 7, 0 ); //Ready Up button(?)
				game.Session( "DarkWizard" ).GetController<ControllerXbox360>().LeftStick.AddAxis( JoyAxis.X, JoyAxis.Y, 0 ); //Left Stick Movement
				game.Session( "DarkWizard" ).GetController<ControllerXbox360>().RightStick.AddAxis( JoyAxis.U, JoyAxis.R, 0 ); //Right Stick Movement

				game.Session( "LightWizard" ).GetController<ControllerXbox360>().Up.AddAxisButton( AxisButton.PovYMinus, 1 );     // Up for Player 2 / DPAD UP
				game.Session( "LightWizard" ).GetController<ControllerXbox360>().Down.AddAxisButton( AxisButton.PovYPlus, 1 ); // Down for Player 2 / DPAD DOWN
				game.Session( "LightWizard" ).GetController<ControllerXbox360>().Left.AddAxisButton( AxisButton.PovXMinus, 1 ); // Left for Player 2 / DPAD LEFT
				game.Session( "LightWizard" ).GetController<ControllerXbox360>().Right.AddAxisButton( AxisButton.PovXPlus, 1 ); // Right for Player 2 / DPAD RIGHT

				game.Session( "LightWizard" ).GetController<ControllerXbox360>().B.AddButton( 1, 1 ); // Fire Element Key for Player 2
				game.Session( "LightWizard" ).GetController<ControllerXbox360>().A.AddButton( 0, 1 ); // Earth Element Key for Player 2
				game.Session( "LightWizard" ).GetController<ControllerXbox360>().Y.AddButton( 3, 1 );  // Lightning Element Key for Player 2
				game.Session( "LightWizard" ).GetController<ControllerXbox360>().X.AddButton( 2, 1 ); // Water Element Key for Player 2

				game.Session( "LightWizard" ).GetController<ControllerXbox360>().Start.AddButton( 7, 1 ); //Ready Up button(?) 
				game.Session( "LightWizard" ).GetController<ControllerXbox360>().LeftStick.AddAxis( JoyAxis.X, JoyAxis.Y, 1 ); //Left Stick Movement
				game.Session( "LightWizard" ).GetController<ControllerXbox360>().RightStick.AddAxis( JoyAxis.U, JoyAxis.R, 1 ); //Right Stick Movement

				// Initialize Scene
				Scene_Game = new Scene_GameClass();
				{
					Scene_Game.LeapController = LeapController;
				}
				Scene_Game.Initialize( game );
				game.FirstScene = Scene_Game;

				// Test tweeting interface
				TweetinviClass test = new TweetinviClass();
			}
			// Update
			{
				// Start the game application
				game.Start();
			}
			// Cleanup
			{
				// Dispose of the Leap Motion Controller
				LeapController.RemoveListener( LeapListener );
				LeapController.Dispose();
				LeapListener.Dispose();
			}
		}
	}
}