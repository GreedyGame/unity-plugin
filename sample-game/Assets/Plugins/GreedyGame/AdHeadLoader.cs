using UnityEngine;
using System;
using System.Collections;
using GreedyGame.Runtime.Common;
using GreedyGame.Platform;

public class AdHeadLoader : MonoBehaviour {

	public string AdUnit = null;
	private GreedyAdManager ggAdManager = null;
	
	void Awake (){	
		Debug.Log(String.Format("AdHeadLoader - awake {0}", AdUnit));
		ggAdManager = GreedyAdManager.Instance;
	}

	void Start (){
		ggAdManager.FetchAdHead (AdUnit);
	}
	
	void OnDestroy (){
		ggAdManager.RemoveAllAdHead ();
	}
	
}
