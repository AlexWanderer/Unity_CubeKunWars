using UnityEngine;

public class UIFollowTarget : MonoBehaviour {
	
	public Transform target;
	public Vector2 offset;
	private RectTransform rectTransform = null;

	void Awake () {
		rectTransform = GetComponent<RectTransform>();
	}

	void Update () {
		if (!target) return;
		rectTransform.position = RectTransformUtility.WorldToScreenPoint(Camera.main,target.position) + offset;
	}
}