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
		DontDestroyOnLoad(this.gameObject) ;
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

	}
		
	
	void OnGreedyEvent(RuntimeEvent greedy_events){
		Debug.Log(String.Format("OnGreedyEvent - {0}", greedy_events));

		if (greedy_events == RuntimeEvent.CAMPAIGN_AVAILABLE) {

		}if(greedy_events == RuntimeEvent.CAMPAIGN_NOT_AVAILABLE) {
			if(Application.loadedLevel == 0){
				Application.LoadLevel (1);
			}
		}if(greedy_events == RuntimeEvent.CAMPAIGN_DOWNLOADED) {
			if(Application.loadedLevel == 0){
				Application.LoadLevel (1);
			}
		}
	}


}
