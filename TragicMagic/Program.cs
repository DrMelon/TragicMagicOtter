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
			Scene_GameClass Scene_Game;

			// Initialize
			{
				// Game Window
				game = new Game(
					"Tragic Magic", // Window Title
					1920, 1080, // Window size
					60, // Target FPS
					true // Fullscreen
				);

				// Leap Motion Controller
				LeapListener = new Leap.Listener();
				LeapController = new Leap.Controller();
				LeapController.AddListener( LeapListener );

				// Initialize player sessions & scene
                game.AddSession( "LightWizard" );
                game.AddSession( "DarkWizard" );

                // Initialize player controls

                // Keyboard / IPAC Controls
                // Movement
                game.Session("LightWizard").Controller.Up.AddKey(Key.W);    // Up for Player 1
                game.Session("LightWizard").Controller.Down.AddKey(Key.S);  // Down for Player 1
                game.Session("LightWizard").Controller.Left.AddKey(Key.A);  // Left for Player 1
                game.Session("LightWizard").Controller.Right.AddKey(Key.D); // Right for Player 1

                game.Session("DarkWizard").Controller.Up.AddKey(Key.Up);    // Up for Player 2
                game.Session("DarkWizard").Controller.Down.AddKey(Key.Down);  // Down for Player 2
                game.Session("DarkWizard").Controller.Left.AddKey(Key.Left);  // Left for Player 2
                game.Session("DarkWizard").Controller.Right.AddKey(Key.Right); // Right for Player 2

                // Elements
                game.Session("LightWizard").Controller.B.AddKey(Key.Num1); // Fire Element Key for Player 1
                game.Session("LightWizard").Controller.A.AddKey(Key.Num2); // Earth Element Key for Player 1
                game.Session("LightWizard").Controller.Y.AddKey(Key.Num3); // Lightning Element Key for Player 1
                game.Session("LightWizard").Controller.X.AddKey(Key.Num4); // Water Element Key for Player 1

                game.Session("DarkWizard").Controller.B.AddKey(Key.Num7); // Fire Element Key for Player 2
                game.Session("DarkWizard").Controller.A.AddKey(Key.Num8); // Earth Element Key for Player 2
                game.Session("DarkWizard").Controller.Y.AddKey(Key.Num9); // Lightning Element Key for Player 2
                game.Session("DarkWizard").Controller.X.AddKey(Key.Num0); // Water Element Key for Player 2

                // Xbox / Playstation Controller Controls
                // TODO: Define Xbox controller buttons. 
                // Note: Otter contains a two-way binding so that the B button can be accessed with Controller.Circle for playstation bindings etc.
                //game.Session("LightWizard").Controller.B.AddButton( XBOX_B )


				Scene_Game = new Scene_GameClass();
				{
					Scene_Game.LeapController = LeapController;
				}
				
                
				game.FirstScene = Scene_Game;
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
