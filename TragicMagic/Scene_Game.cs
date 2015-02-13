using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Leap;

namespace TragicMagic
{
	class Scene_Game : Scene
	{
		public Leap.Controller LeapController;

		public Scene_Game()
		{
		}

		public void Initialize()
		{
			GameManager gamemanager = new GameManager();
			{
				gamemanager.OtterScene = this;
				gamemanager.LeapController = LeapController;
			}
			Add( gamemanager );

			int radius = 6;
			int size = 16;
			int offsetx = Game.Instance.HalfWidth;
			int offsety = Game.Instance.HalfHeight;
			for ( int x = -radius; x < radius; x++ )
			{
				for ( int y = -radius; y < radius; y++ )
				{
					float speed = ( x * y ) / radius;
					PulseSphere pulser = new PulseSphere( ( x * size ) + offsetx, ( y * size ) + offsety, speed );
					{
						pulser.LeapController = LeapController;
					}
					Add( pulser );
				}
			}
		}

		~Scene_Game()
		{
			RemoveAll();
		}
	}
}