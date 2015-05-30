package com.greedygame.android.unity;

import android.app.Activity;
import android.util.Log;

import com.greedygame.android.GreedyGameAgent;
import com.greedygame.android.GreedyGameAgent.FETCH_TYPE;
import com.greedygame.android.GreedyGameAgent.OnINIT_EVENT;
import com.greedygame.android.IAgentListner;
import com.unity3d.player.UnityPlayer;

public class GreedyGame {

    protected static String TAG = "GreedyGame";
    private static GreedyGameAgent ggAgent = null;
    private String gameObjectName;
    private String version = "5.7";
    Activity gameActivity = null;
    
	public GreedyGame() {
		gameActivity = UnityPlayer.currentActivity;
		
		
		ggAgent = new GreedyGameAgent(gameActivity, new GreedyListner());
		ggAgent.setAdHeadAnimation(false);
		Log.i("GreedyGame", "Agent version = "+ggAgent.get_verison() +"Wrapper verison = "+version);
		this.setDebug(true);
	}
	
	public void init(String _gameObject, String _gameId, String []_units){
		gameObjectName = _gameObject;
		ggAgent.init(_gameId, _units, FETCH_TYPE.DOWNLOAD_BY_ID);
	}

	public void cancelDownload() {
		ggAgent.cancelDownload();
	}
	
	public String activeTheme() {
		return ggAgent.activeTheme();
	}
	

	public float getProgress() {
		return this.progress;
	}
	
	public void fetchHeadAd(String unit_id){
		ggAgent.fetchHeadAd(unit_id);
	}
	
	public void fetchHeadAd(String unit_id, float x, float y){
		ggAgent.fetchHeadAd(unit_id,x,y);
	}
	
	public void removeHeadAd(String unit_id){
		ggAgent.removeHeadAd(unit_id);
	}
	
	public String newTheme() {
		return ggAgent.newTheme();
	}
	
	public int isForceUpdate() {
		if(ggAgent.isForceUpdate()) {
			return 1;
		}else {
			return 0;
		}
	}
	
	public String getActivePath(){
		return ggAgent.getActivePath();
	}
	
	public void setDebug(boolean b) {
		isDebug = b;
		ggAgent.setDebug(b);
	}
	
    private static boolean isDebug = false;
    protected static void LogE(String msg, Exception e) {
        Log.e(TAG, msg, e);
        e.printStackTrace();
    }

    protected static void LogD(String msg) {
        if (isDebug) {
            Log.d(TAG, msg);
        }
    }
    
    float progress = 0;
    private class GreedyListner implements IAgentListner{
    	
		@Override
		public void onDownload(boolean success) {
			if(success){
				String t = activeTheme();
       			if(t == null){
       				t = "";
       			}
				UnityPlayer.UnitySendMessage(gameObjectName, "GG_postDownload", t);
				
			}else{
				//ToDO: fail campaign
			}
			

			
		}
		
		@Override
		public void onProgress(float _progress) {
			
			progress = _progress;
		}

		@Override
		public void onUnitClicked(boolean clicked) {
			Log.i("GreedyGame", "onUnitClicked = "+clicked);
			int i = 0;
			if(clicked){
				i = 1;
			}
			UnityPlayer.UnitySendMessage(gameObjectName, "GG_onClicked", Integer.toString(i));
			
		}

		@Override
		public void onInit(OnINIT_EVENT arg0) {
			int r = -1;
			/*
			 * -1 = using no campaign
			 * 0 = campaign already cached
			 * 1 = new campaign found to download
			 */   
			
			if(arg0 == OnINIT_EVENT.CAMPAIGN_NOT_AVAILABLE) {
				r = -1;
			}else if(arg0 == OnINIT_EVENT.CAMPAIGN_CACHED) {
				r = 0;
			}else if(arg0 == OnINIT_EVENT.CAMPAIGN_FOUND) {
				r = 1;
			}
   			
   			UnityPlayer.UnitySendMessage(gameObjectName, "GG_onInit", Integer.toString(r));  	
		}
    }

}
