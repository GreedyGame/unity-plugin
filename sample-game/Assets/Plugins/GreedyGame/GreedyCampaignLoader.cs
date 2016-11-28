using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using GreedyGame.Runtime;
using GreedyGame.Platform;

public class GreedyCampaignLoader : SingletoneBase<GreedyCampaignLoader>{
	
	void Awake(){
		DontDestroyOnLoad(this.gameObject) ;
		if (RuntimePlatform.Android == Application.platform) {
			GreedyGameAgent.Instance.setCampaignStateListener(new StateListener());
			GreedyGameAgent.Instance.setCampaignProgressListener(new ProgressListener());
			GreedyGameAgent.Instance.init ();
		}else{
			moveToNextScene();
		}
	}
	
	private static void moveToNextScene(){
		if (Application.loadedLevel == 0) {
			Application.LoadLevel (1);
		}
	}

	public class StateListener : CampaignStateListener {

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

		public void onFound() {
			/**
         * TODO: Campaign is found. Starting download of assets. This will be followed by onAvailable callback once download completes successfully.
         **/
			moveToNextScene();
		}

		
	}

	public class ProgressListener : CampaignProgressListener {

		public void onProgress(int progress) {
		
		}
		
	}

	public static void showFloat(String f_id){
		Debug.Log (String.Format ("Fetching FloatUnit {0}", f_id));
		GreedyGameAgent.Instance.showFloat(f_id);
	}

	public static void removeFloat(string f_id){
		Debug.Log (String.Format ("Remove FloatUnit"));
		GreedyGameAgent.Instance.removeFloat(f_id);
	}
	
}
