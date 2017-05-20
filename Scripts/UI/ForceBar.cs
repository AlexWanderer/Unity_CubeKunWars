using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MackySoft.CubeKunWars {
	public class ForceBar : MonoBehaviour {

		public static ForceBar Instance {
			get { return _Instance ? _Instance : (_Instance = FindObjectOfType<ForceBar>()); }
		}
		private static ForceBar _Instance = null;
		
		public RectTransform allyBarTr;
		public RectTransform enemyBarTr;
		public Text allyCount;
		public Text enemyCount;

		private void Start () {
			BarUpdate();
		}

		public void BarUpdate () {
			float p = (float)CubeKun.list.Count(c => c.Team == Team.Ally) / (float)CubeKun.list.Count * 2;
			allyBarTr.localScale = new Vector3(p,1,1);
			enemyBarTr.localScale = new Vector3(2- p,1,1);
			allyCount.text = CubeKun.list.Count(c => c.Team == Team.Ally).ToString();
			enemyCount.text = CubeKun.list.Count(c => c.Team == Team.Enemy).ToString();
		}

	}
}