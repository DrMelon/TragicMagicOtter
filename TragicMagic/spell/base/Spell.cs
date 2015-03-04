using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Matthew Cormack @johnjoemcbob
// 19/02/2015
// A simple projectile with hitbox which will form the harmful part of spells
// Depends on: N/A

namespace TragicMagic
{
    class SpellClass : Entity
    {
		// Defines
		private const float TRAIL_BETWEEN = 0.2f; // Time between leaving trail marks
		private const float TRAIL_BETWEEN_RANDOM = 0.05f; // Random addition to time between leaving trail marks

		// The ID of the wizard this spell was cast by
		public int ID = 0;

		// The direction of the spell's movement
		public Vector2 Direction = new Vector2( 0, 0 );

		// The angle of the spell movement
		public float Angle = 0;

		// The clamped movement speed of the spell projectile
		private Speed MovementSpeed = new Speed( 10 );

		// The trail graphic to render to the ground each frame
		protected Otter.Image GroundTrail;

		// The time between leaving trail marks
		private float NextTrail = 0;
		protected float TrailBetween;
		protected float TrailBetweenRandom;

        public SpellClass( int wizard, float x, float y, Vector2 direction, float speed = 1 )
        {
			ID = wizard;
			X = x;
			Y = y;
			Direction = direction;

			MovementSpeed.X = Direction.X * speed;
			MovementSpeed.Y = Direction.Y * speed;
        }
        ~SpellClass()
        {

        }

		public override void Added()
		{
			base.Added();

			// Initialie collider
			SetHitbox( 10, 10, ( (int) ColliderType.Wizard ) + ID );
			Hitbox.CenterOrigin();

			// Initialize trail mark timers to seconds
			TrailBetween = TRAIL_BETWEEN * 60;
			TrailBetweenRandom = TRAIL_BETWEEN_RANDOM * 60;
		}

		public override void Update()
		{
			base.Update();

			X += MovementSpeed.X;
			Y += MovementSpeed.Y;

			// Handle collision logic
			for ( short wizard = 0; wizard < Scene_GameClass.WIZARDS; wizard++ )
			{
				if ( wizard != ID )
				{
					Collider collision = Hitbox.Collide( X, Y, ( (int) ColliderType.Wizard ) + wizard );
					if ( collision != null ) // Collision has happened
					{
						// Remove this collider to stop further collisions
						Scene.Remove( this );

						// Increment score of spell caster
						Scene_GameClass scene = (Scene_GameClass) Scene;
						scene.Wizards[ID].Score++;
					}
				}
			}

			// Render the trail particle for this frame onto the ground
			if ( GroundTrail != null )
			{
				// Is time to lay a new trail
				if ( NextTrail < Game.Instance.Timer )
				{
					// Is in the scene
					if ( Scene != null )
					{
						// Cast to appropriate scene for Tragic Magic
						Scene_GameClass scenegame = (Scene_GameClass ) Scene;
						// Check the ground surface render target exists
						if ( scenegame.GroundSurface != null )
						{
							// Randomly rotate the trail mark
							GroundTrail.Angle = Rand.Float( 0, 360 );

							// Draw the trail mark
							scenegame.GroundSurface.Draw( GroundTrail, X, Y );

							// Set timer for next mark
							NextTrail = Game.Instance.Timer + TrailBetween + Rand.Float( -TrailBetweenRandom, TrailBetweenRandom );
						}
					}
				}
			}

			// Handle cleanup when this spell leaves the game screen area
			if (
				( X < 0 ) || // Left
				( X > Game.Instance.Width ) || // Right
				( Y < 0 ) || // Top
				( Y > Game.Instance.Height ) // Bottom
			)
			{
				Scene.Remove( this );
			}
		}

		public void SetSpeed( float speed )
		{
			MovementSpeed.X = Direction.X * speed;
			MovementSpeed.Y = Direction.Y * speed;
		}
    }
}