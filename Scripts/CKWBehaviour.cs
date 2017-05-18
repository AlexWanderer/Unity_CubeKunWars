using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Chronos;

namespace MackySoft.CubeKunWars {

	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(Health))]
	[RequireComponent(typeof(Timeline))]
	public abstract class CKWBehaviour : MonoBehaviour {

		public static List<CKWBehaviour> list = new List<CKWBehaviour>();
		protected static bool isQuitting = false;
		
		[Range(0,100)]
		public int priority = 50;
		public float distanceStrength = 0.5f;

		protected StatusGUI status = null;

		public virtual Team Team { get; set; }

		public Transform Tr { get; private set; }
		public Rigidbody Rigid { get; private set; }
		public Health Health { get; private set; }
		public Timeline Timeline { get; private set; }
		
		protected virtual void Awake () {
			Tr = transform;

			Rigid = GetComponent<Rigidbody>();

			Health = GetComponent<Health>();

			Timeline = GetComponent<Timeline>();
			Timeline.mode = TimelineMode.Global;
			Timeline.globalClockKey = "Root";
			Timeline.rewindable = false;
		}

		protected virtual void OnEnable () {
			list.Add(this);
		}

		protected virtual void OnDisable () {
			list.Remove(this);
			if (!isQuitting)
				status.Disable();
		}

		protected virtual void Start () {
			status = PoolManager.GetPoolSafe(StatusGUI.Prefab.gameObject).Get<StatusGUI>(StatusGUI.Canvas.transform);
			status.Initialize(this);
		}

		protected virtual void OnDestroy () {
			
		}
		
		private void OnApplicationQuit () {
			isQuitting = true;
		}
		
		public float CalculatePriority (CKWBehaviour behaviour) {
			return priority + Vector3.Distance(Tr.position,behaviour.Tr.position) * distanceStrength;
		}
		
		public Team GetTeamRelative (CKWBehaviour behaviour) {
			if (!behaviour)
				throw new ArgumentNullException("behaviour");
			else if (
				Team != Team.Independent && behaviour.Team == Team.Independent ||
				Team == Team.Independent && behaviour.Team != Team.Independent
			)
				return Team.Independent;
			else if (Team == behaviour.Team)
				return Team.Ally;
			else
				return Team.Enemy;
		}

	}
	
	public enum Team {
		Independent,
		Ally,
		Enemy
	}
}