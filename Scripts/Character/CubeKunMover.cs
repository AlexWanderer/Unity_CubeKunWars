using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Chronos;

namespace MackySoft.CubeKunWars {
	public class CubeKunMover : AIPath {
		
		public float sleepVelocity = 0.4f;
		public CubeKun parent;

		public void Move () {
			if (!canMove || !target) return;
			
			var dir = CalculateVelocity(GetFeetPosition());
			RotateTowards(targetDirection);
			
			dir.y = 0;
			if (dir.sqrMagnitude > sleepVelocity * sleepVelocity) {

			} else {
				dir = Vector3.zero;
			}
			tr.Translate(dir * parent.Timeline.deltaTime,Space.World);
		}
		
		public override Vector3 GetFeetPosition () {
			return tr.position;
		}

	}
}