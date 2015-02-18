using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TragicMagic
{
    class SpellClass : Entity
    {
		// The ID of the wizard this spell was cast by
		public int ID = 0;

		// The clamped movement speed of the spell projectile
		private Speed MovementSpeed = new Speed( 10 );

        public SpellClass( int wizard = 0, float x = 0, float y = 0 )
        {
			ID = wizard;
			X = x;
			Y = y;
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

			MovementSpeed.X = 5;
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
						Scene.Remove( this );
					}
				}
			}
		}

		public override void Render()
		{
			base.Render();

			Hitbox.Render();
		}
    }
}