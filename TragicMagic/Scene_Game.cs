﻿using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Leap;

// Matthew Cormack @johnjoemcbob
// 13/02/15
// Main game scene, contains gameplay
// Depends on: Leap.Controller, GameWands, Wizard, HUDHandler, TragicStateManager, Spell

namespace TragicMagic
{
	enum ColliderType
	{
		Spell = 0, // Two values, one for each wizard
		Wizard = 2, // This and the values after are used for separate wizard tags
	}

	class Scene_GameClass : Scene
	{
		// Defines
		public const short WIZARDS = 2;
		private const float WAND_ROTATE_MAX = 70;

		// Store a reference to the LeapController object to pass to children
		public Leap.Controller LeapController;

		// Store a reference to the GameWands to query tool positions
		public GameWandsClass GameWands = new GameWandsClass();

		// Store the two wizard players
		public List<WizardClass> Wizards;

		// Store the handler of player HUDs
		public HUDHandlerClass HUDHandler;

		// The list of currently active projectiles
		public List<SpellClass> Projectile = new List<SpellClass>();

		// Store a state manager
		private TragicStateManagerClass TragicStateManager = new TragicStateManagerClass();

		// Ground image
		public Surface GroundSurface;
		private Otter.Image Ground;

		public Scene_GameClass()
		{
		}
		~Scene_GameClass()
		{
		}

		public void Initialize( Game game )
		{
			// Initialize the ground surface render target
			GroundSurface = new Surface( 1920, 1080 );
			GroundSurface.AutoClear = false;
			AddGraphic( GroundSurface );

			// Initialize the ground graphic (Added in ClearGround)
			Ground = new Otter.Image( "../../resources/ground.png" );

			// Set the reference to LeapController within the GameWands class & add to game update
			GameWands.LeapController = LeapController;
			GameWands.OnCast = OnCast;
			Add( GameWands );

			// Initialize the two wizard players
			float wizardoffset = 256;

			// Create new wizard list
			Wizards = new List<WizardClass>();
			{
				Wizards.Add( new WizardClass(
					game.Session( "LightWizard" ),
					GameWands,
					WizardTypeStruct.WIZARD_LIGHT,
					0,
					90
				) );

				// Dark wizard on the left
				Wizards.Add( new WizardClass(
					game.Session( "DarkWizard" ),
					GameWands,
					WizardTypeStruct.WIZARD_DARK,
					1,
					-90
				) );
			}

			// Add the wizards to the scene.
			foreach ( WizardClass wiz in Wizards )
			{
				wiz.CurrentScene = this;
				Add( wiz );
			}

			// Add a reference of this scene to the HUDHandler
			HUDHandler = new HUDHandlerClass( this );
			Add( HUDHandler );

			// Setup state manager last
			TragicStateManager.CurrentScene = this;
			Add( TragicStateManager );
		}

		public override void Update()
		{
			base.Update();

			// Update the wizard combos TODO: Move this inside WizardClass
			for ( short wizard = 0; wizard < WIZARDS; wizard++ )
			{
				HUDElement_ComboBarClass combobar = (HUDElement_ComboBarClass) HUDHandler.HUDElement_Combo[wizard];
				if ( combobar != null )
				{
					combobar.UpdateElements( Wizards[wizard].ComboInputs );
				}
			}

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

		// Reset any round specific game objects (i.e. spells) when the round ends
		// IN: N/A
		// OUT: N/A
		public void Reset()
		{
			foreach ( SpellClass spell in Projectile )
			{
				Remove( spell );
			}
			Projectile.Clear();
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

		// Scene callback for OnCast, sends cast command to wizards
		// IN: (wizard) The wizard casting the spell, (speed) The speed of the casting wand action
		//     (wand) The state of the wand at the time of casting
		// OUT: (int) Meaningless
		private int OnCast( short wizard, float speed, WandInformation wand )
		{
			Wizards[wizard].TryToCastSpell();
			return 0;
		}

		// Clears the ground surface render target at the start of a new round
		// IN: N/A
		// OUT: N/A
		public void ClearGround()
		{
			GroundSurface.Clear();
			GroundSurface.Draw( Ground );
		}
	}
}