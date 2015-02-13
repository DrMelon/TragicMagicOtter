using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Leap;

namespace TragicMagic
{
	class PulseSphere : Entity
	{
		public ClampedValue R = new ClampedValue();
		public ClampedValue G = new ClampedValue();
		public ClampedValue B = new ClampedValue();

		public float R_Direction = 1;
		public float G_Direction = 1;
		public float B_Direction = 1;

		public float Speed = 1;

		public Leap.Controller LeapController;

		Otter.Image imgBall = Otter.Image.CreateCircle( 7 );

		public PulseSphere() : base()
		{
			imgBall.SetPosition( 250, 250 );
		}

		public PulseSphere( float x, float y, float speed = 1 ) : base()
		{
			imgBall.SetPosition( x, y );
			Speed = speed;
		}

		public override void Added()
		{
			base.Added();

			SetGraphic( imgBall );
			imgBall.CenterOrigin();

			R.Maximum = 100;
			R.OnMinimum = DirectionR;
			R.OnMaximum = DirectionR;

			G.Maximum = 100;
			G.OnMinimum = DirectionG;
			G.OnMaximum = DirectionG;

			B.Maximum = 100;
			B.OnMinimum = DirectionB;
			B.OnMaximum = DirectionB;
		}

		public override void Update()
		{
			base.Update();

			R.Value += R_Direction * Game.Instance.DeltaTime * Speed;
			G.Value += G_Direction * Game.Instance.DeltaTime * Speed;
			B.Value += B_Direction * Game.Instance.DeltaTime * Speed;

			R.Update();
			G.Update();
			B.Update();

			imgBall.Color.R = Util.ScaleClamp( R.Value, 0, 100, 1, 0 );
			imgBall.Color.G = Util.ScaleClamp( G.Value, 0, 100, 1, 0 );
			imgBall.Color.B = Util.ScaleClamp( B.Value, 0, 100, 1, 0 );

			if ( LeapController != null )
			{
				Leap.Frame frame = LeapController.Frame();
				{
					if ( frame.Hands.Count > 0 )
					{
						imgBall.Color.R = 1;
					}
				}
				frame.Dispose();
			}
		}

		private int DirectionR()
		{
			R_Direction *= -1;
			return 0;
		}

		private int DirectionG()
		{
			G_Direction *= -1;
			return 0;
		}

		private int DirectionB()
		{
			B_Direction *= -1;
			return 0;
		}
	}
}