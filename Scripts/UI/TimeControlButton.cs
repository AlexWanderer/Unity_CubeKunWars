using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Chronos;

namespace MackySoft.CubeKunWars {

	[RequireComponent(typeof(Button))]
	public class TimeControlButton : MonoBehaviour {
		
		public Sprite pausedIcon;
		public Sprite normalIcon;
		public Sprite acceleratedIcon;
		private Button button;
		private Image image;

		private void Awake () {
			button = GetComponent<Button>();
			image = transform.GetChild(0).GetComponent<Image>();
		}

		private void Start () {
			button.onClick.AddListener(OnClick);
			SetIcon();
		}

		private void Update () {
			if (Input.GetKeyDown(KeyCode.T)) {
				OnClick();
			}
		}

		private void OnClick () {
			switch (GameManager.TimeState) {
				//case TimeState.Paused: GameManager.Clock.localTimeScale = 1; break;
				case TimeState.Normal: GameManager.Clock.localTimeScale = 2; break;
				case TimeState.Accelerated: GameManager.Clock.localTimeScale = 1; break;
			}
			SetIcon();
		}

		private void SetIcon () {
			switch (GameManager.TimeState) {
				//case TimeState.Paused: image.sprite = pausedIcon; break;
				case TimeState.Normal: image.sprite = normalIcon; break;
				case TimeState.Accelerated: image.sprite = acceleratedIcon; break;
			}
		}

	}
}