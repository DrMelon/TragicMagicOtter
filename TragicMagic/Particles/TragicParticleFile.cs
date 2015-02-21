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
 * First 4 Bytes: "TPSF"
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
 * TOTAL SIZE: 32 Bytes
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
			if (fileHandle.CanRead && fileHandle.Length == 32)
			{
				// Read first 4 bytes of file, check header.
				int filePosition = 0;
				byte[] header = new byte[4];
				fileHandle.Read(header, filePosition, 4);
				string headerString = System.Text.Encoding.Default.GetString(header);

				if (headerString == "TPSF")
				{
                    filePosition += 4;

					// Read Emit Distance
					byte[] emitDistance = new byte[4];
					fileHandle.Read(emitDistance, 0, 4);
					particleSystemInfo.emitDistance = System.BitConverter.ToSingle(emitDistance, 0);
					filePosition += 4;

					// Read Emit Jitter
					byte[] emitJitter = new byte[4];
					fileHandle.Read(emitDistance, 0, 4);
					particleSystemInfo.emitDistanceJitter = System.BitConverter.ToSingle(emitJitter, 0);
                    filePosition += 4;

                    // Read Emit Angle
                    byte[] emitAngle = new byte[4];
                    fileHandle.Read(emitAngle, 0, 4);
                    particleSystemInfo.emitAngle = System.BitConverter.ToSingle(emitAngle, 0);
                    filePosition += 4;

                    // Read Emit Angle Jitter
                    byte[] emitAngleJitter = new byte[4];
                    fileHandle.Read(emitAngleJitter, 0, 4);
                    particleSystemInfo.emitAngleJitter = System.BitConverter.ToSingle(emitAngleJitter, 0);
                    filePosition += 4;

                    // Read Emit Amount
                    byte[] emitAmount = new byte[4];
                    fileHandle.Read(emitAmount, 0, 4);
                    particleSystemInfo.emitAmount = System.BitConverter.ToInt16(emitAmount, 0);
                    filePosition += 4;

                    // Read Particle Shake Amount
                    byte[] particleShake = new byte[4];
                    fileHandle.Read(particleShake, 0, 4);
                    particleSystemInfo.particleShake = System.BitConverter.ToSingle(particleShake, 0);
                    filePosition += 4;

                    // Read Particle Lifetime
                    byte[] particleLifetime = new byte[4];
                    fileHandle.Read(particleLifetime, 0, 4);
                    particleSystemInfo.particleLifetime = System.BitConverter.ToSingle(particleLifetime, 0);
                    



				}

			}

		}

		public void SaveFile()
		{
            // Attempt to write file out
            if(fileHandle.CanWrite)
            {
                fileHandle.SetLength(32);
                int filePosition = 0;

                // Write header
                byte[] header = new byte[4];
                header = System.Text.Encoding.Default.GetBytes("TPSF");
                fileHandle.Write(header, 0, 4);
                filePosition += 4;

                // Write Emit Distance
                byte[] emitDistance = new byte[4];
                emitDistance = System.BitConverter.GetBytes(particleSystemInfo.emitDistance);
                fileHandle.Write(emitDistance, 0, 4);
                filePosition += 4;

                // Write Emit Jitter
                byte[] emitJitter = new byte[4];
                emitJitter = System.BitConverter.GetBytes(particleSystemInfo.emitDistanceJitter);
                fileHandle.Write(emitJitter, 0, 4);
                filePosition += 4;

                // Write Emit Angle
                byte[] emitAngle = new byte[4];
                emitAngle = System.BitConverter.GetBytes(particleSystemInfo.emitAngle);
                fileHandle.Write(emitAngle, 0, 4);
                filePosition += 4;

                // Write Emit Angle Jitter
                byte[] emitAngleJitter = new byte[4];
                emitAngleJitter = System.BitConverter.GetBytes(particleSystemInfo.emitAngleJitter);
                fileHandle.Write(emitAngleJitter, 0, 4);
                filePosition += 4;

                // Write Emit Amount
                byte[] emitAmount = new byte[4];
                emitAmount = System.BitConverter.GetBytes(particleSystemInfo.emitAmount);
                fileHandle.Write(emitAmount, 0, 4);
                filePosition += 4;

                // Write Particle Shake Amount
                byte[] particleShake = new byte[4];
                particleShake = System.BitConverter.GetBytes(particleSystemInfo.particleShake);
                fileHandle.Write(particleShake, 0, 4);
                filePosition += 4;

                // Write Particle Lifetime
                byte[] particleLifetime = new byte[4];
                particleLifetime = System.BitConverter.GetBytes(particleSystemInfo.particleLifetime);
                fileHandle.Write(particleLifetime, 0, 4);
                       



            }
		}

		ParticleSystem GetParticleSystem()
		{
			return particleSystemInfo;
		}

        void SetNewParticleSystem(ParticleSystem newParticles)
        {
            particleSystemInfo = newParticles;
        }

		//Destructor closes file.
		~TragicParticleFile()
		{
			fileHandle.Close();
		}
	}
}