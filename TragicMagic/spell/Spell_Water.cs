using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// J. Brown @DrMelon
// 07/03/2015
// Water Blast Spell
// Depends on: Spell

namespace TragicMagic
{
    class Spell_WaterClass : SpellClass
    {
		// The main particle system is a spray of water; a splashy droplets look
        private ParticleSystem Particle_Spray;


		public Spell_WaterClass()
			: base( 0, 0, 0, new Vector2( 0, 0 ), 0 )
		{

		}
		public Spell_WaterClass( int wizard, float x, float y, Vector2 direction, float speed = 2 )
			: base( wizard, x, y, direction, speed )
		{

		}
        ~Spell_WaterClass()
		{

		}

		public override void Added()
		{
			base.Added();

			// Create main spell particle system
			Particle_Spray = new ParticleSystem( X, Y );
            Particle_Spray.Initialize(10, 10, 0, 360, 15, 15, "../../resources/particle/water_circle.png", 87, 87, 1.5f);
            Particle_Spray.beginColour = Color.White;
            Particle_Spray.endColour = (Color.Blue * Color.Gray * Color.Gray);
            Particle_Spray.endColour.A = 0;
            Particle_Spray.particleShake = 4;
            Particle_Spray.particleStartRotation = Rand.Float(-360, 360);
            Particle_Spray.particleEndRotation = Rand.Float(-360, 360);
            Particle_Spray.Start();
            Scene.Add(Particle_Spray);


            GroundSplat = new Otter.Image("../../resources/particle/splash.png");
            GroundSplat.Color = Color.Cyan;
            GroundSplat.Scale = 2.5f;
            GroundSplat.Color.A = 0.4f;

            GroundTrail = new Otter.Image("../../resources/particle/splash.png");
            GroundTrail.Color = Color.Cyan;
			GroundTrail.Color.A = 0.2f;

			// Remove default audio sample
			AudioLoop.Stop();
			AudioLoop = null;

			// Initialize the fire audio loop
			AudioLoop = new Sound( "../../resources/audio/water.wav", true );
			AudioLoop.Attenuation = 0.1f;
			AudioLoop.Play();
		}

		public override void Update()
		{
			base.Update();

			// Main particles need to stay attached to the collider
            Particle_Spray.X = X;
            Particle_Spray.Y = Y;
            Particle_Spray.particleStartRotation = Rand.Float(-360, 360);
            Particle_Spray.particleEndRotation = Rand.Float(-360, 360);

		}

		public override void Removed()
		{
			base.Removed();

			// Cleanup all particles
            Particle_Spray.ClearGraphics();
            Scene.Remove(Particle_Spray);
            Particle_Spray = null;


		}
	}
}
