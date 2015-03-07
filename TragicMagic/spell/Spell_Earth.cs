using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Matthew Cormack @johnjoemcbob
// 07/03/2015
// A basic earth projectile rock spell
// Depends on: Spell, ParticleSystem

namespace TragicMagic
{
	class Spell_EarthClass : SpellClass
	{
		// Defines
		private const float TRAIL_BETWEEN = 0.1f; // Time between leaving trail marks
		private const float TRAIL_BETWEEN_RANDOM = 0.05f; // Random addition to time between leaving trail marks
		private const short IMAGE_ROCKS = 4; // The number of rock images to choose between
		private const short IMAGE_ROCKS_SMALL = 6; // The number of small rock images to choose between
		private const short ROCK_EXTRA = 6; // The extra rocks circling the big central one
		private const short ROCK_EXTRA_RANDOM = 4; // The random change in number of small rocks
		private const float ROCK_EXTRA_OFFSET = 48; // The offset from the center of the rock

		// The main particle system of this dust cloud
		private ParticleSystem Particle_Dust;

		public Spell_EarthClass()
			: base( 0, 0, 0, new Vector2( 0, 0 ), 0 )
		{

		}
		public Spell_EarthClass( int wizard, float x, float y, Vector2 direction, float speed = 1 )
			: base( wizard, x, y, direction, speed )
		{

		}
		~Spell_EarthClass()
		{

		}

		public override void Added()
		{
			base.Added();

			// Add base rock image to projectile
			Graphic = new Otter.Image( "../../resources/particle/rock" + Rand.Int( 1, IMAGE_ROCKS ) + ".png" );
			Graphic.CenterOrigin();

			// Add the circling smaller rocks
			int extras = ROCK_EXTRA + Rand.Int( -ROCK_EXTRA_RANDOM, ROCK_EXTRA_RANDOM );
			for ( int extra = 1; extra <= extras; extra++ )
			{
				AddGraphic( new Otter.Image( "../../resources/particle/rock_small" + Rand.Int( 1, IMAGE_ROCKS_SMALL ) + ".png" ) );
				Graphics[extra].OriginX = -ROCK_EXTRA_OFFSET;
				Graphics[extra].OriginY = -ROCK_EXTRA_OFFSET;
				Graphics[extra].Angle = 360 / extras * extra;
			}

			// Create main fire ball particle system
			Particle_Dust = new ParticleSystem( X, Y );
			Particle_Dust.Initialize( 50, 20, 0, 360, 1, 15, "../../resources/particle/smoke.png", 87, 87, 0.8f );
			Particle_Dust.particleShake = 1;
			Particle_Dust.beginColour = Color.Gray * Color.Gray * Color.Orange + Color.Gray;
			Particle_Dust.beginColour.A = 0.5f;
			Particle_Dust.endColour = Color.Black;
			Particle_Dust.endColour.A = 0;
			Particle_Dust.particleStartScale = 0.1f;
			Particle_Dust.particleEndScale = 1.0f;
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

			// Rotate main rock image
			Graphic.Angle += 5;

			// Rotate other smaller rocks
			foreach ( Graphic graphic in Graphics )
			{
				// Isn't the first main graphic
				if ( graphic != Graphic )
				{
					// Rotate & offset in sine wave
					graphic.Angle += 7;
					graphic.OriginX = -ROCK_EXTRA_OFFSET + ( (float) Math.Sin( ( graphic.Angle % 360 ) / 360 ) * 20 );
					graphic.OriginY = -ROCK_EXTRA_OFFSET + ( (float) Math.Sin( ( graphic.Angle % 360 ) / 360 ) * 20 );
				}
			}

			// Main dust particles need to stay attached to the collider
			Particle_Dust.X = X + (float) Math.Sin( Particle_Dust.Angle / 90 ) * 25;
			Particle_Dust.Y = Y + (float) Math.Cos( Particle_Dust.Angle / 90 ) * 25;
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