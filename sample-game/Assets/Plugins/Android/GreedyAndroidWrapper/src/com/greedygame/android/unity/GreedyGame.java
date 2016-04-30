package com.greedygame.android.unity;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.HashSet;
import java.util.List;
import java.util.Set;

import android.app.Activity;
import android.util.Log;
import android.widget.FrameLayout;
import android.widget.Toast;

import com.greedygame.android.adhead.FloatAdLayout;
import com.greedygame.android.agent.GreedyGameAgent;
import com.greedygame.android.agent.GreedyGameAgent.FetchType;
import com.greedygame.android.agent.GreedyGameAgent.OnInitEvent;
import com.greedygame.android.agent.IAgentListener;
import com.unity3d.player.UnityPlayer;

public class GreedyGame {

	protected static String TAG = "GreedyGame";
	private static GreedyGameAgent ggAgent = null;
	private String gameObjectName;
	private String version = "6.4";
	Activity gameActivity = null;

	private FloatAdLayout ggFloat = null;

	public GreedyGame() {
		try {
			
			 
			gameActivity = UnityPlayer.currentActivity;
			ggAgent = GreedyGameAgent.install(gameActivity, new GreedyListener());
			ggFloat = new FloatAdLayout(gameActivity);
			ggAgent.gameEngine = "unity";

			gameActivity.runOnUiThread(new Runnable() {
				public void run() {
					FrameLayout.LayoutParams params = new FrameLayout.LayoutParams(
							FrameLayout.LayoutParams.WRAP_CONTENT,
							FrameLayout.LayoutParams.WRAP_CONTENT);
					gameActivity.addContentView(ggFloat, params);
				}
			});
			Log.i("GreedyGame", "Agent version = " + ggAgent.getVersion()
					+ "Wrapper verison = " + version);
			this.setDebug(false);
		} catch (Exception e) {
			LogE("sdk error ", e);
		}
	}

	public void init(String _gameObject, String _gameId, String[] _units, boolean isEdit) {
		Log.i("GreedyGame", "_gameObject = " + _gameObject + ", _gameId = "
				+ _gameId + ", isEdit = " + isEdit);
		try {
			gameObjectName = _gameObject;

			// Remove null and empty
			List<String> list = new ArrayList<String>(Arrays.asList(_units));
			list.removeAll(Arrays.asList("", null));

			// Remove duplicates
			Set<String> stringSet = new HashSet<String>(list);
			String[] filteredArray = stringSet.toArray(new String[0]);
			

			if (isEdit) {
				ggAgent.isDebug = true;
			}
			//ggAgent.onCustomEvent("ggstart");
			ggAgent.init(filteredArray, FetchType.DOWNLOAD_BY_ID);

		} catch (Exception e) {
			LogE("sdk error ", e);
		}
	}

	public String activeTheme() {
		try {
			return ggAgent.activeTheme();
		} catch (Exception e) {
			LogE("sdk error ", e);
		}
		return null;
	}

	public float getProgress() {
		try {
			return this.progress;
		} catch (Exception e) {
			LogE("sdk error ", e);
		}
		return 100;
	}

	public void fetchHeadAd(final String unit_id) {
		gameActivity.runOnUiThread(new Runnable() {
			public void run() {
				try {
					ggFloat.fetchHeadAd(unit_id);
				} catch (Exception e) {
					LogE("sdk error ", e);
				}
			}
		});
	}

	public void fetchHeadAd(final String unit_id, final int x, final int y) {
		gameActivity.runOnUiThread(new Runnable() {
			public void run() {
				try {
					ggFloat.fetchHeadAd(unit_id, x, y);
				} catch (Exception e) {
					LogE("sdk error ", e);
				}
			}
		});
	}

	public void removeAllHeadAd() {
		gameActivity.runOnUiThread(new Runnable() {
			public void run() {
				try {
					ggFloat.remove();
				} catch (Exception e) {
					LogE("sdk error ", e);
				}
			}
		});
	}


	public void showEngagementWindow(){
			//ggFloat.showEngagementWindow();
		}

	public void showEngagementWindow(String unit_id){
			ggFloat.showEngagementWindow(unit_id);
		}
	

	public String getActivePath() {
		try {
			return ggAgent.getActivePath();
		} catch (Exception e) {
			LogE("sdk error ", e);
		}
		return null;
	}

	public void onCustomEvent(String event_name) {
		try {
			//ggAgent.onCustomEvent(event_name);
		} catch (Exception e) {
			LogE("sdk error ", e);
		}
	}

	public void setDebug(boolean b) {
		try {
			isDebug = b;
			ggAgent.setDebug(b);
		} catch (Exception e) {
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

	private class GreedyListener implements IAgentListener {

		

		@Override
		public void onDownload() {
			
				String t = activeTheme();
				if (t == null) {
					t = "";
				}
				UnityPlayer.UnitySendMessage(gameObjectName, "GG_DownloadSuccess",
						t);

			

		}

		@Override
		public void onProgress(float _progress) {

			progress = _progress;
		}

		@Override
		public void onInit(OnInitEvent arg0) {
			int r = -1;
			/*
			 * -1 = using no campaign 0 = campaign already cached 1 = new
			 * campaign found to download
			 */

			if (arg0 == OnInitEvent.CAMPAIGN_NOT_AVAILABLE) {
				r = -1;
			} else if (arg0 == OnInitEvent.CAMPAIGN_AVAILABLE) {
				r = 1;
			}

			UnityPlayer.UnitySendMessage(gameObjectName, "GG_onInit",
					Integer.toString(r));
		}


		@Override
		public void onError() {
			UnityPlayer.UnitySendMessage(gameObjectName,"GG_DownloadError",null);
		}

		@Override
		public void onPermissionsUnavailable(ArrayList<String> permissions) {
			

	}
	

}

}



