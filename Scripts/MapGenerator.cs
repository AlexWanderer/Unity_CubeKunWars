using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

namespace MackySoft.CubeKunWars {

	[RequireComponent(typeof(BoxCollider))]
	public class MapGenerator : MonoBehaviour {
		
		public GenerateInfo[] infos = new GenerateInfo[0];

		private void Start () {
			var box = GetComponent<BoxCollider>();
			for (int i = 0;infos.Length > i;i++) {
				for (int j = 0;infos[i].count > j;j++) {
					var ins = Instantiate(infos[i].prefab);
					ins.Tr.position = new Vector3(
						Random.Range(box.bounds.min.x,box.bounds.max.x),
						0,
						Random.Range(box.bounds.min.z,box.bounds.max.z)
					);
					ins.Team = infos[i].team;
				}
			}
			box.enabled = false;
		}
	}

	[Serializable]
	public struct GenerateInfo {
		public Team team;
		public int count;
		public CKWBehaviour prefab;
	}
}