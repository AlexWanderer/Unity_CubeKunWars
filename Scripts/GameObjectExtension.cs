using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameObjectExtension {

	public static void SetLayer (this GameObject gameObject,int layer, bool needSetChildrens = true) {
		if(!gameObject) return;

		gameObject.layer = layer;
		
		if(!needSetChildrens) return;
		
		foreach(Transform childTransform in gameObject.transform)
		  SetLayer(childTransform.gameObject,layer,needSetChildrens);
	}

	public static void SetLayer (this GameObject gameObject,string layerName,bool needSetChildrens = true) {
		SetLayer(gameObject,LayerMask.NameToLayer(layerName),needSetChildrens);
	}
}