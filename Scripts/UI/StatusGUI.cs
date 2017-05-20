using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MackySoft.CubeKunWars {

	[RequireComponent(typeof(UIFollowTarget))]
	public class StatusGUI : MonoBehaviour {

		#region Variables

		private static StatusGUI _Prefab = null;
		private static Canvas _Canvas = null;

		[Header("References")]
		public Image hpBar;
		public Image arrow;
		
		[Header("Color")]
		public bool colorChange = true;
		public Color safetyColor = Color.blue;
		public Color warningColor = Color.yellow;
		public Color dangerColor = Color.red;

		private RectTransform hpBarTr;
		private UIFollowTarget uft;
		
		#endregion

		#region Properties

		public static StatusGUI Prefab {
			get { return _Prefab ? _Prefab : (_Prefab = Resources.Load<StatusGUI>("Prefabs/UI/StatusGUI")); }
		}
		
		public static Canvas Canvas {
			get { return _Canvas ? _Canvas : (_Canvas = FindObjectOfType<Canvas>()); }
			set { _Canvas = value; }
		}
		
		public CKWBehaviour Behaviour { get; private set; }

		#endregion
		
		private void Awake () {
			hpBarTr = hpBar.GetComponent<RectTransform>();
			uft = GetComponent<UIFollowTarget>();
		}

		public void Initialize (CKWBehaviour behaviour) {
			if (Behaviour) Disable();
			Behaviour = behaviour;

			uft.target = Behaviour.Tr;
			Behaviour.Health.onValueChanged.AddListener(OnHealthValueChanged);
			UpdateArrowColor();
			OnHealthValueChanged(Behaviour.Health);

			gameObject.SetActive(true);
		}

		public void Disable () {
			if (!Behaviour)
				throw new NullReferenceException();
			
			Behaviour.Health.onValueChanged.RemoveListener(OnHealthValueChanged);
			Behaviour = null;

			gameObject.SetActive(false);
		}

		public void UpdateArrowColor () {
			if (!Behaviour)
				throw new NullReferenceException();
			switch (Behaviour.Team) {
				case Team.Independent: arrow.color = Color.white; break;
				case Team.Ally: arrow.color = Color.blue; break;
				case Team.Enemy: arrow.color = Color.red; break;
			}
		}
		
		private void OnHealthValueChanged (Health health) {
			float p = (float)health.Value / (float)health.Max;
			hpBarTr.localScale = new Vector3(p,1,1);
			if (colorChange) {
				if (p > 0.5f)
					hpBar.color = safetyColor;
				else if (p > 0.25f)
					hpBar.color = warningColor;
				else
					hpBar.color = dangerColor;
			}
		}
	}
}