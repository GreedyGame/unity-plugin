using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using GreedyGame.Runtime;
using GreedyGame.Platform;
using GreedyGame.Runtime.Units;
using GreedyGame.Commons;

public class GreedyCampaignLoader : SingletoneBase<GreedyCampaignLoader>{

    public List<string> unitList;

    public bool AdmobMediation = false;

    public bool FacebookMediation = false;

    public bool MopubMediation = false;

    public bool EnableCrashReporting = true;
	
	void Awake(){
		DontDestroyOnLoad(this.gameObject) ;
		if (RuntimePlatform.Android == Application.platform) {
            GGAdConfig adConfig = new GGAdConfig();
            adConfig.setListener(new GreedyAgentListener());
            adConfig.enableCrash(EnableCrashReporting);
            adConfig.enableAdmob(AdmobMediation);
            adConfig.enableFAN(FacebookMediation);
            adConfig.enableMopub(MopubMediation);
            adConfig.addUnitList(unitList);
			GreedyGameAgent.Instance.init (adConfig);
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

		public void onAvailable(string campaignId) {
            /**
         * TODO: New campaign is available and ready to use for the next scene.
         **/

		}

        public void onUnavailable() {
			/**
         * TODO: No campaign is available, proceed with normal flow of the game.
         **/
		}

		public void onFound() {
			/**
         * TODO: Campaign is found. Starting download of assets. This will be followed by onAvailable callback once download completes successfully.
         **/
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
        }

        public void onProceed()
        {
            /**
         * TODO: Make use of this call to switch from loading scene to the first scene.
         * Make sure that you perform checks to see if you are in the loading scene and then only switch 
         * scenes or else upon a refresh call you might get switched from an intermediate scene to 
         * the next one. ( Refer to moveToNextScene function )
         **/
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
