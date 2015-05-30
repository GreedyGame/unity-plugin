using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using GreedyGame.Runtime.Common;
using GreedyGame.Platform;

public class GreedyCampaignLoader : SingletoneBase<GreedyCampaignLoader>{

	public GUIStyle LaterBtnStyle, DownloadBtnStyle;
	public float BtnWidth, BtnHeight;
	public GUITexture loading;	
	private GreedyAdManager ggAdManager = null;

	void Awake(){		
		ggAdManager = GreedyAdManager.Instance;
	}

	void Start() {
		GlobalConfig[] ggLoaders = Resources.FindObjectsOfTypeAll<GlobalConfig> ();
		GlobalConfig ggConfig = ggLoaders [0];
		ggAdManager.init (ggConfig.GameId, ggConfig.AdUnits.ToArray(), OnGreedyEvent);
	}
	
	void OnGUI () {
		if(ggAdManager.isNewCampaign){
			loading.enabled = false;
			Rect a = new Rect (0, Screen.height - 200, Screen.width*ggAdManager.progress/100.0f, 30);
			DrawRectangle (a, Color.white);
			if(ggAdManager.isForced == false){
				if (GUI.Button(new Rect ( (Screen.width - BtnWidth)/2 - BtnWidth/2, Screen.height - 100, BtnWidth, BtnHeight), "Next Time", LaterBtnStyle)) {
					ggAdManager.cancelDownload();
				}
			}
		}
	}


	void OnGreedyEvent(RuntimeEvent greedy_events){
		Debug.Log(String.Format("OnGreedyEvent - {0}", greedy_events));
		if (greedy_events == RuntimeEvent.CAMPAIGN_LOADED || 
		    greedy_events == RuntimeEvent.CAMPAIGN_NOT_LOADED) {
			Application.LoadLevel (1);
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

