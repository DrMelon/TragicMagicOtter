using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
			if ( play )
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