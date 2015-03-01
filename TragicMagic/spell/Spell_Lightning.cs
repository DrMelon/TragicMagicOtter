using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// J. Brown @DrMelon
// 1/03/2015
// Simple lightning spell projectile.
// Depends on: Spell

namespace TragicMagic
{
    class Spell_LightningClass : SpellClass
    {
        // The main particle system acts like a fireball, so that the spell's motion/travel is easy to see.
        private ParticleSystem Particle_Lightning;


        // The lightning crackle is a smaller subsystem of particles, with animated lightning bolts
        private ParticleSystem Particle_LightningCrackle;

        public Spell_LightningClass()
            : base(0, 0, 0, new Vector2(0, 0), 0)
        {

        }
        public Spell_LightningClass(int wizard, float x, float y, Vector2 direction, float speed = 2)
            : base(wizard, x, y, direction, speed)
        {

        }
        ~Spell_LightningClass()
        {

        }

        public override void Added()
        {
            base.Added();

            // Create main spell particle system
            Particle_Lightning = new ParticleSystem(X, Y);
            Particle_Lightning.Initialize(0, 0, 0, 360, 5, 10, "../../resources/particle/star.png", 87, 87, 0.8f);
            Particle_Lightning.beginColour = Color.Cyan;
            Particle_Lightning.endColour = (Color.Blue * Color.Gray * Color.Gray);
            Particle_Lightning.endColour.A = 0;
            Particle_Lightning.particleShake = 4;
            Particle_Lightning.Start();
            Scene.Add(Particle_Lightning);


            // Add crackle
            Particle_LightningCrackle = new ParticleSystem(X, Y);
            Particle_LightningCrackle.Initialize(40, 40, 0, 360, 4, 5, "../../resources/particle/crackle_sheet.png", 87, 87, 0.2f, true, 6, 4);
            Particle_LightningCrackle.beginColour = Color.Cyan;
            Particle_LightningCrackle.endColour = Color.White;
            Particle_LightningCrackle.endColour.A = 0;
            Particle_LightningCrackle.Start();
            Scene.Add(Particle_LightningCrackle);
        }

        public override void Update()
        {
            base.Update();

            // Main fireball particles need to stay attached to the collider
            Particle_Lightning.X = X;
            Particle_Lightning.Y = Y;

            Particle_LightningCrackle.X = X;
            Particle_LightningCrackle.Y = Y;

        }

        public override void Removed()
        {
            base.Removed();

            // Cleanup all particles
            Particle_Lightning.ClearGraphics();
            Scene.Remove(Particle_Lightning);
            Particle_Lightning = null;

            Particle_LightningCrackle.ClearGraphics();
            Scene.Remove(Particle_LightningCrackle);
            Particle_LightningCrackle = null;
        }
    }
}