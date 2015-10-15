package com.greedygame.android.unity;

import android.app.Activity;
import android.util.Log;

import com.greedygame.android.FloatAdLayout;
import com.greedygame.android.GreedyGameAgent;
import com.greedygame.android.GreedyGameAgent.FETCH_TYPE;
import com.greedygame.android.GreedyGameAgent.OnINIT_EVENT;
import com.greedygame.android.IAgentListner;
import com.unity3d.player.UnityPlayer;


public class GreedyGame {

    protected static String TAG = "GreedyGame";
    private static GreedyGameAgent ggAgent = null;
    private String gameObjectName;
    private String version = "6.1";
    Activity gameActivity = null;

	private FloatAdLayout ggFloat = null;
    
	public GreedyGame() {
		try{
			gameActivity = UnityPlayer.currentActivity;
			ggAgent = new GreedyGameAgent(gameActivity, new GreedyListner());
			ggAgent.setAdHeadAnimation(false);
			ggFloat = new FloatAdLayout(gameActivity);
			Log.i("GreedyGame", "Agent version = "+ggAgent.get_verison() +"Wrapper verison = "+version);
			this.setDebug(true);
		}catch(Exception e){
			LogE("sdk error ", e);
		}
	}
	
	public void init(String _gameObject, String _gameId, String []_units){
		try{
			gameObjectName = _gameObject;
			ggAgent.init(_gameId, _units, FETCH_TYPE.DOWNLOAD_BY_ID);
		}catch(Exception e){
			LogE("sdk error ", e);
		}
	}

	public void cancelDownload() {
		try{
			ggAgent.cancelDownload();
		}catch(Exception e){
			LogE("sdk error ", e);
		}
	}
	
	public String activeTheme() {
		try{
			return ggAgent.activeTheme();
		}catch(Exception e){
			LogE("sdk error ", e);
		}
		return null;
	}
	

	public float getProgress() {
		try{
			return this.progress;
		}catch(Exception e){
			LogE("sdk error ", e);
		}
		return 100;
	}
	
	public void fetchHeadAd(String unit_id){
		try{
			ggFloat.fetchHeadAd(unit_id);
		}catch(Exception e){
			LogE("sdk error ", e);
		}
	}

	public void fetchHeadAd(String unit_id, int x, int y){
		try{
			ggFloat.fetchHeadAd(unit_id, x, y);
		}catch(Exception e){
			LogE("sdk error ", e);
		}
	}
	
	public void removeHeadAd(String unit_id){
		try{
			ggFloat.removeHeadAd(unit_id);
		}catch(Exception e){
			LogE("sdk error ", e);
		}
	}
	
	public String newTheme() {
		try{
			return ggAgent.newTheme();
		}catch(Exception e){
			LogE("sdk error ", e);
		}
		return null;
	}
	
	public int isForceUpdate() {
		try{
			if(ggAgent.isForceUpdate()) {
				return 1;
			}else {
				return 0;
			}
		}catch(Exception e){
			LogE("sdk error ", e);
		}
		return 0;
	}
	
	public String getActivePath(){
		try{
			return ggAgent.getActivePath();
		}catch(Exception e){
			LogE("sdk error ", e);
		}
		return null;
	}
	
	public void onStartEvent(){
		try{
			ggAgent.onCustomEvent("UnityOnStart");
		}catch(Exception e){
			LogE("sdk error ", e);
		}
	}
	
	public void onDestroyEvent(){
		try{
			ggAgent.onCustomEvent("UnityOnDestroy");
		}catch(Exception e){
			LogE("sdk error ", e);
		}
	}

	public void onCustomEvent(String event_name){
		try{
			ggAgent.onCustomEvent(event_name);
		}catch(Exception e){
			LogE("sdk error ", e);
		}
	}

	public void setDebug(boolean b) {
		try{
			isDebug = b;
			ggAgent.setDebug(b);
		}catch(Exception e){
			LogE("sdk error ", e);
		}
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
