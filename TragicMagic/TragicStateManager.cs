using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Matthew Cormack @johnjoemcbob
// 17/02/15
// Handles the individual unique state logic & switching between them
// Depends on: Scene_Game

namespace TragicMagic
{
	// The states describing Tragic Magic
	enum TragicState
	{
		Menu, // The first state, displays credits & asks the users to touch wands to begin
		Game, // The main state, with gameplay logic
		Score, // The battle end state, which displays the score of both players & the winner
		Twitter // The tweet state, allowing players to tweet their battleground
	}

	class TragicStateManagerClass : Entity
	{
		// Reference to the current scene
		public Scene_GameClass CurrentScene;

		// Manage the state of the game
		private StateMachine<TragicState> TragicStateMachine = new StateMachine<TragicState>();

		public TragicStateManagerClass()
		{

		}

		~TragicStateManagerClass()
		{

		}

		public override void Added()
		{
			// Add state machine component
			AddComponent( TragicStateMachine );

			// Initialize to the menu state
			TragicStateMachine.ChangeState( TragicState.Menu );
		}

		// State: Menu
		private void EnterMenu()
		{
			CurrentScene.HUDHandler.AddTeam();

			// Reinitalize wizards to offscreen
			foreach ( WizardClass wizard in CurrentScene.Wizards )
			{
				float wizardoffset = -256; // Offscreen
				{
					if ( wizard.Graphic.Angle > 0 ) // Light player
					{
						wizardoffset = Game.Instance.Width - wizardoffset;
					}
				}
				wizard.SetPosition( wizardoffset, Game.Instance.HalfHeight );
				wizard.Destination = new Vector2( wizard.X, wizard.Y );
			}
		}
		private void UpdateMenu()
		{
			bool play = true; // Initialize both players to detected, so game can begin
			{
				for ( short wizard = 0; wizard < Scene_GameClass.WIZARDS; wizard++ )
				{
					float time = CurrentScene.GameWands.Wand[wizard].Time_Recorded; // Last time the wand was detected
					float difference = Game.Instance.Timer - time; // The difference in time between now and the last time recorded
					if ( ( time == 0 ) || ( difference > GameWandsClass.CAST_BETWEEN ) ) // Hasn't been detected, or last time detected was a while ago
					{
						play = false; // Player undetected, game cannot begin
					}
				}
			}
			if ( play || Game.Instance.Session( "LightWizard" ).Controller.A.Pressed ) // TODO: Remove temp button start
			{
				TragicStateMachine.ChangeState( TragicState.Game );
			}
		}
		private void ExitMenu()
		{
			CurrentScene.HUDHandler.RemoveTeam();
		}

		// State: Game
		private void EnterGame()
		{
			CurrentScene.HUDHandler.AddCombo();

			// Move wizards towards starting point
			foreach ( WizardClass wizard in CurrentScene.Wizards )
			{
				float wizardoffset = 256; // Onscreen
				{
					if ( wizard.Graphic.Angle > 0 ) // Light player
					{
						wizardoffset = Game.Instance.Width - wizardoffset;
					}
				}
				wizard.Destination = new Vector2( wizardoffset, Game.Instance.HalfHeight );
			}
		}
		private void UpdateGame()
		{
			
		}
		private void ExitGame()
		{
			CurrentScene.HUDHandler.RemoveCombo();
		}
	}
}