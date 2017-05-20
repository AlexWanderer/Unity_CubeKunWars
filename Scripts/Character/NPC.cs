using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Chronos;

namespace MackySoft.CubeKunWars {
	public class NPC : CubeKun {

		public override Team Team {
			get { return _Team; }
			set {
				gameObject.SetLayer(value.ToString());
				_Team = value;
			}
		}
		[SerializeField]
		private Team _Team = Team.Ally;

		public override CKWBehaviour Target {
			get { return _Target; }
			set {
				_Target = value;
				Mover.target = value ? value.Tr : null;
				ShootControl(Target);
			}
		}
		private CKWBehaviour _Target = null;
		
		private Player player;
		
		protected override void Awake () {
			base.Awake();
			Team = _Team;
		}

		protected override void OnDisable () {
			if (player) Deselect();
			base.OnDisable();
		}

		protected override void Update () {
			if (GameManager.TimeState == TimeState.Paused) return;
			base.Update();
			if (Mover.target) {
				Mover.Move();
			} else {
				
			}
		}
		
		private void OnValidate () {
			Team = _Team;
		}

		public override void Targeting () {
			if (player) {
				Target = player.Target;
			} else {
				base.Targeting();
			}
		}

		public void Select (Player player) {
			this.player = player;
			player.selectingList.Add(this);
			status.arrow.color = Color.green;
		}

		public void Deselect () {
			player.selectingList.Remove(this);
			status.UpdateArrowColor();
			player = null;
		}

	}
}