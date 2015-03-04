using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Matthew Cormack @johnjoemcbob
// 14/02/15
// Handle HUDs for both players, each element added to this will be added to both HUDs &
// can be looked up from this class
// Depends on: Scene_Game, HUD

namespace TragicMagic
{
	class HUDHandlerClass : Entity
	{
		// Defines
		public const short HUDs = 2;

		// Hold a reference to the current scene in order to add new entities to it
		public Scene_GameClass CurrentScene;

		// Store the two wizard HUDs
		public HUDClass[] HUD;

		// Store the Leap Motion Controller warning HUD entities
		public HUDElementClass[] HUDElement_Leap;

		// Store the Team Member credit HUD entities
		public HUDElementClass[] HUDElement_Team;

		// Store the combo bar HUD entities
		public HUDElementClass[] HUDElement_Combo;

		// Store the game round timer HUD entities
		public HUDElementClass[] HUDElement_Timer;

		// Store the game round score HUD entities
		public HUDElementClass[] HUDElement_Score;

		// Store the game round end outcome HUD entities
		public HUDElementClass[] HUDElement_Outcome;

		// Store the virtual keyboard HUD entities
		public HUDElementClass[] HUDElement_Keyboard;

		// Setup the HUD objects, and their element containers
		// IN: (scene_current) The current scene
		// OUT: N/A
		public HUDHandlerClass( Scene_GameClass scene_current )
			: base()
		{
			// Reference to the current scene
			CurrentScene = scene_current;

			// Parent HUD objects
			HUD = new HUDClass[HUDs];
			{
				HUD[0] = new HUDClass( CurrentScene, -90 );
				HUD[1] = new HUDClass( CurrentScene, 90 );
			}

			// Leap Motion Controller warning objects
			HUDElement_Leap = new HUDElementClass[HUDs];

			// Team display objects
			HUDElement_Team = new HUDElementClass[HUDs];

			// Combo objects
			HUDElement_Combo = new HUDElementClass[HUDs];

			// Timer objects
			HUDElement_Timer = new HUDElementClass[HUDs];

			// Score objects
			HUDElement_Score = new HUDElementClass[HUDs];

			// Outcome objects
			HUDElement_Outcome = new HUDElementClass[HUDs];

			// Keyboard objects
			HUDElement_Keyboard = new HUDElementClass[HUDs];
		}

		public override void Added()
		{
			base.Added();
		}

		public override void Update()
		{
			base.Update();
		}

		// Add the Leap Motion Controller missing warning to the HUDs
		// IN: N/A
		// OUT: N/A
		public void AddLeapWarning()
		{
			if ( HUDElement_Leap[0] != null ) { return; }; // Already added

			// Display the HUD elements
			for ( short hud = 0; hud < HUDs; hud++ )
			{
				HUDElement_Leap[hud] = new HUDElement_LeapClass(
					CurrentScene, // Reference to the current scene
					Game.Instance.HalfHeight / 2, // Position X
					25 // Position Y
				);
               
			}

			Add( HUDElement_Leap[0], HUDElement_Leap[1] );
		}

		// Remove the Leap Motion Controller missing warning from the HUDs
		// IN: N/A
		// OUT: N/A
		public void RemoveLeapWarning()
		{
			if ( HUDElement_Leap[0] == null ) { return; }; // Already removed

			Remove( HUDElement_Leap[0], HUDElement_Leap[1] );

			// Flag both HUD elements for garbage collection
			for ( short hud = 0; hud < HUDs; hud++ )
			{
				HUDElement_Leap[hud] = null;
			}
		}

		// Add the team credits to the HUDs
		// IN: N/A
		// OUT: N/A
		public void AddTeam()
		{
			if ( HUDElement_Team[0] != null ) { return; }; // Already added

			// Display the HUD elements
			for ( short hud = 0; hud < HUDs; hud++ )
			{
				HUDElement_Team[hud] = new HUDElement_TeamClass(
					CurrentScene, // Reference to the current scene
					0, // Position X
					150 // Position Y
				);
			}

			Add( HUDElement_Team[0], HUDElement_Team[1] );
		}

		// Remove the team credits from the HUDs
		// IN: N/A
		// OUT: N/A
		public void RemoveTeam()
		{
			if ( HUDElement_Team[0] == null ) { return; }; // Already removed

			Remove( HUDElement_Team[0], HUDElement_Team[1] );

			// Flag both HUD elements for garbage collection
			for ( short hud = 0; hud < HUDs; hud++ )
			{
				HUDElement_Team[hud] = null;
			}
		}

		// Add the combo bar to the HUDs
		// IN: N/A
		// OUT: N/A
		public void AddCombo()
		{
			if ( HUDElement_Combo[0] != null ) { return; }; // Already added

			// Display the HUD elements
			for ( short hud = 0; hud < HUDs; hud++ )
			{
				HUDElement_Combo[hud] = new HUDElement_ComboBarClass(
					CurrentScene, // Reference to the current scene
					Game.Instance.HalfHeight / 2, // Position X
					50 // Position Y
				);
			}

			Add( HUDElement_Combo[0], HUDElement_Combo[1] );
		}

		// Remove the combo bar from the HUDs
		// IN: N/A
		// OUT: N/A
		public void RemoveCombo()
		{
			if ( HUDElement_Combo[0] == null ) { return; }; // Already removed

			Remove( HUDElement_Combo[0], HUDElement_Combo[1] );

			// Flag both HUD elements for garbage collection
			for ( short hud = 0; hud < HUDs; hud++ )
			{
				HUDElement_Combo[hud] = null;
			}
		}

		// Add the game round timer to the HUDs
		// IN: N/A
		// OUT: N/A
		public void AddTimer()
		{
			if ( HUDElement_Timer[0] != null ) { return; }; // Already added

			// Display the HUD elements
			for ( short hud = 0; hud < HUDs; hud++ )
			{
				HUDElement_Timer[hud] = new HUDElement_TimerClass(
					CurrentScene, // Reference to the current scene
					Game.Instance.HalfHeight / 2, // Position X
					20 // Position Y
				);
			}

			Add( HUDElement_Timer[0], HUDElement_Timer[1] );
		}

		// Remove the game round timer from the HUDs
		// IN: N/A
		// OUT: N/A
		public void RemoveTimer()
		{
			if ( HUDElement_Timer[0] == null ) { return; }; // Already removed

			Remove( HUDElement_Timer[0], HUDElement_Timer[1] );

			// Flag both HUD elements for garbage collection
			for ( short hud = 0; hud < HUDs; hud++ )
			{
				HUDElement_Timer[hud] = null;
			}
		}

		// Add the game round score to the HUDs
		// IN: N/A
		// OUT: N/A
		public void AddScore()
		{
			if ( HUDElement_Score[0] != null ) { return; }; // Already added

			// Display the HUD elements
			for ( short hud = 0; hud < HUDs; hud++ )
			{
				HUDElement_Score[hud] = new HUDElement_ScoreClass(
					CurrentScene, // Reference to the current scene
					Game.Instance.HalfHeight / 16, // Position X
					80 // Position Y
				);
			}

			Add( HUDElement_Score[0], HUDElement_Score[1] );
		}

		// Remove the game round score from the HUDs
		// IN: N/A
		// OUT: N/A
		public void RemoveScore()
		{
			if ( HUDElement_Score[0] == null ) { return; }; // Already removed

			Remove( HUDElement_Score[0], HUDElement_Score[1] );

			// Flag both HUD elements for garbage collection
			for ( short hud = 0; hud < HUDs; hud++ )
			{
				HUDElement_Score[hud] = null;
			}
		}

		// Add the game round end outcome to the HUDs
		// IN: N/A
		// OUT: N/A
		public void AddOutcome()
		{
			if ( HUDElement_Outcome[0] != null ) { return; }; // Already added

			// Find the winner of the round
			short winner = -1;
			float maxscore = 0;
			for ( short hud = 0; hud < HUDs; hud++ )
			{
				if ( CurrentScene.Wizards[hud].Score > maxscore ) // Player with the highest score wins
				{
					winner = hud;
				}
			}

			// Display the HUD elements
			for ( short hud = 0; hud < HUDs; hud++ )
			{
				HUDElement_Outcome[hud] = new HUDElement_OutcomeClass(
					CurrentScene, // Reference to the current scene
					Game.Instance.HalfHeight / 2, // Position X
					Game.Instance.HalfWidth / 3, // Position Y
					CurrentScene.Wizards[hud].Score, // The final score of the player
					( hud == winner ) // Whether or not this player was the winner
				);
			}

			Add( HUDElement_Outcome[0], HUDElement_Outcome[1] );
		}

		// Remove the game round end outcome from the HUDs
		// IN: N/A
		// OUT: N/A
		public void RemoveOutcome()
		{
			if ( HUDElement_Outcome[0] == null ) { return; }; // Already removed

			Remove( HUDElement_Outcome[0], HUDElement_Outcome[1] );

			// Flag both HUD elements for garbage collection
			for ( short hud = 0; hud < HUDs; hud++ )
			{
				HUDElement_Outcome[hud] = null;
			}
		}

		// Add the virtual keyboard to the HUDs
		// IN: N/A
		// OUT: N/A
		public void AddKeyboard()
		{
			if ( HUDElement_Keyboard[0] != null ) { return; }; // Already added

			// Display the HUD elements
			for ( short hud = 0; hud < HUDs; hud++ )
			{
				HUDElement_Keyboard[hud] = new HUDElement_KeyboardClass(
					CurrentScene, // Reference to the current scene
					Game.Instance.HalfHeight / 2, // Position X
					Game.Instance.HalfWidth / 3, // Position Y
					hud // The HUD index, for the player the keyboard belongs to
				);
			}

			Add( HUDElement_Keyboard[0], HUDElement_Keyboard[1] );
		}

		// Remove the virtual keyboard from the HUDs
		// IN: N/A
		// OUT: N/A
		public void RemoveKeyboard()
		{
			if ( HUDElement_Keyboard[0] == null ) { return; }; // Already removed

			Remove( HUDElement_Keyboard[0], HUDElement_Keyboard[1] );

			// Flag both HUD elements for garbage collection
			for ( short hud = 0; hud < HUDs; hud++ )
			{
				HUDElement_Keyboard[hud] = null;
			}
		}

		// Add an entity to both HUDs
		// IN: (entity1) The entity representing the first HUD element, (entity2) The entity representing the second HUD element
		// OUT: N/A
		private void Add( HUDElementClass entity1, HUDElementClass entity2 )
		{
			HUD[0].Add( entity1 );
			HUD[1].Add( entity2 );
		}

		// Remove an entity from both HUDs
		// IN: (entity1) The entity representing the first HUD element, (entity2) The entity representing the second HUD element
		// OUT: N/A
		private void Remove( HUDElementClass entity1, HUDElementClass entity2 )
		{
			HUD[0].Remove( entity1 );
			HUD[1].Remove( entity2 );
		}
	}
}