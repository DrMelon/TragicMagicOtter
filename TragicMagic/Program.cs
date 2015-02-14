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

			Scene_GameClass Scene_Game = new Scene_GameClass();
			{
				Scene_Game.LeapController = LeapController;
			}
			Scene_Game.Initialize();
			game.FirstScene = Scene_Game;

			game.Start();

			// Cleanup the Leap Motion Controller
			LeapController.RemoveListener( LeapListener );
			LeapController.Dispose();
			LeapListener.Dispose();
        }
    }
}
