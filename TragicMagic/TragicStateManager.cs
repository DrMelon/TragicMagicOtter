using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

	struct MatchOutcome
	{
		public float Score;
		public bool Winner;
	}

	class TragicStateManagerClass : Entity
	{
		// Defines
		private const float ROUND_TIME = 60; // Time for a round to last, in seconds
		private const float SCORE_TIME = 5; // Time for a round's outcome to display for, in seconds
		private const float FADE_SPEED = 0.03f; // Speed at which to fade in/out the ground tiles

		// Reference to the current scene
		public Scene_GameClass CurrentScene;

        // Reference to screen shaker
        public ScreenShaker ScreenShake;

		// Manage the state of the game
		private StateMachine<TragicState> TragicStateMachine = new StateMachine<TragicState>();

		// Hold a timer to time the length of each round (multiply to change the define to seconds)
		private AutoTimer RoundTime = new AutoTimer( ROUND_TIME * 60 );

		// Hold a timer to time the length of each round outcome display (multiply to change the define to seconds)
		private AutoTimer ScoreTime = new AutoTimer( SCORE_TIME * 60 );

		// The fade in/out animation for the ground
		private ClampedSpeedValueClass Ground_Alpha;

		// The tweet finished flag for each wizard
		private bool[] TweetFinished;

		// The match outcome for each wizard (saved for the Twitter state)
		private MatchOutcome[] Outcome;

		// The thread to handle tweeting the match image & outcome
		private Thread TweetThread;

		public TragicStateManagerClass()
		{

		}

		~TragicStateManagerClass()
		{
			// Wait for the image to be tweeted before cleaning up
			if ( TweetThread != null )
			{
				TweetThread.Join();
			}
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

            // Initialize the camera shaker
            ScreenShake = new ScreenShaker();
            this.Scene.Add(ScreenShake);

			// Initialize the tweet finished flag for the wizards
			TweetFinished = new bool[Scene_GameClass.WIZARDS];
			{
				for ( short wizard = 0; wizard < Scene_GameClass.WIZARDS; wizard++ )
				{
					TweetFinished[wizard] = false;
				}
			}

			// Initialize the match outcome for the wizards
			Outcome = new MatchOutcome[Scene_GameClass.WIZARDS];
		}

		// State: Menu
		private void EnterMenu()
		{
			CurrentScene.HUDHandler.AddTeam();
			CurrentScene.ClearGround();
			
			// Move wizards offscreen
			foreach ( WizardClass wizard in CurrentScene.Wizards )
			{
				wizard.Pause = false; // Unpause to allow player movement (nothing to do with Otter Entity pausing, still need some input)
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
			CurrentScene.GroundSurface.Alpha = Ground_Alpha.Value;
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
			CurrentScene.GroundSurface.Alpha = Ground_Alpha.Value;
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
				wizard.Pause = true; // Pause to stop major input and update of the player (nothing to do with Otter Entity pausing, still need some input)
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

			// Save screenshot
			TweetinviClass.SaveScreenshot();
		}
		private void UpdateScore()
		{
			// Game timer logic
			{
				if ( ScoreTime.AtMax )
				{
					TragicStateMachine.ChangeState( TragicState.Twitter );
				}
			}
		}
		private void ExitScore()
		{
			// Save match outcome for each wizard (for Tweet state)
			for ( short wizard = 0; wizard < Scene_GameClass.WIZARDS; wizard++ )
			{
				HUDElement_OutcomeClass outcome = (HUDElement_OutcomeClass) CurrentScene.HUDHandler.HUDElement_Outcome[wizard];
				Outcome[wizard].Score = outcome.Score;
				Outcome[wizard].Winner = outcome.Winner;
			}

			CurrentScene.HUDHandler.RemoveOutcome();
		}

		// State: Twitter
		private void EnterTwitter()
		{
			CurrentScene.HUDHandler.AddKeyboard();

			for ( short wizard = 0; wizard < Scene_GameClass.WIZARDS; wizard++ )
			{
				TweetFinished[wizard] = false;
			}
		}
		private void UpdateTwitter()
		{
			// Handle input from each wizard to set finished tweeting
			for ( short wizard = 0; wizard < Scene_GameClass.WIZARDS; wizard++ )
			{
				// Get the controller for this wizard
				ControllerXbox360 controller = Game.Instance.Sessions[wizard].GetController<ControllerXbox360>();
				if ( controller.Y.Pressed )
				{
					TweetFinished[wizard] = !TweetFinished[wizard];
				}
			}

			// Handle switching back to the menu state when all wizards are finished
			bool finished = true;
			{
				for ( short wizard = 0; wizard < Scene_GameClass.WIZARDS; wizard++ )
				{
					if ( !TweetFinished[wizard] )
					{
						finished = false;
						break;
					}
				}
			}
			if ( finished )
			{
				TragicStateMachine.ChangeState( TragicState.Menu );
			}
		}
		private void ExitTwitter()
		{
			// Cleanup old thread
			if ( TweetThread != null )
			{
				TweetThread.Abort();
				TweetThread.Join();
				TweetThread = null;
			}

			// Start the thread to tweet the outcome
			TweetThread = new Thread( new ThreadStart( this.TweetInThread ) );
			TweetThread.Start();
		}

		private void TweetInThread()
		{
			// Setup tweet text
			HUDElement_KeyboardClass[] keyboard = new HUDElement_KeyboardClass[2];
			{
				keyboard[0] = (HUDElement_KeyboardClass) CurrentScene.HUDHandler.HUDElement_Keyboard[0];
				keyboard[1] = (HUDElement_KeyboardClass) CurrentScene.HUDHandler.HUDElement_Keyboard[1];
			}
			string tweet = "";
			{
				// 1. First wizard's twitter handle
				{
					string username = "Player 1";
					if ( keyboard[1].UserString.Length > 0 )
					{
						username = "@" + keyboard[1].UserString;
					}
					tweet += "" + username;
				}
				// 2. First wizard's score
				{
					tweet += " (" + Outcome[1].Score + ")";
				}
				// 3. Match outcome
				{
					if ( Outcome[1].Winner )
					{
						tweet += " beat";
					}
					else if ( Outcome[0].Winner )
					{
						tweet += " lost to";
					}
					else
					{
						tweet += " drew with";
					}
				}
				// 4. Second wizard's twitter handle
				{
					string username = "Player 2";
					if ( keyboard[0].UserString.Length > 0 )
					{
						username = "@" + keyboard[0].UserString;
					}
					tweet += " " + username;
				}
				// 5. Second wizard's score
				{
					tweet += " (" + Outcome[0].Score + ")!";
				}
			}
			// Remove HUD keyboards
			CurrentScene.HUDHandler.RemoveKeyboard();

			// Tweet the image & text
			TweetinviClass.TweetImage( tweet );
		}
	}
}