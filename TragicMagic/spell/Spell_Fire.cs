using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Matthew Cormack @johnjoemcbob
// 26/02/2015
// A basic fire ball projectile
// Depends on: Spell

namespace TragicMagic
{
	class Spell_FireClass : SpellClass
	{
		// The particle system of this fire ball
		private ParticleSystem Particle_Fire;

        public Spell_FireClass()
			: base( 0, 0, 0, new Vector2( 0, 0 ), 0 )
        {
			
		}
        public Spell_FireClass( int wizard, float x, float y, Vector2 direction, float speed = 1 )
			: base( wizard, x, y, direction, speed )
        {
			
		}
        ~Spell_FireClass()
        {
			
        }

		public override void Added()
		{
			base.Added();

			// Create main fire ball particle system
			Particle_Fire = new ParticleSystem( X, Y );
			Particle_Fire.Initialize( 50, 20, 0, 360, 5, 15, "../../resources/particle/fire.png", 87, 87, 0.8f );
			Particle_Fire.beginColour = Color.Red;
			Particle_Fire.endColour = Color.Red;
			Particle_Fire.endColour.A = 0;
			Particle_Fire.Start();
			Scene.Add( Particle_Fire );
		}

		public override void Update()
		{
			base.Update();

			// Main fireball particles need to stay attached to the collider
			Particle_Fire.X = X;
			Particle_Fire.Y = Y;
		}

		public override void Removed()
		{
			base.Removed();

			// Cleanup all particles
			Particle_Fire.ClearGraphics();
			Scene.Remove( Particle_Fire );
			Particle_Fire = null;
		}
	}
}