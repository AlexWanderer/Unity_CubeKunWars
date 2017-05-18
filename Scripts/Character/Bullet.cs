using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Chronos;

namespace MackySoft.CubeKunWars {

	[RequireComponent(typeof(Timeline))]
	[RequireComponent(typeof(BoxCollider))]
	public class Bullet : MonoBehaviour {

		#region Variables

		public int power = 20;
		public float speed = 10;

		private Transform _Tr;
		private Coroutine disableCoroutine = null;

		#endregion

		#region Properties

		public CubeKun Parent { get; private set; }
		public Timeline Timeline { get; private set; }
		public TrailRenderer Trail { get; private set; }

		public Transform Tr {
			get { return _Tr ? _Tr : (_Tr = transform); }
		}

		#endregion

		public void Shot (Transform point,CubeKun parent,int power,float speed,float time) {
			var ins = PoolManager.GetPoolSafe(gameObject).Get<Bullet>(point.position,point.rotation);
			Parent = parent;
			ins.gameObject.SetLayer("Bullet_" + parent.Team.ToString());
			ins.power = power;
			ins.speed = speed;
			ins.Trail.Clear();
			ins.DisableStart(time);
		}

		private void Awake () {
			Timeline = GetComponent<Timeline>();
			Trail = GetComponent<TrailRenderer>();
		}

		private void Update () {
			if (GameManager.TimeState == TimeState.Paused) return;
			Tr.Translate(Tr.forward * speed * Timeline.deltaTime,Space.World);
		}

		public void Disable () {
			DisableStop();
			gameObject.SetActive(false);
		}

		public void DisableStart (float time) {
			DisableStop();
			disableCoroutine = StartCoroutine(DisableCoroutine(time));
		}

		public void DisableStop () {
			if (disableCoroutine != null)
				StopCoroutine(disableCoroutine);
		}

		private IEnumerator DisableCoroutine (float time) {
			yield return Timeline.WaitForSeconds(time);
			Disable();
		}
		
	}
}