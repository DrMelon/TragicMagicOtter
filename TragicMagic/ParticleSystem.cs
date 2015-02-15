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

namespace TragicMagic
{
    class ParticleSystem : Entity
    {
        // List of particles still in-scene.
        List<Particle> livingParticles;



        public ParticleSystem()
        {

        }


    }
}
