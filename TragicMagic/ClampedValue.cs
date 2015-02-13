using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TragicMagic
{
	class ClampedValue
	{
		public float Minimum = 0;
		public float Maximum = 1;
		public float Value = 0;

		public Func<int> OnMaximum = DefaultMaximum;
		public Func<int> OnMinimum = DefaultMinimum;

		public void Update()
		{
			if ( Value > Maximum )
			{
				Value = Maximum;
				OnMaximum();
			}
			if ( Value < Minimum )
			{
				Value = Minimum;
				OnMinimum();
			}
		}

		private static int DefaultMaximum()
		{
			return 0;
		}

		private static int DefaultMinimum()
		{
			return 0;
		}
	}
}