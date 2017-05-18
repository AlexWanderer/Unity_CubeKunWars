using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MackySoft.CubeKunWars {
	public class AmmunitionStore : BasePoint {

		protected override void OnCubeKunEnter (CubeKun cubekun) {
			if (GetTeamRelative(cubekun) != Team.Ally) return;
			for (int i = 0;cubekun.Weapons.Length > i;i++)
				cubekun.Weapons[i].Replenishment();
		}

	}
}