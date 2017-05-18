using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MackySoft.CubeKunWars {
	public class Weapon : MonoBehaviour {
		
		[Header("Shoot Settings")]
		public Transform[] firingPoints;
		public float interval = 0.5f;
		
		[Header("Bullet Settings")]
		public Bullet bullet;
		public int power = 20;
		public float speed = 5;
		public float time = 5;

		[Header("Ammunition")]
		[SerializeField]
		private int _Ammo = 10;

		[SerializeField]
		private int _Magazine = 3;
		
		public float reload = 2;

		public CubeKun parent;
		public bool IsReloading { get; private set; }
		public int I_Magazine { get; private set; }
		public int I_Ammo { get; private set; }

		public bool IsEmpty {
			get { return Magazine == 0 && Ammo == 0; }
		}

		public bool IsShooting {
			get { return shootCoroutine != null; }
		}

		public int Ammo {
			get { return _Ammo; }
			set {
				_Ammo = Mathf.Clamp(value,0,value);
				if (Ammo == 0 && Magazine > 0 && Application.isPlaying)
					StartCoroutine(ReloadCoroutine());
			}
		}

		public int Magazine {
			get { return _Magazine; }
			set { _Magazine = Mathf.Clamp(value,0,value); }
		}

		private Coroutine shootCoroutine = null;

		private void Awake () {
			I_Magazine = Magazine;
			I_Ammo = Ammo;
		}

		private void OnValidate () {
			Magazine = _Magazine;
			Ammo = _Ammo;
		}

		public void ShootStart () {
			if (shootCoroutine == null)
				shootCoroutine = StartCoroutine(ShootCoroutine());
		}

		public void ShootStop () {
			if (shootCoroutine == null) return;
			StopCoroutine(shootCoroutine);
			shootCoroutine = null;
		}

		public void Shoot () {
			for (int i = 0;firingPoints.Length > i;i++) {
				bullet.Shot(firingPoints[i],parent,power,speed,time);
			}
			Ammo--;
		}

		public void Replenishment () {
			Magazine = I_Magazine;
			Ammo = I_Ammo;
		}

		private IEnumerator ShootCoroutine () {
			Magazine--;
			while (!IsEmpty) {
				Shoot();
				yield return parent.Timeline.WaitForSeconds(interval);
			}
		}

		private IEnumerator ReloadCoroutine () {
			if (IsEmpty || IsReloading)
				yield break;
			ShootStop();
			IsReloading = true;
			yield return parent.Timeline.WaitForSeconds(reload);
			Ammo = I_Ammo;
			IsReloading = false;
			ShootStart();
		}

	}
}