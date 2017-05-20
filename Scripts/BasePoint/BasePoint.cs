using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MackySoft.CubeKunWars {
	
	[RequireComponent(typeof(BoxCollider))]
	public abstract class BasePoint : CKWBehaviour {

		public static new List<BasePoint> list = new List<BasePoint>();
		
		public override Team Team {
			get { return _Team; }
			set {
				_Team = value;
				gameObject.SetLayer(value.ToString());
				switch (value) {
					case Team.Independent: SetColor(Color.white); break;
					case Team.Ally: SetColor(allyColor); break;
					case Team.Enemy: SetColor(enemyColor); break;
				}
				OnTeamChanged();
			}
		}
		[SerializeField]
		private Team _Team = Team.Ally;

		public Color allyColor = Color.blue;
		public Color enemyColor = Color.red;

		public BoxCollider Box { get; private set; }
		public Renderer[] Renderers {
			get { return _Renderers != null ? _Renderers : (_Renderers = GetComponentsInChildren<Renderer>()); }
		}
		private Renderer[] _Renderers;

		protected override void Awake () {
			base.Awake();
			Box = GetComponent<BoxCollider>();
			Box.isTrigger = true;
			
			Rigid.isKinematic = true;
		}
		
		protected override void OnEnable () {
			base.OnEnable();
			list.Add(this);
		}

		protected override void OnDisable () {
			base.OnDisable();
			list.Remove(this);
		}

		protected override void Start () {
			base.Start();
			Initialize();
		}

		protected virtual void OnTriggerEnter (Collider other) {
			var bullet = other.GetComponent<Bullet>();
			if (bullet) {
				if (Team != Team.Independent)
					Health.Value -= bullet.power;
				return;
			}
			var cubekun = other.GetComponentInParent<CubeKun>();
			if (cubekun) {
				if (Team == Team.Independent || Team != Team.Independent && Health.IsDead)
					Team = cubekun.Team;
				OnCubeKunEnter(cubekun);
			}
		}

		protected virtual void OnValidate () {
			Team = _Team;
		}
		
		public void SetColor (Color color) {
			for (int i = 0;Renderers.Length > i;i++) {
#if UNITY_EDITOR
				if (!Application.isPlaying)
					Renderers[i].sharedMaterial.color = color;
				else
#endif
					Renderers[i].material.color = color;
			}
		}

		protected virtual void OnCubeKunEnter (CubeKun cubekun) { }
		protected virtual void OnTeamChanged () {
			if (status) status.UpdateArrowColor();
		}
	}
}