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
		ggAdManager.RemoveAdHead (AdUnit);
	}

	void OnGUI() {
		if (GUI.Button(new Rect(10, 10, 200, 200), "HIDE")){
			Debug.Log("Clicked the button with text");
			ggAdManager.RemoveAdHead (AdUnit);
		}

		if (GUI.Button(new Rect(210, 10, 200, 200), "SHOW")){
			Debug.Log("Clicked the button with text");
			ggAdManager.FetchAdHead (AdUnit);
		}
	}
}
