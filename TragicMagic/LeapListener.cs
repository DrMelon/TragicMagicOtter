﻿using System;
using System.Threading.Tasks;
using Leap;

namespace TragicMagic
{
	public class LeapListenerClass : Leap.Listener
	{
		public override void OnInit( Controller controller )
		{
			
		}

		public override void OnConnect( Controller controller )
		{
			
		}

		public override void OnDisconnect( Controller controller )
		{
			
		}

		public override void OnExit( Controller controller )
		{
			
		}

		public override void OnFrame( Controller controller )
		{
			var frame = controller.Frame();
			
		}
	}
}