using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Chronos;

namespace MackySoft.CubeKunWars {

	[RequireComponent(typeof(Timeline))]
	public class Barrack : BasePoint {

		public CubeKun prefab;
		public float time = 5;

		private Coroutine spawnCoroutine = null;

		protected override void Start () {
			base.Start();
			//Health.onDie.AddListener(h => SpawnStop());
		}

		public void SpawnStart () {
			if (spawnCoroutine == null && isActiveAndEnabled)
				spawnCoroutine = StartCoroutine(SpawnCoroutine());
		}

		public void SpawnStop () {
			if (spawnCoroutine != null)
				StopCoroutine(spawnCoroutine);
		}

		private IEnumerator SpawnCoroutine () {
			while (true) {
				Spawn(prefab);
				yield return Timeline.WaitForSeconds(time);
			}
		}

		public CubeKun Spawn (CubeKun prefab) {
			if (!prefab)
				throw new ArgumentNullException("prefab");
			var ins = CubeKun.GetInstance(prefab,Tr.position,Tr.rotation,Team);
			return ins;
		}

		protected override void OnTeamChanged () {
			if (!Application.isPlaying) return;
			base.OnTeamChanged();
			switch (Team) {
				case Team.Independent:
					SpawnStop();
					break;
				case Team.Ally:
				case Team.Enemy:
					SpawnStart();
					break;
			}
		}
	}
}