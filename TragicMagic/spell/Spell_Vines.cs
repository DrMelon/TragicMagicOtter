using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// J. Brown @DrMelon
// 07/03/2015
// Vines Spell
// Depends on: Spell, ParticleSystem

namespace TragicMagic
{
	class Spell_VinesClass : SpellClass
	{
		// Defines
		private const float HEIGHT_MIN = 50; // The minimum height of the sine wave

		// The particle contains 2 particle systems; two vines that sinewave across eachother
		private ParticleSystem VineOne;
		private SineWave VineSineOne;

		private ParticleSystem VineTwo;
		private SineWave VineSineTwo;

		// The height from the central line for the wave to rise
		private float Height = 100;

		public Spell_VinesClass()
			: base( 0, 0, 0, new Vector2( 0, 0 ), 0 )
		{

		}
		public Spell_VinesClass( int wizard, float x, float y, Vector2 direction, float speed = 2 )
			: base( wizard, x, y, direction, speed )
		{

		}
		~Spell_VinesClass()
		{

		}

		public override void Added()
		{
			base.Added();

			float offsetsin = Rand.Float( -180, 180 );

			// Create first vine
			VineSineOne = new SineWave( 5, 1, offsetsin );
			VineOne = new ParticleSystem( X, Y - VineSineOne.Value * 100 );
			VineOne.Initialize( 0, 0, 0, 0, 1, 60, "../../resources/particle/vine.png", 87, 87, 0.3f );
			VineOne.beginColour = Color.Orange * Color.Gray;
			VineOne.endColour = ( Color.Green * Color.Gray * Color.Red );
			VineOne.endColour.A = 0;
			VineOne.particleShake = 4;
			VineOne.particleStartRotation = ( VineSineOne.Value * ( 180.0f / (float) Math.PI ) ) * 2;
			VineOne.particleEndRotation = VineOne.particleStartRotation;
			VineOne.Start();
			Scene.Add( VineOne );
			this.AddComponent( VineSineOne );

			// Create second vine
			VineSineTwo = new SineWave( 5, 1, 180.0f + offsetsin );
			VineTwo = new ParticleSystem( X, Y - VineSineTwo.Value * 100 );
			VineTwo.Initialize( 0, 0, 0, 0, 1, 60, "../../resources/particle/vine.png", 87, 87, 0.3f );
			VineTwo.beginColour = Color.Orange * Color.Gray;
			VineTwo.endColour = ( Color.Green * Color.Gray * Color.Red );
			VineTwo.endColour.A = 0;
			VineTwo.particleShake = 4;
			VineTwo.particleStartRotation = ( VineSineTwo.Value * ( 180.0f / (float) Math.PI ) ) * 2;
			VineTwo.particleEndRotation = VineTwo.particleStartRotation;
			VineTwo.Start();
			Scene.Add( VineTwo );
			this.AddComponent( VineSineTwo );

			GroundTrail = new Otter.Image( "../../resources/particle/leaf.png" );
			GroundTrail.Color = Color.Green * Color.Gray * Color.Yellow;

			TrailBetween = 5;
			TrailBetweenRandom = 5;

		}

		public override void Update()
		{
			base.Update();

			// Main particles need to stay attached to the collider
			VineOne.X = X;
			VineOne.Y = Y - VineSineOne.Value * Height;

			VineTwo.X = X;
			VineTwo.Y = Y - VineSineTwo.Value * Height;

			// Randomize the colour of the trail ground leaves
			GroundTrail.Color.G = Color.Green.G + Rand.Float( -0.8f, -0.1f );

			// Reduce the height of the sine wave
			Height = Math.Max( HEIGHT_MIN, Height - 1 );
		}

		public override void Removed()
		{
			base.Removed();

			// Cleanup all particles
			VineOne.ClearGraphics();
			Scene.Remove( VineOne );
			VineOne = null;

			VineTwo.ClearGraphics();
			Scene.Remove( VineTwo );
			VineTwo = null;
		}
	}
}