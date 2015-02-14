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

				// Initialize game session & scene
				game.AddSession( "Battle" );

				Scene_Game = new Scene_GameClass();
				{
					Scene_Game.LeapController = LeapController;
				}
				Scene_Game.Initialize();
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
