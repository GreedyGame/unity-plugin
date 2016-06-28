using UnityEngine;
using System;
using System.Collections;
using GreedyGame.Runtime.Common;
using GreedyGame.Platform;

public class FloatUnitLoader : MonoBehaviour {

	public string FloatUnit = null;
	private GreedyAdManager ggAdManager = null;
	
	void Awake (){	
		Debug.Log(String.Format("FloatUnitLoader - awake {0}", FloatUnit));
		ggAdManager = GreedyAdManager.Instance;
	}

	void Start (){
		ggAdManager.fetchFloatUnit (FloatUnit);
	}
	
	void OnDestroy (){
		ggAdManager.removeAllFloatUnits ();
	}
	
}
