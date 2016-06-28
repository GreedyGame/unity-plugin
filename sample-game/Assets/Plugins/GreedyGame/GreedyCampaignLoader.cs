using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using GreedyGame.Runtime.Common;
using GreedyGame.Platform;

public class GreedyCampaignLoader : SingletoneBase<GreedyCampaignLoader>{
	
	public bool isDebug = false;
	private bool isLazyLoad = true;
	void Awake(){
		DontDestroyOnLoad(this.gameObject) ;
		if (RuntimePlatform.Android == Application.platform ||
		    Application.isEditor) {
			GreedyAdManager.Instance.init (isDebug, isLazyLoad, new GreedyAgentListener());
		}else{
			moveToNextScene();
		}
	}
	
	private static void moveToNextScene(){
		if (Application.loadedLevel == 0) {
			Application.LoadLevel (1);
		}
	}

	public class GreedyAgentListener : IAgentListener
	{
		public void onAvailable() {
			/**
			* TODO: New campaign is available and ready to use for the next scene.
			**/
			moveToNextScene();
		}

		public void onUnavailable() {
			/**
			* TODO: No campaign is available, proceed with normal follow of the game.
			**/
			moveToNextScene();
		}

		public void onProgress(int progress) {
			/**
			* TODO: progress will give progress value of download from 0 to 100.
			* This can be used to render loading bar.
			**/
		}

		public void onPermissionsUnavailable(string[] permissions) {
			/**
         * TODO: Prompt user to give required permission
         **/
			for(int i = 0; i < permissions.Length; i++) {
				string p = permissions[i];
				Debug.Log(String.Format("permission unavailable = {0}", p));
			}
		}
	}

	public static void fetchFloatAd(String f_id){
		Debug.Log (String.Format ("Fetching FloatUnit {0}", f_id));
		GreedyAdManager.Instance.fetchFloatUnit (f_id);
	}

	public static void removeFloatAd(){
		GreedyAdManager.Instance.removeAllFloatUnits ();
	}
	
	
}
