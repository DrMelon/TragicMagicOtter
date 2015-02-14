using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Matthew Cormack @johnjoemcbob
// 13/02/15
// Represents a clampable scalar value between a minimum and a maximum, with
// extra functionality provided by OnMinimum and OnMaximum callbacks
// Depends on: ClampedValue

namespace TragicMagic
{
	class ClampedSpeedValueClass : ClampedValueClass
	{
		// The speed to change the value
		public float Speed = 1;

		// The current direction of travel between the minimum and maximum values (-1 or 1)
		public short Direction = 1;

		public override void Update()
		{
			// Invert direction when out of range
			if ( ( Value > Maximum ) || ( Value < Minimum ) )
			{
				Direction *= -1;
			}

			// Base clamping update
			base.Update();

			// Update the value with the speed after clamping to ensure the custom callbacks run
			Value += Direction * Game.Instance.DeltaTime * Speed;
		}
	}
}