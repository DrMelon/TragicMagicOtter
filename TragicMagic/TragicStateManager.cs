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
		// Defines
		private const float ROUND_TIME = 10; // Time for a round to last, in seconds
		private const float SCORE_TIME = 5; // Time for a round's outcome to display for, in seconds
		private const float FADE_SPEED = 0.03f; // Speed at which to fade in/out the ground tiles

		// Reference to the current scene
		public Scene_GameClass CurrentScene;

		// Manage the state of the game
		private StateMachine<TragicState> TragicStateMachine = new StateMachine<TragicState>();

		// Hold a timer to time the length of each round (multiply to change the define to seconds)
		private AutoTimer RoundTime = new AutoTimer( ROUND_TIME * 60 );

		// Hold a timer to time the length of each round outcome display (multiply to change the define to seconds)
		private AutoTimer ScoreTime = new AutoTimer( SCORE_TIME * 60 );

		// The fade in/out animation for the ground
		private ClampedSpeedValueClass Ground_Alpha;

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

			// Add the game round timer to the manager
			AddComponent( RoundTime );
			AddComponent( ScoreTime );

			// Initalize first wizards to offscreen
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
				wizard.Pause = true; // Pause to stop player movement (nothing to do with Otter Entity pausing, still need some input)
			}

			// Initialize the fade in/out
			Ground_Alpha = new ClampedSpeedValueClass();
			{
				Ground_Alpha.Value = 0;
				Ground_Alpha.Minimum = 0;
				Ground_Alpha.Maximum = 1;
				Ground_Alpha.Speed = FADE_SPEED;
			}
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

					// Also ensure wizards cannot move on the title screen
					CurrentScene.Wizards[wizard].CanMove = false; // Lock into any animations
				}
			}
			if ( play || Game.Instance.Session( "DarkWizard" ).GetController<ControllerXbox360>().Start.Pressed ) // TODO: Remove temp button start
			{
				TragicStateMachine.ChangeState( TragicState.Game );
			}

			// Update the fade out
			Ground_Alpha.Direction = -1;
			Ground_Alpha.Update( false );
			foreach ( KeyValuePair<string, Entity> entity in CurrentScene.test.Entities )
			{
				foreach( Graphic graphic in entity.Value.Graphics )
				{
					graphic.Alpha = Ground_Alpha.Value;
				}
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
			CurrentScene.HUDHandler.AddTimer();
			CurrentScene.HUDHandler.AddScore();

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
				wizard.CanMove = false; // Lock into this animation
				wizard.Pause = false; // Unpause to allow player movement (nothing to do with Otter Entity pausing, still need some input)

				// Reset the wizard
				wizard.ComboInputs = "";
				wizard.Score = 0;
			}

			// Start game timer
			RoundTime.Start();

			// Show tilemap ground
			Ground_Alpha.Value = 0;
			Ground_Alpha.Direction = 1;
		}
		private void UpdateGame()
		{
			// Game timer logic
			{
				// Update the timer/score HUDs
				for ( short hud = 0; hud < HUDHandlerClass.HUDs; hud++ )
				{
					// Timer
					HUDElement_TimerClass timer = (HUDElement_TimerClass) CurrentScene.HUDHandler.HUDElement_Timer[hud];
					float time = ROUND_TIME - ( RoundTime.Value / 60 ); // Convert back to seconds
					timer.SetValue( time );

					// Score
					HUDElement_ScoreClass score = (HUDElement_ScoreClass) CurrentScene.HUDHandler.HUDElement_Score[hud];
					score.SetValue( CurrentScene.Wizards[hud].Score );
				}

				// End the round when the timer runs out
				if ( RoundTime.AtMax )
				{
					TragicStateMachine.ChangeState( TragicState.Score );
				}
			}

			// Update the fade in
			Ground_Alpha.Direction = 1;
			Ground_Alpha.Update( false );
			foreach ( KeyValuePair<string, Entity> entity in CurrentScene.test.Entities )
			{
				foreach( Graphic graphic in entity.Value.Graphics )
				{
					graphic.Alpha = Ground_Alpha.Value;
				}
			}
		}
		private void ExitGame()
		{
			CurrentScene.HUDHandler.RemoveCombo();
			CurrentScene.HUDHandler.RemoveTimer();
			CurrentScene.HUDHandler.RemoveScore();

			// Move wizards offscreen
			foreach ( WizardClass wizard in CurrentScene.Wizards )
			{
				float wizardoffset = -256; // Offscreen
				{
					if ( wizard.Graphic.Angle > 0 ) // Light player
					{
						wizardoffset = Game.Instance.Width - wizardoffset;
					}
				}
				wizard.Destination = new Vector2( wizardoffset, Game.Instance.HalfHeight );
				wizard.CanMove = false; // Lock into this animation
			}

			// Cleanup the game scene (spells, etc)
			CurrentScene.Reset();

			// Hide tilemap ground
			Ground_Alpha.Value = 1;
			Ground_Alpha.Direction = -1;
		}

		// State: Score
		private void EnterScore()
		{
			CurrentScene.HUDHandler.AddOutcome();

			// Start game timer
			ScoreTime.Start();
		}
		private void UpdateScore()
		{
			// Game timer logic
			{
				if ( ScoreTime.AtMax )
				{
					TragicStateMachine.ChangeState( TragicState.Menu );
				}
			}

			if ( Game.Instance.Session( "DarkWizard" ).GetController<ControllerXbox360>().Start.Pressed ) // TODO: Remove temp tweeting
			{
				
			}
		}
		private void ExitScore()
		{
			CurrentScene.HUDHandler.RemoveOutcome();
		}

        
	}
       
}