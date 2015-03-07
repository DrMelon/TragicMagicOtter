using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TragicMagic;

// J. Brown @DrMelon
// 07/03/2015
// A HUD element which is shown when the player casts a spell. It is created, tweens upwards, and fades out before destroying itself.
// current player
// Depends on: HUDElement, ClampedSpeedValue, SpellInformation

namespace TragicMagic
{
    class HUDElement_SpellCastText : HUDElementClass
    {
		// Defines
		private const float FADE_SPEED = 0.03f;

		// The text displaying the spell's name
		private Otter.Text Text_SpellName;
        private String spellName;

		// The clamped value of the fade amount 
		private ClampedSpeedValueClass Alpha;

		// The flag for fading out this element when removed
		private bool FadeOut = false;

        public HUDElement_SpellCastText(Scene scene_current, float x = 0, float y = 0, SpellInformation spellCast = null) : base(scene_current)
        {
            X = x;
            Y = y;
            LifeSpan = 45f * 2; // Only last for 45 frames -- begin fading out 
            if (spellCast != null)
            {
                spellName = spellCast.spellName;
            }
            else
            {
                spellName = "No Name For Spell!";
            }
        }

        public override void Added()
        {
            base.Added();

            Text_SpellName = new Otter.Text(spellName, 48);
            {
                Text_SpellName.X = X;
                Text_SpellName.Y = Y;
                Text_SpellName.CenterOrigin();
                Text_SpellName.OutlineColor = Color.Black;
                Text_SpellName.OutlineThickness = 2;
            }
            Parent.AddGraphic(Text_SpellName);
            // Initialize the fade in/out
            Alpha = new ClampedSpeedValueClass();
            {
                Alpha.Value = 0;
                Alpha.Minimum = 0;
                Alpha.Maximum = 1;
                Alpha.Speed = FADE_SPEED;
            }

            // Update the images to have this initial alpha value
            foreach (Graphic graphic in Parent.Graphics)
            {
                graphic.Alpha = Alpha.Value;
            }
        }
        public override void Update()
        {
            base.Update();

            // Move up a little bit.
            X += (float)Math.Sin(this.Graphic.Angle) * -1f;

            if(Timer > 45)
            {
                FadeOut = true;
            }

            if (FadeOut) // Fade out at the end of the animation
            {
                Alpha.Update();

                // Update the images to have this new alpha value
                foreach (Graphic graphic in Parent.Graphics)
                {
                    graphic.Alpha = Alpha.Value;
                }

                // Remove from scene when done
                if (Alpha.Value <= 0)
                {
                    CurrentScene.Remove(this);
                }
            }
            else // Fade in at the start of the animation
            {
                if (Parent.Graphic.Alpha < 1) // Still fading in
                {
                    Alpha.Update();

                    // Update the images to have this new alpha value
                    foreach (Graphic graphic in Parent.Graphics)
                    {
                        graphic.Alpha = Alpha.Value;
                    }
                }
            }
        }

        // Return whether or not the element should actually be removed at this point
        // NOTE: Useful for fade out animations, etc
        // IN: N/A
        // OUT: (bool) True to remove from scene
        public override bool Remove()
        {
            Alpha.Value = 1; // This element was giving trouble by fading twice on exit, alpha wasn't set right for some reason
            FadeOut = true;
            return false;
        }
    }
}
