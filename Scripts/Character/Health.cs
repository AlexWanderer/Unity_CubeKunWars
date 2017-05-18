using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MackySoft.CubeKunWars {

	[Serializable]
	public class HealthEvent : UnityEvent<Health> { }

	public class Health : MonoBehaviour {
		
		[SerializeField]
		private int _Value = 100;
		[SerializeField]
		private int _Max = 100;

		[Header("Events")]
		public HealthEvent onValueChanged;
		public HealthEvent onDie;

		public int Value {
			get { return _Value; }
			set {
				_Value = Mathf.Clamp(value,0,Max);
				onValueChanged.Invoke(this);
				if (IsDead) onDie.Invoke(this);
			}
		}

		public int Max {
			get { return _Max; }
			set {
				_Max = Mathf.Clamp(value,1,_Max);
				Value = Value;
			}
		}

		public bool IsDead {
			get { return Value == 0; }
		}

		private void OnValidate () {
			Value = _Value;
			Max = _Max;
		}

	}
}