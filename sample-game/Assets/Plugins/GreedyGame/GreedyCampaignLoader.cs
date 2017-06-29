using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using GreedyGame.Runtime;
using GreedyGame.Platform;
using GreedyGame.Runtime.Units;

public class GreedyCampaignLoader : SingletoneBase<GreedyCampaignLoader>{
	
	void Awake(){
		DontDestroyOnLoad(this.gameObject) ;
		if (RuntimePlatform.Android == Application.platform) {
			GreedyGameAgent.Instance.init (new GreedyAgentListener());
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
            Debug.Log("Inside onAvailable function");
            moveToNextScene();
            refreshNativeUnits();
            

		}

        private void refreshNativeUnits()
        {
            NativeUnit[] nativeScriptObjects = FindObjectsOfType(typeof(NativeUnit)) as NativeUnit[];
            foreach(NativeUnit nativeUnit in nativeScriptObjects) {
                nativeUnit.GG_SetUpTexture();
            }

            SharedNativeUnit[] sharedScriptObjects = FindObjectsOfType(typeof(SharedNativeUnit)) as SharedNativeUnit[];
            foreach (SharedNativeUnit sharedUnit in sharedScriptObjects)
            {
                sharedUnit.GG_SetUpTexture();
            }

            GreedyGameAgent.Instance.removeAllFloatUnits();
            GreedyGameAgent.Instance.fetchFloatUnit("float-2002");
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
			//moveToNextScene();
		}

		public void onProgress(int progress) {
			/**
         * TODO: Campaign progress from 1-100.
         **/
		}

        public void onError(string error)
        {
            /**
         * TODO: No Campaign will be served since the initialization resulted in an error. 
         * If device api level is below 15 this callback is invoked.
         **/
            moveToNextScene();
        }


    }

	public static void showFloat(String f_id){
		Debug.Log (String.Format ("Fetching FloatUnit {0}", f_id));
		GreedyGameAgent.Instance.fetchFloatUnit (f_id);
	}

	public static void removeFloatAd(string FloatUnit){
		Debug.Log (String.Format ("Remove FloatUnit"));
		GreedyGameAgent.Instance.removeFloatUnit (FloatUnit);
	}

	public static void removeAllFloatAds(){
		Debug.Log (String.Format ("Remove AllFloatUnits"));
		GreedyGameAgent.Instance.removeAllFloatUnits ();

	}
	
}
