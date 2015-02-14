using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Leap;

// Matthew Cormack
// 14/02/15
// Handles the Leap.Controller's view, queries for tool tracking positions & converts them
// into a wand position for each player
// Depends on: Leap.Controller

// TODO:
// Basic identification based on the side of the Leap the wand is on
// More advanced identification if 2 tools are present to find the one closest to the side of the player
// Better spell casting identification
// Easy swapping of wand player side

namespace TragicMagic
{
	// Structure to hold the information describing individual wands in game space
	struct WandInformation
	{
		public Vector2 Position;
		public Vector2 Direction;
	}

	class GameWandsClass : Entity
	{
		// Defines
		private static short WANDS = 2;

		// Store reference to the LeapController to query for tool tracking positions
		public Leap.Controller LeapController;

		// Store the data describing each player's wand
		public WandInformation[] Wand;

		public GameWandsClass() : base()
		{
			// Initialize wand information to 0
			Wand = new WandInformation[WANDS];
			{
				for ( short wand = 0; wand < WANDS; wand++ )
				{
					Wand[wand].Position = new Vector2();
					Wand[wand].Direction = new Vector2();
				}
			}
		}

		public override void Added()
		{
			base.Added();

			
		}

		public override void Update()
		{
			base.Update();

			// If there is a Leap hand present, turn to a shade of red
			if ( LeapController != null )
			{
				Leap.Frame frame = LeapController.Frame();
				{
					if ( frame.Tools.Count > 0 ) // Detecting tools
					{
						if ( frame.Tools.Count > 1 ) // Two tools, find closest to each side
						{

						}
						else // One tool, split the Leap in half to find the side it's more likely to belong to
						{
							Leap.Tool tool = frame.Tools[0];
							Vector position = tool.StabilizedTipPosition;

							// Store the position of this wand as the appropriate player
							short wand = 0;
							{
								if ( position.z > 0 )
								{
									Console.Write( "Yes" + "\n" );
									wand = 1;
								}
							}
							Wand[wand].Position = new Vector2( position.x, position.z );
						}
					}
				}
				frame.Dispose();
			}
		}
	}
}