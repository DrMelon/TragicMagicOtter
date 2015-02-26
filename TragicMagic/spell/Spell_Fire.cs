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
			Particle_Fire.Initialize( 50, 20, 30, 10, 10, 5, "../../resources/team/johnjoemcbob.png", 240, 240, 0.2f );
			Particle_Fire.Start();
			Scene.Add( Particle_Fire );
		}

		public override void Update()
		{
			base.Update();

			
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