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

namespace TragicMagic
{
	// Structure to hold the information describing individual wands in game space
	struct WandInformation
	{
		public Vector2 Position;
		public Vector2 Direction;
		public float Height;
		public float Time_Recorded; // The time this data was recorded at
		public float Time_Cast; // The time the last spell was cast at
		public int ID; // The ID of the tool that this wand represents
	}

	class GameWandsClass : Entity
	{
		// Defines
		private const short WANDS = 2;
		private const float CAST_SPEED = 20; // NOTE: Above 30/40 seems to be good
		public const float CAST_BETWEEN = 10; // Minimum time between casts

		// Store reference to the LeapController to query for tool tracking positions
		public Leap.Controller LeapController;

		// Store the data describing each player's wand
		public WandInformation[] Wand;
		public WandInformation[] LastWand;

		// Whether or not the Leap Motion Controller device is inverted, leading to player's controlling the wrong wand
		public bool Inverted = false;

		// Variable function callback
		public Func<short, float, WandInformation, int> OnCast = DefaultCast;

		public GameWandsClass() : base()
		{
			// Initialize wand information to 0
			Wand = new WandInformation[WANDS];
			LastWand = new WandInformation[WANDS];
			{
				for ( short wand = 0; wand < WANDS; wand++ )
				{
					Wand[wand].Position = new Vector2();
					Wand[wand].Direction = new Vector2();
					Wand[wand].Time_Recorded = 0;
					Wand[wand].Time_Cast = 0;
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
						Console.WriteLine( "Tools: " + frame.Tools.Count );
						if ( frame.Tools.Count > 1 ) // Two tools, find closest to each side
						{
							// Check whether the second tool is closest to the player with the Leap cable extending to their right (default light wizard)
							short closest = 0; // Store the id of the closest tool
							{
								Tool tool1 = frame.Tools[0];
								Tool tool2 = frame.Tools[1];
								if ( tool2.StabilizedTipPosition.z < tool1.StabilizedTipPosition.z ) // Second tool actually belongs on the other side
								{
									closest = 1;
								}
							}

							// Initialize the position and direction wand variables
							Vector position;
							Vector direction;

							// Store the position of this wand as the appropriate player (default light wizard)
							short wand = 0;
							short tool = closest; // The tool to look up
							{
								if ( Inverted ) // Invert the wand id if the Leap device is inverted, to correct it
								{
									wand = Convert.ToSByte( !Convert.ToBoolean( wand ) ); // Convert to bool, invert, convert back to short
								}

								position = frame.Tools[tool].StabilizedTipPosition;
								direction = frame.Tools[tool].Direction;
							}
							Wand[wand].Position = new Vector2( position.x, position.z );
							Wand[wand].Height = position.y;
							Wand[wand].Direction = new Vector2( direction.x, direction.z );
							Wand[wand].Time_Recorded = Game.Instance.Timer;
							Wand[wand].ID = tool;

							// Store the position of this wand as the appropriate player (default dark wizard)
							wand = 1;
							tool = Convert.ToSByte( !Convert.ToBoolean( closest ) ); // The tool to look up, inverted to get the opposite of the closest
							{
								if ( Inverted ) // Invert the wand id if the Leap device is inverted, to correct it
								{
									wand = Convert.ToSByte( !Convert.ToBoolean( wand ) ); // Convert to bool, invert, convert back to short
								}

								position = frame.Tools[tool].StabilizedTipPosition;
								direction = frame.Tools[tool].Direction;
							}
							Wand[wand].Position = new Vector2( position.x, position.z );
							Wand[wand].Height = position.y;
							Wand[wand].Direction = new Vector2( direction.x, direction.z );
							Wand[wand].Time_Recorded = Game.Instance.Timer;
							Wand[wand].ID = tool;
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
							Wand[wand].Height = position.y;
							Wand[wand].Direction = new Vector2( direction.x, direction.z );
							Wand[wand].Time_Recorded = Game.Instance.Timer;
							Wand[wand].ID = 0;
						}

						// Ensure the wands point in the right direction
						{
							// NOTE: This does not get inverted, as it is actually based on which side of the Leap the player is on,
							//       so it should be the same whether the light wizard is id 0 or the dark wizard is
							short wand = 0;

							// Player with the Leap cable extending to their right (default light wizard)
							wand = 0;
							if ( Wand[wand].Direction.Y < 0 ) // Direction y (z in 3d space) should always be negative for this player's aiming wand
							{
								Wand[wand].Direction *= -1; // If not, correct it
							}

							// Player with the Leap cable extending to their left (default dark wizard)
							wand = 1;
							if ( Wand[wand].Direction.Y > 0 ) // Direction y (z in 3d space) should always be positive for this player's aiming wand
							{
								Wand[wand].Direction *= -1; // If not, correct it
							}
						}

						// Check for wand casting
						{
							for ( short wand = 0; wand < WANDS; wand++ )
							{
								if ( LastWand[wand].Position != null ) // May not have data for the last wand position yet
								{
									// Keep the value of the last spell cast time for this wand
									Wand[wand].Time_Cast = LastWand[wand].Time_Cast;

									float framedifference = Wand[wand].Time_Recorded - LastWand[wand].Time_Recorded;
									if ( framedifference < 5 ) // Last frame information captured was recently
									{
										// Calculate speed of movement using the last position & this one
										float speed = ( LastWand[wand].Height - Wand[wand].Height ) / framedifference; // Only apply downwards, divide by time since last frame
										if ( speed > CAST_SPEED ) // Minimum speed required to cast
										{
											float lastcast = Game.Instance.Timer - Wand[wand].Time_Cast;
											if ( lastcast > CAST_BETWEEN ) // Minimum time between casts
											{
												OnCast( wand, speed, Wand[wand] );

												// Update last cast time for delay between casts
												Wand[wand].Time_Cast = Game.Instance.Timer;
											}
										}
									}
								}

								// Update LastWand for the next update
								LastWand[wand] = Wand[wand];
							}
						}
					}
				}
				frame.Dispose();
			}
		}

		// Default callback for OnCast
		// IN: (wizard) The wizard casting the spell, (speed) The speed of the casting wand action
		//     (wand) The state of the wand at the time of casting
		// OUT: (int) Meaningless
		private static int DefaultCast( short wizard, float speed, WandInformation wand )
		{
			return 0;
		}
	}
}