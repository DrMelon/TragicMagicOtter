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
		// The ID of the wizard this spell was cast by
		public int ID = 0;

		// The direction of the spell's movement
		public Vector2 Direction = new Vector2( 0, 0 );

		// The clamped movement speed of the spell projectile
		private Speed MovementSpeed = new Speed( 10 );

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
		}

		public override void Render()
		{
			base.Render();

			Hitbox.Render();
		}

		public void SetSpeed( float speed )
		{
			MovementSpeed.X = Direction.X * speed;
			MovementSpeed.Y = Direction.Y * speed;
		}
    }
}