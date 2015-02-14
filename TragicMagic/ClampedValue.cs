using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Matthew Cormack @johnjoemcbob
// 13/02/15
// Represents a clampable scalar value between a minimum and a maximum, with
// extra functionality provided by OnMinimum and OnMaximum callbacks
// Depends on: N/A

namespace TragicMagic
{
	class ClampedValueClass
	{
		// Basic variables
		public float Minimum = 0;
		public float Maximum = 1;
		public float Value = 0;

		// Variable function callbacks
		public Func<int> OnMaximum = DefaultMaximum;
		public Func<int> OnMinimum = DefaultMinimum;

		public virtual void Update()
		{
			// If the value exceeds the range
			if ( Value > Maximum )
			{
				Value = Maximum;
				OnMaximum();
			}
			// If the value falls below the range
			if ( Value < Minimum )
			{
				Value = Minimum;
				OnMinimum();
			}
		}

		// Default callback for OnMinimum
		// IN: N/A
		// OUT: (int) Meaningless
		private static int DefaultMinimum()
		{
			return 0;
		}

		// Default callback for OnMaximum
		// IN: N/A
		// OUT: (int) Meaningless
		private static int DefaultMaximum()
		{
			return 0;
		}
	}
}