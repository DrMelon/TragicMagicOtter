using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;
using Otter.UI;

// J. Brown (@DrMelon)
// 07/03/2015
// This is an object that shakes the camera when Shake() is called.
// Depends on: Otter

namespace TragicMagic
{
    public class ScreenShaker : Entity
    {
        // Variables that will store our camera X,Y coordinates before shaking
        private float priorCameraX = 0f;
        private float priorCameraY = 0f;

        // Variable used to keep track of how long we have been shaking for
        private float shakeTimer = 0f;
        // Number of frames to shake the camera for. Gets set in constructor
        private float shakeFrames = 0f;
        // Bool used to determine if the camera needs shaking or not
        private bool shakeCamera = false;
        // Shake strength. Determines the power of a shake.
        private float shakeStrength = 2f;

        // Default constructor
        public ScreenShaker()
        {
        }

        public void ShakeCamera(float shakeDur = 20f, float shakeStr = 2f)
        {
            // If camera isn't already shaking
            if (!shakeCamera)
            {
                // Save our original X,Y values
                priorCameraX = this.Scene.CameraX;
                priorCameraY = this.Scene.CameraY;

                // Set shakeCamera to true, and our shake duration
                shakeCamera = true;
                shakeFrames = shakeDur;
                shakeStrength = shakeStr;
            }
        }

        public override void UpdateLast()
        {
            if (shakeCamera)
            {
                // Move the Camera X,Y values a random, but controlled amount
                this.Scene.CameraX = priorCameraX + (10 - 6 * shakeStrength * Rand.Float(-1, 1));
                this.Scene.CameraY = priorCameraY + (10 - 6 * shakeStrength * Rand.Float(-1, 1));

                // Increase the shake timer by one frame
                // and check if we have been shaking long enough
                shakeTimer++;
                if (shakeTimer >= shakeFrames)
                {
                    shakeCamera = false;
                    shakeTimer = 0;
                    shakeFrames = 0;

                    this.Scene.CameraX = priorCameraX;
                    this.Scene.CameraY = priorCameraY;
                }
            }
        }
    }
}

