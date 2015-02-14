using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TragicMagic
{
    class Program
    {
        static void Main( string[] args )
        {
			// Initialize the game window
			Game game = new Game( "Game", 1920, 1080, 60, true );

			// Initialize Leap Motion Controller
			Leap.Listener LeapListener;
			Leap.Controller LeapController;

			LeapListener = new Leap.Listener();
			LeapController = new Leap.Controller();
			LeapController.AddListener( LeapListener );

            game.AddSession( "Battle" );

			Scene_Game scene_game = new Scene_Game();
			{
				scene_game.LeapController = LeapController;
			}
			scene_game.Initialize();
            game.FirstScene = scene_game;

			game.Start();

			// Cleanup the Leap Motion Controller
			LeapController.RemoveListener( LeapListener );
			LeapController.Dispose();
			LeapListener.Dispose();
        }
    }
}
