using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Chronos;
using DG.Tweening;

namespace MackySoft.CubeKunWars {

	public class Player : CubeKun {
		
		public float whistleRange = 2;

		public Transform Pointer { get; private set; }
		public bool IsWhistling { get; private set; }
		public List<NPC> selectingList = new List<NPC>();

		public override Team Team {
			get { return Team.Ally; }
			set { throw new NotImplementedException(); }
		}
		
		private RaycastHit hit;
		private Transform whistle;
		
		protected override void Awake () {
			base.Awake();
			gameObject.tag = "Player";
			gameObject.SetLayer("Player");
			
			Pointer = new GameObject("pointer").transform;
			Mover.target = Pointer;

			whistle = Tr.Find("Whistle");
			whistle.localScale = Vector3.zero;
		}

		protected override void Start () {
			Initialize();
			base.Start();
		}

		protected override void OnDestroy () {
			if (isQuitting) return;
			base.OnDestroy();
			Destroy(Pointer.gameObject);
		}

		protected override void Update () {
			if (Input.GetMouseButtonDown(0)) {
				if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out hit)) {
					IsWhistling = hit.collider.GetComponentInParent<Player>() == this;
				}
			}

			Mover.canMove = Input.GetMouseButton(0);
			Mover.canSearch = Mover.canMove;
			GameManager.Clock.paused = !Mover.canMove;
			IsWhistling = IsWhistling && Mover.canMove;

			if (IsWhistling) {
				whistle.localScale = Vector3.one * whistleRange;
			} else {
				whistle.localScale = Vector3.zero;
			}

			if (Mover.canMove && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out hit)) {
				Pointer.position = hit.point;
				Mover.Move();
				RotateArm();
			} else {

			}
		}

		protected virtual void OnTriggerEnter (Collider other) {
			if (IsWhistling) {
				var npc = other.GetComponentInParent<NPC>();
				if (npc && GetTeamRelative(npc) == Team.Ally && !selectingList.Contains(npc)) {
					npc.Select(this);
				}
			}
			
		}
	}
}