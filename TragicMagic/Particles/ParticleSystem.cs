using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using TragicMagic;

//@Author: J. Brown (DrMelon)
//@Date: 15/02/15
//@Purpose: This is a particle system for use in spells, etc. These can be loaded from and saved to files. 
//@Usage: To use in-game, instantiate a particlesystem object and set the parameters manually or load from file. Add to the scene, and call Start or Stop to start/stop emitting.

namespace TragicMagic
{
    class ParticleSystem : Entity
    {
        
        // Emitting or not
        public bool isEmitting = false;

        // Emit "distance" - how far from origin along angle particles will drift
        public float emitDistance = 30.0f;

        // Emit distance randomness
        public float emitDistanceJitter = 10.0f;

        // Emit angle
        public float emitAngle = 0.0f;

        // Emit angle randomness
        public float emitAngleJitter = 15.0f;

        // Number of particles to emit per frame
        public int emitAmount = 1;

        // Start and End scale
        public float particleStartScale = 1.0f;
        public float particleEndScale = 1.0f;
        
        // Start and End rotation
        public float particleStartRotation = 0.0f;
        public float particleEndRotation = 0.0f;

        // Local/Global particle space
        public bool particleLocalSpace = false;

        // Inverse particle system - goes to the centre from the outside
        public bool particleMovementInverted = false;

        // Particle image to use -- not loaded from file!
        public string imageSource;
        public int imageWidth;
        public int imageHeight;

        public float particleShake = 0.0f;

        // Particle lifetime
        public float particleLifetime = 30.0f;

        // Animated particles
        bool particlesAnimated = false;
        int numParticleFrames = 1;
        int particleAnimLoops = 1;

		// Colour changes
		public Color beginColour;
		public Color endColour;

        // For local particle system
        public List<Particle> activeLocalParticles;
        public float oldX, oldY;

		// The angle of the particle system as a whole
		public float Angle = 0;
   

        public ParticleSystem(float x = 0, float y = 0)
        {
            X = x;
            Y = y;
            oldX = x;
            oldY = y;

			beginColour = Color.White;
			endColour = Color.White;

			activeLocalParticles = new List<Particle>();

			// Draw particles on top of all but the HUD
			Layer = 6;
        }

        // Create on the fly whoa
        public void Initialize(float emitdistance, float emitdistancejitter, float emitangle, float emitanglejitter, int emitamt, float life, string imgsource, int imgwidth, int imgheight, float imgscale, bool animated = false, int numframes = 1, int numloops = 1)
        {
            // Set variables yeah
            emitDistance = emitdistance;
            emitDistanceJitter = emitdistancejitter;
            emitAngle = emitangle;
            emitAngleJitter = emitanglejitter;
            emitAmount = emitamt;
            particleLifetime = life;
            imageSource = imgsource;
            imageWidth = imgwidth;
            imageHeight = imgheight;
            particleStartScale = imgscale;
            particlesAnimated = animated;
            numParticleFrames = numframes;
            particleAnimLoops = numloops;
        }

        public override void Update()
        {
            base.Update();


            if(isEmitting)
            {

                for (int i = 0; i < emitAmount; i++)
                {
                    // Create a particle!
                    Particle newParticle = new Particle(this.X, this.Y, imageSource, imageWidth, imageHeight);
                    newParticle.LifeSpan = particleLifetime;

                    // Figure out the final X and Y positions of this particle
                    // Based on angle, distance jitter etc
                    float thisAngle = this.emitAngle + (Rand.Float(-emitAngleJitter, emitAngleJitter));
                    float thisDistance = this.emitDistance + (Rand.Float(-emitDistanceJitter, emitDistanceJitter));

                    thisAngle = thisAngle * ((float)Math.PI / 180.0f);

                    newParticle.FinalX = this.X + (float)Math.Sin(thisAngle) * thisDistance;
                    newParticle.FinalY = this.Y + (float)Math.Cos(thisAngle) * thisDistance;

                    if(particleMovementInverted) // Move from outside inwards
                    {
                        // Can't pass properties by reference in C#
                        float finalX = newParticle.FinalX;
                        float finalY = newParticle.FinalY;
                        Utilities.Swap(ref finalX, ref this.X);
                        Utilities.Swap(ref finalY, ref this.Y);
                        newParticle.FinalX = finalX;
                        newParticle.FinalY = finalY;
                    }

                    newParticle.ScaleX = particleStartScale;
                    newParticle.ScaleY = particleStartScale;

                    newParticle.FinalScaleX = particleEndScale;
                    newParticle.FinalScaleY = particleEndScale;

					newParticle.Color = beginColour;
					newParticle.FinalColor = endColour;

					newParticle.Alpha = beginColour.A;
					newParticle.FinalAlpha = endColour.A;

                    newParticle.Angle = particleStartRotation;
                    newParticle.FinalAngle = particleEndRotation;

                    newParticle.Animate = particlesAnimated;
                    newParticle.FrameCount = numParticleFrames;
                    newParticle.FrameOffset = Rand.Int(0, numParticleFrames - 1);
                    newParticle.Loops = particleAnimLoops;

					// Draw particles on top of all but the HUD
					newParticle.Layer = 7;
                    newParticle.CenterOrigin = true;

					newParticle.Start(); // Initialize graphics
                    newParticle.Graphic.Shake = particleShake;

                    // Add to scene if global, and to system local space if local
                    //if (particleLocalSpace)
                   // {
                        activeLocalParticles.Add(newParticle);
                   // }
                    this.Scene.Add(newParticle);
                    
                }
            }



            // If we have local particles...
            if(particleLocalSpace)
            {
                foreach (Particle particle in activeLocalParticles)
                {
                    // Move the particles with the parent system
                    particle.FinalX += (X - oldX);
                    particle.FinalY += (Y - oldY);
					particle.Angle = Angle;
                }
            }

            // Update particle system's old position
            oldX = X;
            oldY = Y;

        }

        public void Start()
        {
            isEmitting = true;
        }

        public void Stop()
        {
            isEmitting = false;
        }

        

        


    }
}
