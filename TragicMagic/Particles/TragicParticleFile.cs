using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using TragicMagic;

//@Author: J. Brown / DrMelon
//@Date: 21/02/15
//@Purpose: This class outlines an interface for loading and saving Particle Systems to Tragic Particle System Files (.tps files)
//@Usage: A TragicParticleFile object represents a .tps file you want to deal with; a file is opened when the constructor is called, and by altering the ParticleSystem object inside
// the class, the user can then save back out to the file.

/* Tragic Particle System File Definition
 * 
 * 
 * - HEADER -
 * First 8 Bytes: "TPSF"
 * 
 * - DATA - 
 * 4 Bytes, Float, Emit Distance
 * 4 Bytes, Float, Emit Jitter
 * 4 Bytes, Float, Emit Angle
 * 4 Bytes, Float, Emit Angle Jitter
 * 4 Bytes, Int,   Emit Amount
 * 4 Bytes, Float, Particle Shake
 * 4 Bytes, Float, Particle Lifespan
 * 
 * TOTAL SIZE: 36 Bytes
 *
 */
 



namespace TragicMagic.Particles
{
    class TragicParticleFile
    {
        // Internal particlesystem that gets modified
        ParticleSystem particleSystemInfo = null;

        // File handle
        FileStream fileHandle = null;

        public TragicParticleFile(string filename)
        {
            // Attempt to open or create a file with this filename.
            fileHandle = new FileStream(filename, FileMode.OpenOrCreate);

            // Create new particle system
            particleSystemInfo = new ParticleSystem(0, 0);
        }

        public void LoadFile()
        {
            // Attempt to read the .tps file.
            if (fileHandle.CanRead && fileHandle.Length == 36)
            {
                // Read first 4 bytes of file, check header.
                int filePosition = 0;
                byte[] header = new byte[8];
                fileHandle.Read(header, filePosition, 8);
                string headerString = System.Text.Encoding.Default.GetString(header);

                if(headerString == "TPSF")
                {
                    filePosition += 8;

                    // Read Emit Distance
                    byte[] emitDistance = new byte[4];
                    fileHandle.Read(emitDistance, filePosition, 4);
                    particleSystemInfo.emitDistance = System.BitConverter.ToSingle(emitDistance, 0);
                    filePosition += 4;

                    // Read Emit Jitter
                    byte[] emitJitter = new byte[4];
                    fileHandle.Read(emitDistance, filePosition, 4);
                    particleSystemInfo.emitDistanceJitter = System.BitConverter.ToSingle(emitJitter, 0);


                }

            }

        }

        public void SaveFile()
        {

        }

        ParticleSystem GetParticleSystem()
        {
            return particleSystemInfo;
        }

        //Destructor closes file.
        public ~TragicParticleFile()
        {
            fileHandle.Close();
        }


    }
}
