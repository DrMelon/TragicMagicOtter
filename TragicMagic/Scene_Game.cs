using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Leap;

// Matthew Cormack @johnjoemcbob
// 13/02/15
// Main game scene, contains gameplay
// Depends on: Leap.Controller, GameWands, PulseCircle, Wizard

namespace TragicMagic
{
	class Scene_GameClass : Scene
	{
		// Defines
		private const short WIZARDS = 2;
		private const float WAND_ROTATE_MAX = 45;

		// Store a reference to the LeapController object to pass to children
		public Leap.Controller LeapController;

		// Store a reference to the GameWands to query tool positions
		public GameWandsClass GameWands = new GameWandsClass();

		private PulseCircleClass LeapWarning = null;

		// Store the two wizard players
        private List<WizardClass> Wizards;
        // Store the players' input strings.
        private String lightWizardInput = "";
        private String darkWizardInput = "";
        //TODO: Input strings should probably exist inside wizard class,
        // along with a reference to the relevant session.

		// Store the two wizard HUDs
		//private HUDClass[] HUD;

		public Scene_GameClass()
		{
			Initialize();
		}

		~Scene_GameClass()
		{
		}

		public void Initialize()
		{
			// Set the reference to LeapController within the GameWands class & add to game update
			GameWands.LeapController = LeapController;
			Add( GameWands );

			// Initialize the two wizard players
			float wizardoffset = 256;

            // Create new wizard list
            Wizards = new List<WizardClass>();

			// Light wizard on the right
			Wizards.Add(new WizardClass(
				GameWands,
				WizardTypeStruct.WIZARD_LIGHT,
				new Vector2( Game.Instance.Width - wizardoffset, Game.Instance.HalfHeight ),
				90
			));

			// Dark wizard on the left
			Wizards.Add(new WizardClass(
				GameWands,
				WizardTypeStruct.WIZARD_DARK,
				new Vector2( wizardoffset, Game.Instance.HalfHeight ),
				-90
			));

            // Add the wizards to the scene.
            foreach(WizardClass wiz in Wizards)
            {
                Add(wiz);
            }

            


			// Initialize grid of pulsing circles
			//int radius = 6;
			//int size = 16;
			//int offsetx = Game.Instance.HalfWidth;
			//int offsety = Game.Instance.HalfHeight;
			//for ( int x = -radius; x < radius; x++ )
			//{
			//	for ( int y = -radius; y < radius; y++ )
			//	{
			//		float speed = ( x * y ) / radius;
			//		PulseCircleClass pulser = new PulseCircleClass( ( x * size ) + offsetx, ( y * size ) + offsety, speed );
			//		{
			//			pulser.LeapController = LeapController;
			//		}
			//		Add( pulser );
			//	}
			//}
		}

		public override void Update()
		{
			base.Update();

			// Update the rotation of the wand based on Leap tool tracking
            foreach(WizardClass wiz in Wizards)
            {
                // Suggest changing GameWands.Wand[] to be a list too, but for now this is OK
                wiz.WandDirection = GameWands.Wand[Wizards.IndexOf(wiz)].Direction;
                wiz.WandAngle = GameWands.Wand[Wizards.IndexOf(wiz)].Direction.X * WAND_ROTATE_MAX;
            }

          

			// Display errors if the Leap device is missing
			Update_CheckLeap();

            // Update keypresses
            Update_CheckControls();
		}

		// Check the count of Leap Motion Devices & display a warning if there are none
		// NOTE: Called from main game update every frame
		// IN: N/A
		// OUT: N/A
		private void Update_CheckLeap()
		{
			if ( ( LeapController == null ) || ( LeapController.Devices.Count == 0 ) ) // No devices connected
			{
				if ( LeapWarning == null ) // Does not exist yet
				{
					LeapWarning = new PulseCircleClass( 250, 250, 100 ); // Placeholder for actual warning graphic
					Add( LeapWarning );
				}
			}
			else // Atleast 1 device connected
			{
				if ( LeapWarning != null ) // Not already disposed of
				{
					Remove( LeapWarning );
					LeapWarning = null; // Disregard reference to pulser, flag for garbage collect
				}
			}
		}


        // Check the player inputs, see if anything has been pressed.
        // Called from main game update every frame.
        // IN: N/A
        // OUT: N/A
        private void Update_CheckControls()
        {
            // Checking Element Buttons (Only When Just Pressed)
            if (this.Game.Session("LightWizard").Controller.A.Pressed)
            {
                lightWizardInput += "A";
            }
            if (this.Game.Session("LightWizard").Controller.B.Pressed)
            {
                lightWizardInput += "B";
            }
            if (this.Game.Session("LightWizard").Controller.X.Pressed)
            {
                lightWizardInput += "X";
            }
            if (this.Game.Session("LightWizard").Controller.Y.Pressed)
            {
                lightWizardInput += "Y";
            }
            if (this.Game.Session("DarkWizard").Controller.A.Pressed)
            {
                darkWizardInput += "A";
            }
            if (this.Game.Session("DarkWizard").Controller.B.Pressed)
            {
                darkWizardInput += "B";
            }
            if (this.Game.Session("DarkWizard").Controller.X.Pressed)
            {
                darkWizardInput += "X";
            }
            if (this.Game.Session("DarkWizard").Controller.Y.Pressed)
            {
                darkWizardInput += "Y";
            }

            // Checking Movement (While Held)
            if(this.Game.Session("LightWizard").Controller.Left.Down)
            {
                //TODO: Move Light Wizard Left. Movement funcs not implemented yet.
            }
            if (this.Game.Session("LightWizard").Controller.Right.Down)
            {
                //TODO: Move Light Wizard Right. Movement funcs not implemented yet.
            }
            if (this.Game.Session("LightWizard").Controller.Up.Down)
            {
                //TODO: Move Light Wizard Up. Movement funcs not implemented yet.
            }
            if (this.Game.Session("LightWizard").Controller.Down.Down)
            {
                //TODO: Move Light Wizard Down. Movement funcs not implemented yet.
            }
            if (this.Game.Session("DarkWizard").Controller.Left.Down)
            {
                //TODO: Move Dark Wizard Left. Movement funcs not implemented yet.
            }
            if (this.Game.Session("DarkWizard").Controller.Right.Down)
            {
                //TODO: Move Dark Wizard Right. Movement funcs not implemented yet.
            }
            if (this.Game.Session("DarkWizard").Controller.Up.Down)
            {
                //TODO: Move Dark Wizard Up. Movement funcs not implemented yet.
            }
            if (this.Game.Session("DarkWizard").Controller.Down.Down)
            {
                //TODO: Move Dark Wizard Down. Movement funcs not implemented yet.
            }


            // Update Input strings to only be 10 in length; trim off the leading characters
            if(lightWizardInput.Length > 10)
            {
                lightWizardInput = lightWizardInput.Substring(1, 10);
            }
            if (darkWizardInput.Length > 10)
            {
                darkWizardInput = darkWizardInput.Substring(1, 10);
            }

            //DEBUG: Checking combo system works - writes to debugger, open with ~ or ` key
            SpellInformation whatSpell = ComboSystem.Instance.CheckSpell(lightWizardInput);
            if(whatSpell != null)
            {
               Debugger.Log("", whatSpell.spellName + " just got cast!\n");
            }
            


        }


        


	}
}