using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MackySoft.CubeKunWars {

	[ExecuteInEditMode]
	public class CameraController : MonoBehaviour {

		public Transform target;
		public Vector3 offset;

		private Transform tr;

		void Start () {
			tr = transform;
		}
		
		void Update () {
			if (!target) return;
			//tr.position = target.position + Vector3.up * height;
			tr.position = target.position + offset;
		}
	}
}