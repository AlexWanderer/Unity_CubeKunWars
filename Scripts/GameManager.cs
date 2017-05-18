using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Chronos;

namespace MackySoft.CubeKunWars {
	public static class GameManager {
		
		public static GlobalClock Clock {
			get { return _Clock ? _Clock : (_Clock = Timekeeper.instance.Clock("Root")); }
		}
		private static GlobalClock _Clock = null;

		public static TimeState TimeState {
			get { return Timekeeper.GetTimeState(Clock.localTimeScale); }
		}

	}
}