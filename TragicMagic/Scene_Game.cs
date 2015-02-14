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
// Depends on: Leap.Controller, GameWands, Wizard, HUDHandler

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

		// Store the two wizard players
		private List<WizardClass> Wizards;
		// Store the players' input strings.
		private String lightWizardInput = "";
		private String darkWizardInput = "";
		//TODO: Input strings should probably exist inside wizard class,
		// along with a reference to the relevant session.

		// Store the handler of player HUDs
		private HUDHandlerClass HUDHandler;

		public Scene_GameClass()
		{
		}

		~Scene_GameClass()
		{
		}

		public void Initialize( Game game )
		{
			// Set the reference to LeapController within the GameWands class & add to game update
			GameWands.LeapController = LeapController;
			Add( GameWands );

			// Initialize the two wizard players
			float wizardoffset = 256;

			// Create new wizard list
			Wizards = new List<WizardClass>();

			Wizards.Add( new WizardClass(
				game.Session( "LightWizard" ),
				GameWands,
				WizardTypeStruct.WIZARD_LIGHT,
				new Vector2( Game.Instance.Width - wizardoffset, Game.Instance.HalfHeight ),
				90
			) );

			// Dark wizard on the left
			Wizards.Add( new WizardClass(
				game.Session( "DarkWizard" ),
				GameWands,
				WizardTypeStruct.WIZARD_DARK,
				new Vector2( wizardoffset, Game.Instance.HalfHeight ),
				-90
			) );

			// Add the wizards to the scene.
			foreach ( WizardClass wiz in Wizards )
			{
				Add( wiz );
			}

			// Add a reference of this scene to the HUDHandler
			HUDHandler = new HUDHandlerClass( this );
		}

		public override void Update()
		{
			base.Update();

			// Update the rotation of the wand based on Leap tool tracking
			foreach ( WizardClass wiz in Wizards )
			{
				int wand = Wizards.IndexOf( wiz );
				wiz.WandDirection = GameWands.Wand[wand].Direction;
				wiz.WandAngle = GameWands.Wand[wand].Direction.X * WAND_ROTATE_MAX;
			}

			// Display errors if the Leap device is missing
			Update_CheckLeap();
		}

		// Check the count of Leap Motion Devices & display a warning if there are none
		// NOTE: Called from main game update every frame
		// IN: N/A
		// OUT: N/A
		private void Update_CheckLeap()
		{
			if ( ( LeapController == null ) || ( LeapController.Devices.Count == 0 ) ) // No devices connected
			{
				HUDHandler.AddLeapWarning();
			}
			else // Atleast 1 device connected
			{
				HUDHandler.RemoveLeapWarning();
			}
		}
	}
}