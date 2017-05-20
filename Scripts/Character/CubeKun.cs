using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Chronos;
using MackySoft.DynamicCulling;

namespace MackySoft.CubeKunWars {

	[RequireComponent(typeof(CubeKunMover))]
	[RequireComponent(typeof(DynamicCullingTarget))]
	public abstract class CubeKun : CKWBehaviour {
		
		#region Variables

		public static new List<CubeKun> list = new List<CubeKun>();

		public float retargetingRate = 0.1f;

		public float armSpeed = 10;

		private Coroutine targetingCoroutine = null;
		
		#endregion

		#region Properties

		public virtual CKWBehaviour Target { get; set; }

		public CubeKunMover Mover { get; private set; }
		public Transform[] Arms { get; private set; }
		public Weapon[] Weapons { get; private set; }
		
		#endregion

		#region Component Segments

		protected override void Awake () {
			base.Awake();
			Arms = new Transform[] {
				Tr.Find("Arm_L"),
				Tr.Find("Arm_R")
			};

			Weapons = GetComponentsInChildren<Weapon>();
			for (int i = 0;Weapons.Length > i;i++)
				Weapons[i].parent = this;
			
			Rigid.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
			
			Health.onDie.AddListener(h => Disable());
			
			Mover = GetComponent<CubeKunMover>();
			Mover.parent = this;
		}

		protected override void OnEnable () {
			base.OnEnable();
			
			
		}

		protected override void OnDisable () {
			if (isQuitting) return;
			base.OnDisable();
			
			
		}

		protected override void Start () {
			base.Start();
			TargetingStart();
		}
		
		protected virtual void Update () {
			if (GameManager.TimeState == TimeState.Paused) return;
			RotateArm();
		}
		
		protected virtual void OnCollisionEnter (Collision collision) {
			var bullet = collision.gameObject.GetComponent<Bullet>();
			if (bullet) {
				Health.Value -= bullet.power;
				bullet.Disable();
			}
		}

		#endregion

		public static CubeKun GetInstance (CubeKun prefab,Vector3 position,Quaternion rotation,Team team) {
			var cubekun = PoolManager.GetPoolSafe(prefab.gameObject).Get<CubeKun>(position,rotation);
			cubekun.Team = team;
			cubekun.Initialize();
			return cubekun;
		}

		protected override void Initialize () {
			base.Initialize();
			list.Add(this);
			ForceBar.Instance.BarUpdate();
		}

		public void Disable () {
			if (isQuitting) return;
			list.Remove(this);
			gameObject.SetActive(false);
			Health.Value = Health.Max;
			Target = null;
			for (int i = 0;Weapons.Length > i;i++) {
				Weapons[i].ShootStop();
				Weapons[i].Replenishment();
			}
			ForceBar.Instance.BarUpdate();
		}

		public void RotateArm () {
			if (Target) {
				for (int i = 0;Arms.Length > i;i++) {
					var dir = Target.Tr.position - Arms[i].position;
					dir.y = 0;
					var newDir = Vector3.RotateTowards(
						Arms[i].forward,
						dir,
						armSpeed * Timeline.deltaTime,
						0
					);
					Arms[i].rotation = Quaternion.LookRotation(newDir);
				}
			} else {
				for (int i = 0;Arms.Length > i;i++) {
					Arms[i].localRotation = Quaternion.Euler(Vector3.zero);
				}
			}
		}
		
		public CKWBehaviour GetAttackTarget () {
			return
				CKWBehaviour.list.Where(b => b && b != this && GetTeamRelative(b) == Team.Enemy).
				OrderBy(b => CalculatePriority(b)).
				FirstOrDefault();
		}

		public BasePoint GetSuppressionTarget () {
			return
				BasePoint.list.Where(b => b && GetTeamRelative(b) != Team.Ally).
				OrderBy(b => CalculatePriority(b)).
				FirstOrDefault();
		}
		
		public CKWBehaviour GetMoveTarget () {
			if (EmptyCount() == Weapons.Length) {
				AmmunitionStore store = 
					CKWBehaviour.list.Cast<AmmunitionStore>().
					OrderBy(b => Vector3.Distance(Tr.position,b.Tr.position)).
					FirstOrDefault(b => Team == b.Team);
				if (store) return store;
			}
			return null;
		}

		public void TargetingStart () {
			if (targetingCoroutine == null)
				targetingCoroutine = StartCoroutine(TargetingCoroutine());
		}

		public void TargetingStop () {
			if (targetingCoroutine != null)
				StopCoroutine(targetingCoroutine);
		}

		public virtual void Targeting () {
			Target = GetAttackTarget();
			if (!Target)
				Target = GetSuppressionTarget();
		}

		private IEnumerator TargetingCoroutine () {
			while (true) {
				Targeting();
				yield return Timeline.WaitForSeconds(retargetingRate);
			}
		}

		#region Weapons

		public void ShootControl (CKWBehaviour target) {
			if (target) {
				switch (GetTeamRelative(target)) {
					case Team.Independent:
						ShootStopAll();
						break;
					case Team.Ally:
						ShootStopAll();
						break;
					case Team.Enemy:
						ShootStartAll();
						break;
				}
			} else {
				ShootStopAll();
			}
		}

		public int EmptyCount () {
			int count = 0;
			for (int i = 0;Weapons.Length > i;i++)
				if (Weapons[i].IsEmpty) count++;
			return count;
		}

		public void ShootStartAll () {
			for (int i = 0;Weapons.Length > i;i++)
				Weapons[i].ShootStart();
		}

		public void ShootStopAll () {
			for (int i = 0;Weapons.Length > i;i++)
				Weapons[i].ShootStop();
		}

		#endregion

	}
}