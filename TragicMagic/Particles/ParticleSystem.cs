using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TragicMagic;

//@Author: J. Brown (DrMelon)
//@Date: 15/02/15
//@Purpose: This is a particle system for use in spells, etc. These can be loaded from and saved to files. 
//@Usage: To use in-game, instantiate a particlesystem object and set the parameters manually or load from file. Add to the scene, and call Start or Stop to start/stop emitting.

namespace TragicMagic.Particles
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

        // Particle image to use -- not loaded from file!
        public string imageSource;
        public int imageWidth;
        public int imageHeight;

        public float particleShake = 0.0f;

        // Particle lifetime
        public float particleLifetime = 30.0f;


        public ParticleSystem(float x = 0, float y = 0)
        {
            X = x;
            Y = y;
        }

        // Load from file
        public void InitializeFromFile(string fileName)
        {
            // lol no
        }

        // Create on the fly whoa
        public void Initialize(float emitdistance, float emitdistancejitter, float emitangle, float emitanglejitter, int emitamt, float life, string imgsource, int imgwidth, int imgheight)
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

                    newParticle.Graphic.Shake = particleShake;

                    // Add to scene.
                    this.Scene.Add(newParticle);
                    
                }
            }
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
