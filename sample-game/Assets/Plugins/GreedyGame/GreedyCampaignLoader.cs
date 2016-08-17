using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using GreedyGame.Runtime;
using GreedyGame.Platform;

public class GreedyCampaignLoader : SingletoneBase<GreedyCampaignLoader>{
	
	public bool isDebug = false;
	void Awake(){
		Debug.Log("NIKHIL inside awake");
		DontDestroyOnLoad(this.gameObject) ;
		if (RuntimePlatform.Android == Application.platform ||
		    Application.isEditor) {
			GreedyGameAgent.Instance.init (isDebug, new GreedyAgentListener());

		}else{
			moveToNextScene();
		}
	}
	
	private static void moveToNextScene(){
		if (Application.loadedLevel == 0) {
			Application.LoadLevel (1);
		}
	}

	public class GreedyAgentListener : IAgentListener {

		public void onAvailable() {
		/**
         * TODO: New campaign is available and ready to use for the next scene.
         **/
			GreedyGameAgent.isInitDone = true;
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
         * TODO: Progress bar can be shown using progress value from 0 to 100.
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

		public void onActionPerformed (string float_unit, string action) {

		}
	}

	public static void fetchFloatAd(String f_id){
		Debug.Log (String.Format ("Fetching FloatUnit {0}", f_id));
		GreedyGameAgent.Instance.fetchFloatUnit (f_id);
	}

	public static void removeFloatAd(){
		GreedyGameAgent.Instance.removeCurrentFloatUnit ();
	}
	
}
