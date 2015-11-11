using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using GreedyGame.Runtime.Common;
using GreedyGame.Platform;

public class GreedyCampaignLoader : SingletoneBase<GreedyCampaignLoader>{
	
	private bool isSupported = false;

	private GreedyAdManager ggAdManager = null;
	void Awake(){
		if (RuntimePlatform.Android == Application.platform ||
		    Application.isEditor) {
			isSupported = true;
			ggAdManager = GreedyAdManager.Instance;
		}else{
			Application.LoadLevel (1);
		}
	}
	
	void Start() {
		if (isSupported) {
			GlobalConfig[] ggLoaders = Resources.FindObjectsOfTypeAll<GlobalConfig> ();
			if(ggLoaders == null || ggLoaders.Length == 0){
				isSupported = false;
				Debug.LogError("No occurrence of GlobalConfig object found!\nGoto GreedyGame > DynamicUnitManager > Save");
				return;
			}
			GlobalConfig ggConfig = ggLoaders [0];
			ggAdManager.init (ggConfig.GameId, ggConfig.AdUnits.ToArray (), ggConfig.isDebug, ggConfig.isLazyLoad, OnGreedyEvent);
		}

		Application.LoadLevel (1);
	}
	
	void OnGUI () {
		if(isSupported && ggAdManager.isNewCampaign){
			Rect a = new Rect (0, Screen.height/2, Screen.width*ggAdManager.progress/100.0f, 30);
			DrawRectangle (a, Color.black);
		}
	}
	
	void OnGreedyEvent(RuntimeEvent greedy_events){
		Debug.Log(String.Format("OnGreedyEvent - {0}", greedy_events));
		if (greedy_events == RuntimeEvent.CAMPAIGN_LOADED || 
		    greedy_events == RuntimeEvent.CAMPAIGN_NOT_LOADED) {
			//Application.LoadLevel (1);
		}
	}

	void DrawRectangle (Rect position, Color color) {    
		Texture2D texture = new Texture2D(1, 1);
		texture.SetPixel(0,0,color);
		texture.Apply();
		GUI.skin.box.normal.background = texture;
		GUI.Box(position, GUIContent.none);
	}
}
