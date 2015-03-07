using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Matthew Cormack @johnjoemcbob
// 26/02/2015
// A basic dust cloud spell
// Depends on: Spell, ParticleSystem

namespace TragicMagic
{
	class Spell_DustClass : SpellClass
	{
		// Defines
		private const float TRAIL_BETWEEN = 0.1f; // Time between leaving trail marks
		private const float TRAIL_BETWEEN_RANDOM = 0.05f; // Random addition to time between leaving trail marks

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
			Particle_Dust.beginColour = Color.Gray * Color.Gray * Color.Orange + Color.Gray;
			Particle_Dust.beginColour.A = 0.5f;
			Particle_Dust.endColour = Color.Black;
			Particle_Dust.endColour.A = 0;
			Particle_Dust.particleEndScale = 5.0f;
			Particle_Dust.particleStartRotation = Rand.Float( -720, 720 );
			Particle_Dust.particleEndRotation = Rand.Float( -720, 720 );
			Particle_Dust.particleLocalSpace = true;
            Particle_Dust.Start();
			Scene.Add( Particle_Dust );

			// Intitialize the ground trail mark image
			GroundTrail = new Otter.Image( "../../resources/particle/scorch.png" );

			// Initialize trail mark timers to seconds
			TrailBetween = TRAIL_BETWEEN * 60;
			TrailBetweenRandom = TRAIL_BETWEEN_RANDOM * 60;

			// Remove default audio sample
			AudioLoop.Stop();
			AudioLoop = null;

			// Initialize the dust storm audio loop
			AudioLoop = new Sound( "../../resources/audio/dust.wav", true );
			AudioLoop.Attenuation = 0.1f;
			AudioLoop.Play();
		}

		public override void Update()
		{
			base.Update();

			// Main dust particles need to stay attached to the collider
			Particle_Dust.X = X + (float) Math.Sin( Particle_Dust.Angle / 90 ) * 25;
			Particle_Dust.Y = Y + (float) Math.Cos( Particle_Dust.Angle / 90 ) * 25;
			Particle_Dust.Angle += 10;
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