using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Leap;

// Matthew Cormack @johnjoemcbob
// 14/02/15
// Handles the Leap.Controller's view, queries for tool tracking positions & converts them
// into a wand position for each player
// Depends on: Leap.Controller

// TODO:
// More advanced identification if 2 tools are present to find the one closest to the side of the player
// Ensure wands on either side always point at the other wizard
// Better spell casting identification

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
		private const short WANDS = 2;

		// Store reference to the LeapController to query for tool tracking positions
		public Leap.Controller LeapController;

		// Store the data describing each player's wand
		public WandInformation[] Wand;

		// Whether or not the Leap Motion Controller device is inverted, leading to player's controlling the wrong wand
		public bool Inverted = false;

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
							// TODO: More advanced identification if 2 tools are present to find the one closest to the side of the player
						}
						else // One tool, split the Leap in half to find the side it's more likely to belong to
						{
							Leap.Tool tool = frame.Tools[0];
							Vector position = tool.StabilizedTipPosition;
							Vector direction = tool.Direction;

							// Store the position of this wand as the appropriate player
							short wand = 0;
							{
								if ( position.z > 0 )
								{
									wand = 1;
								}
								if ( Inverted ) // Invert the wand id if the Leap device is inverted, to correct it
								{
									wand = Convert.ToSByte( !Convert.ToBoolean( wand ) ); // Convert to bool, invert, convert back to short
								}
							}
							Wand[wand].Position = new Vector2( position.x, position.z );
							Wand[wand].Direction = new Vector2( direction.x, direction.z );
						}

						// Has updated wands this frame, now ensure they point in the right direction
						{
							// TOOD: Ensure wands on either side always point at the other wizard
							// Based on wand id

						}
					}
				}
				frame.Dispose();
			}
		}
	}
}