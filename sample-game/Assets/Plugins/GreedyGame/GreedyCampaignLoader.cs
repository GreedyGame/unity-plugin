using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using GreedyGame.Runtime.Common;
using GreedyGame.Platform;

public class GreedyCampaignLoader : SingletoneBase<GreedyCampaignLoader>{

	public Texture2D SkipButton;
	public int PostLevel;
	private int BtnWidth = 150;
	private int BtnHeight = 70;
	private bool isSupported = false;

	private GreedyAdManager ggAdManager = null;
	void Awake(){
		if (RuntimePlatform.Android == Application.platform) {
			isSupported = true;
			ggAdManager = GreedyAdManager.Instance;
		}else{
			Application.LoadLevel (PostLevel);
		}
	}
	
	void Start() {
		if (isSupported) {
			GlobalConfig[] ggLoaders = Resources.FindObjectsOfTypeAll<GlobalConfig> ();
			if(ggLoaders != null && ggLoaders.Length != 1){
				isSupported = false;
				Debug.LogError("None or multuple occurrence of GlobalConfig object found!\nGoto GreedyGame > DynamicUnitManager > Save");
				return;
			}
			GlobalConfig ggConfig = ggLoaders [0];
			ggAdManager.init (ggConfig.GameId, ggConfig.AdUnits.ToArray (), OnGreedyEvent);

		}

	
	}
	
	void OnGUI () {
		if(isSupported && ggAdManager.isNewCampaign){
			Rect a = new Rect (0, Screen.height/2, Screen.width*ggAdManager.progress/100.0f, 30);
			DrawRectangle (a, Color.black);
			if(ggAdManager.isForced == false){
				if (GUI.Button(new Rect (Screen.width - BtnWidth, Screen.height - 100, BtnWidth, BtnHeight), SkipButton, GUIStyle.none)) {
					ggAdManager.cancelDownload();
				}
			}
		}
	}
	
	void OnGreedyEvent(RuntimeEvent greedy_events){
		Debug.Log(String.Format("OnGreedyEvent - {0}", greedy_events));
		if (greedy_events == RuntimeEvent.CAMPAIGN_LOADED || 
		    greedy_events == RuntimeEvent.CAMPAIGN_NOT_LOADED) {
			Application.LoadLevel (PostLevel);
		}

		if (greedy_events == RuntimeEvent.UNIT_CLOSED){
			Time.timeScale = 1.0f;
		}else if(greedy_events == RuntimeEvent.UNIT_OPENED){
			Time.timeScale = 0.0f;
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
