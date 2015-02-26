using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Matthew Cormack @johnjoemcbob
// 26/02/2015
// A basic dust cloud spell
// Depends on: Spell

namespace TragicMagic
{
	class Spell_DustClass : SpellClass
	{
		// The main particle system of this dust cloud
		private ParticleSystem Particle_Dust;

        public Spell_DustClass()
			: base( 0, 0, 0, new Vector2( 0, 0 ), 0 )
        {
			
		}
        public Spell_DustClass( int wizard, float x, float y, Vector2 direction, float speed = 1 )
			: base( wizard, x, y, direction, speed )
        {
			
		}
        ~Spell_DustClass()
        {
			
        }

		public override void Added()
		{
			base.Added();

			// Create main fire ball particle system
			Particle_Dust = new ParticleSystem( X, Y );
			Particle_Dust.Initialize( 50, 20, 0, 360, 5, 15, "../../resources/particle/smoke.png", 87, 87, 0.8f );
			Particle_Dust.particleShake = 1;
			Particle_Dust.endColour.A = 0;
			Particle_Dust.Start();
			Scene.Add( Particle_Dust );
		}

		public override void Update()
		{
			base.Update();

			// Main dust particles need to stay attached to the collider
			Particle_Dust.X = X;
			Particle_Dust.Y = Y;
		}

		public override void Removed()
		{
			base.Removed();

			// Cleanup all particles
			Particle_Dust.ClearGraphics();
			Scene.Remove( Particle_Dust );
			Particle_Dust = null;
		}
	}
}