using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

using MoPubReward = MoPubManager.MoPubReward;

public class MoPubDemoGUI : MonoBehaviour
{
	private int _selectedToggleIndex;
	private Dictionary<string, List<MoPubReward>> _adUnitToRewardsMapping =
		new Dictionary<string, List<MoPubReward>> ();
	private Dictionary<string, bool> _adUnitToLoadedMapping =
		new Dictionary<string, bool> ();
	private Dictionary<string, bool> _bannerAdUnitToShownMapping =
		new Dictionary<string, bool> ();

	// Workaround for lacking adUnit from onAdLoadedEvent for Banners
	private Queue<string> _requestedBannerAdUnits = new Queue<string> ();

	#if UNITY_ANDROID
	private string[] _bannerAdUnits = new string[] { "b195f8dd8ded45fe847ad89ed1d016da" };
	private string[] _interstitialAdUnits = new string[] { "24534e1901884e398f1253216226017e" };
	private string[] _rewardedVideoAdUnits = new string[] { "920b6145fb1546cf8b5cf2ac34638bb7" };
	private string[] _rewardedRichMediaAdUnits = new string[] { "15173ac6d3e54c9389b9a5ddca69b34b" };
	#elif UNITY_IPHONE
	private string[] _bannerAdUnits =
			new string[] { "0ac59b0996d947309c33f59d6676399f", "23b49916add211e281c11231392559e4" };
	private string[] _interstitialAdUnits =
			new string[] { "4f117153f5c24fa6a3a92b818a5eb630", "3aba0056add211e281c11231392559e4" };
	private string[] _rewardedVideoAdUnits =
			new string[] { "8f000bd5e00246de9c789eed39ff6096", "98c29e015e7346bd9c380b1467b33850" };
	private string[] _rewardedRichMediaAdUnits = new string[] {  };
	#else
	private string[] _bannerAdUnits = new string[] {  };
	private string[] _interstitialAdUnits = new string[] {  };
	private string[] _rewardedVideoAdUnits = new string[] {  };
	private string[] _rewardedRichMediaAdUnits = new string[] {  };
	#endif

	// Label style for no ad unit messages
	private GUIStyle _smallerFont;

	// Buffer space between sections
	private int _sectionMarginSize;

	// Currently selected network
	private string _network;

	// Default text for custom data fields
	private static string _customDataDefaultText = "Optional custom data";

	// String to fill with custom data for Rewarded Videos
	private string _rvCustomData = _customDataDefaultText;

	// String to fill with custom data for Rewarded Rich Media
	private string _rrmCustomData = _customDataDefaultText;


	private static bool IsAdUnitArrayNullOrEmpty (string[] adUnitArray) {
		return (adUnitArray == null || adUnitArray.Length == 0);
	}


	private void addAdUnitsToStateMaps (string[] adUnits) {
		foreach (string adUnit in adUnits) {
			_adUnitToLoadedMapping.Add (adUnit, false);
			// Only banners need this map, but init for all to keep it simple
			_bannerAdUnitToShownMapping.Add (adUnit, false);
		}
	}


	public void loadAvailableRewards (string adUnitId, List<MoPubReward> availableRewards) {
		// Remove any existing available rewards associated with this AdUnit from previous ad requests
		_adUnitToRewardsMapping.Remove (adUnitId);

		if (availableRewards != null) {
			_adUnitToRewardsMapping[adUnitId] = availableRewards;
		}
	}


	public void bannerLoaded () {
		if (_requestedBannerAdUnits.Count > 0) {
			string firstRequestedBannerAdUnit = _requestedBannerAdUnits.Dequeue ();
			_adUnitToLoadedMapping[firstRequestedBannerAdUnit] = true;
			_bannerAdUnitToShownMapping[firstRequestedBannerAdUnit] = true;
		}
	}


	public void adLoaded (string adUnit) {
		_adUnitToLoadedMapping[adUnit] = true;
	}


	public void adDismissed (string adUnit) {
		_adUnitToLoadedMapping[adUnit] = false;
	}


	void Start () {
		addAdUnitsToStateMaps (_bannerAdUnits);
		addAdUnitsToStateMaps (_interstitialAdUnits);
		addAdUnitsToStateMaps (_rewardedVideoAdUnits);
		addAdUnitsToStateMaps (_rewardedRichMediaAdUnits);

		#if UNITY_ANDROID && !UNITY_EDITOR
		MoPub.loadBannerPluginsForAdUnits (_bannerAdUnits);
		MoPub.loadInterstitialPluginsForAdUnits (_interstitialAdUnits);
		MoPub.loadRewardedVideoPluginsForAdUnits (_rewardedVideoAdUnits);
		MoPub.loadRewardedVideoPluginsForAdUnits (_rewardedRichMediaAdUnits);
		#elif UNITY_IPHONE && !UNITY_EDITOR
		MoPub.loadPluginsForAdUnits(_bannerAdUnits);
		MoPub.loadPluginsForAdUnits(_interstitialAdUnits);
		MoPub.loadPluginsForAdUnits(_rewardedVideoAdUnits);
		MoPub.loadPluginsForAdUnits(_rewardedRichMediaAdUnits);
		#endif

		#if !UNITY_EDITOR
		if (!IsAdUnitArrayNullOrEmpty (_rewardedVideoAdUnits)) {
			MoPub.initializeRewardedVideo ();
		}
		#endif

		#if !(UNITY_ANDROID || UNITY_IPHONE)
		Debug.LogWarning("Please switch to either Android or iOS platforms to run sample app!");
		#endif
	}


	void OnGUI () {
		ConfigureGUI ();

		Rect guiArea = new Rect(0,0,100,100);
		guiArea.x += 20;
		guiArea.width -= 40;
		GUILayout.BeginArea(guiArea);
		GUILayout.BeginVertical ();

		CreateTitleSection ();
		CreateBannersSection ();
		CreateInterstitialsSection ();
		CreateRewardedVideosSection ();
		CreateRewardedRichMediaSection ();
		CreateActionsSection ();

		GUILayout.EndVertical ();
		GUILayout.EndArea ();
	}


	private void ConfigureGUI () {
		// Set default label style
		GUI.skin.label.fontSize = 42;

		// Set default button style
		GUI.skin.button.margin = new RectOffset (0, 0, 10, 0);
		GUI.skin.button.stretchWidth = true;
		GUI.skin.button.fixedHeight = (Screen.width >= 960 || Screen.height >= 960) ? 75 : 50;
		GUI.skin.button.fontSize = 34;

		// Set default text field style
		GUI.skin.textField.stretchWidth = true;
		GUI.skin.textField.fixedHeight = 35;
		GUI.skin.textField.fontSize = 28;

		// Buffer space between sections
		_smallerFont = new GUIStyle (GUI.skin.label);
		_smallerFont.fontSize = GUI.skin.button.fontSize;

		_sectionMarginSize = GUI.skin.label.fontSize;
	}


	private void CreateTitleSection () {
		// App title including Plugin and SDK versions
		GUIStyle centeredStyle = new GUIStyle (GUI.skin.label);
		centeredStyle.alignment = TextAnchor.UpperCenter;
		centeredStyle.fontSize = 48;
		GUI.Label (new Rect (0, 10, Screen.width, 60), MoPub.getPluginName (), centeredStyle);
		centeredStyle.fontSize = _smallerFont.fontSize;
		GUI.Label (new Rect (0, 70, Screen.width, 60), "with " + MoPub.getSDKName (), centeredStyle);
	}


	private void CreateBannersSection () {
		int titlePadding = 102;
		GUILayout.Space (titlePadding);
		GUILayout.Label ("Banners");
		if (!IsAdUnitArrayNullOrEmpty (_bannerAdUnits)) {
			foreach (string bannerAdUnit in _bannerAdUnits) {
				GUILayout.BeginHorizontal ();

				GUI.enabled = !_adUnitToLoadedMapping[bannerAdUnit];
				if (GUILayout.Button (CreateRequestButtonLabel (bannerAdUnit))) {
					Debug.Log ("requesting banner with AdUnit: " + bannerAdUnit);
					MoPub.createBanner (bannerAdUnit, MoPubAdPosition.BottomCenter);
					_requestedBannerAdUnits.Enqueue (bannerAdUnit);
				}

				GUI.enabled = _adUnitToLoadedMapping[bannerAdUnit];
				if (GUILayout.Button ("Destroy")) {
					MoPub.destroyBanner (bannerAdUnit);
					_adUnitToLoadedMapping[bannerAdUnit] = false;
					_bannerAdUnitToShownMapping[bannerAdUnit] = false;
				}

				GUI.enabled = _adUnitToLoadedMapping[bannerAdUnit] && !_bannerAdUnitToShownMapping[bannerAdUnit];
				if (GUILayout.Button ("Show")) {
					MoPub.showBanner (bannerAdUnit, true);
					_bannerAdUnitToShownMapping[bannerAdUnit] = true;
				}

				GUI.enabled = _adUnitToLoadedMapping[bannerAdUnit] && _bannerAdUnitToShownMapping[bannerAdUnit];
				if (GUILayout.Button ("Hide")) {
					MoPub.showBanner (bannerAdUnit, false);
					_bannerAdUnitToShownMapping[bannerAdUnit] = false;
				}
				GUI.enabled = true;

				GUILayout.EndHorizontal ();
			}
		} else {
			GUILayout.Label ("No banner AdUnits for " + _network, _smallerFont, null);
		}
	}


	private void CreateInterstitialsSection () {
		GUILayout.Space (_sectionMarginSize);
		GUILayout.Label ("Interstitials");
		if (!IsAdUnitArrayNullOrEmpty (_interstitialAdUnits)) {
			foreach (string interstitialAdUnit in _interstitialAdUnits) {
				GUILayout.BeginHorizontal ();

				GUI.enabled = !_adUnitToLoadedMapping[interstitialAdUnit];
				if (GUILayout.Button (CreateRequestButtonLabel (interstitialAdUnit))) {
					Debug.Log ("requesting interstitial with AdUnit: " + interstitialAdUnit);
					MoPub.requestInterstitialAd (interstitialAdUnit);
				}

				GUI.enabled = _adUnitToLoadedMapping[interstitialAdUnit];
				if (GUILayout.Button ("Show")) {
					MoPub.showInterstitialAd (interstitialAdUnit);
				}
				GUI.enabled = true;

				GUILayout.EndHorizontal ();
			}
		} else {
			GUILayout.Label ("No interstitial AdUnits for " + _network, _smallerFont, null);
		}
	}


	private void CreateRewardedVideosSection () {
		GUILayout.Space (_sectionMarginSize);
		GUILayout.Label ("Rewarded Videos");
		if (!IsAdUnitArrayNullOrEmpty (_rewardedVideoAdUnits)) {
			CreateCustomDataField ("rvCustomDataField", ref _rvCustomData);
			foreach (string rewardedVideoAdUnit in _rewardedVideoAdUnits) {
				GUILayout.BeginHorizontal ();

				GUI.enabled = !_adUnitToLoadedMapping[rewardedVideoAdUnit];
				if (GUILayout.Button (CreateRequestButtonLabel (rewardedVideoAdUnit))) {
					Debug.Log ("requesting rewarded video with AdUnit: " +
						rewardedVideoAdUnit);
					MoPub.requestRewardedVideo (rewardedVideoAdUnit,
						null,
						"rewarded, video, mopub",
						37.7833,
						122.4167,
						"customer101");
				}

				GUI.enabled = _adUnitToLoadedMapping[rewardedVideoAdUnit];
				if (GUILayout.Button ("Show")) {
					MoPub.showRewardedVideo (rewardedVideoAdUnit, GetCustomData (_rvCustomData));
				}
				GUI.enabled = true;

				GUILayout.EndHorizontal ();

				#if !UNITY_EDITOR
				// Display rewards if there's a rewarded video loaded and there are multiple rewards available
				if (MoPub.hasRewardedVideo (rewardedVideoAdUnit) &&
					_adUnitToRewardsMapping.ContainsKey (rewardedVideoAdUnit) &&
					_adUnitToRewardsMapping[rewardedVideoAdUnit].Count > 1) {

					GUILayout.BeginVertical ();
					GUILayout.Space (_sectionMarginSize);
					GUILayout.Label ("Select a reward:");

					foreach (MoPubReward reward in _adUnitToRewardsMapping[rewardedVideoAdUnit]) {
						if (GUILayout.Button (reward.ToString ())) {
							MoPub.selectReward (rewardedVideoAdUnit, reward);
						}
					}

					GUILayout.Space (_sectionMarginSize);
					GUILayout.EndVertical ();
				}
				#endif
			}
		} else {
			GUILayout.Label ("No rewarded video AdUnits for " + _network, _smallerFont, null);
		}
	}


	private void CreateRewardedRichMediaSection ()
	{
		GUILayout.Space (_sectionMarginSize);
		GUILayout.Label ("Rewarded Rich Media");
		if (!IsAdUnitArrayNullOrEmpty (_rewardedRichMediaAdUnits)) {
			CreateCustomDataField ("rrmCustomDataField", ref _rrmCustomData);
			foreach (string rewardedRichMediaAdUnit in _rewardedRichMediaAdUnits) {
				GUILayout.BeginHorizontal ();

				GUI.enabled = !_adUnitToLoadedMapping[rewardedRichMediaAdUnit];
				if (GUILayout.Button (CreateRequestButtonLabel (rewardedRichMediaAdUnit))) {
					Debug.Log ("requesting rewarded rich media with AdUnit: " +
						rewardedRichMediaAdUnit);
					MoPub.requestRewardedVideo (rewardedRichMediaAdUnit,
						null,
						"rewarded, video, mopub",
						37.7833,
						122.4167,
						"customer101");
				}

				GUI.enabled = _adUnitToLoadedMapping[rewardedRichMediaAdUnit];
				if (GUILayout.Button ("Show")) {
					MoPub.showRewardedVideo (rewardedRichMediaAdUnit, GetCustomData (_rrmCustomData));
				}
				GUI.enabled = true;

				GUILayout.EndHorizontal ();

				#if !UNITY_EDITOR
				// Display rewards if there's a rewarded rich media ad loaded and there are multiple rewards available
				if (MoPub.hasRewardedVideo (rewardedRichMediaAdUnit) &&
					_adUnitToRewardsMapping.ContainsKey (rewardedRichMediaAdUnit) &&
					_adUnitToRewardsMapping[rewardedRichMediaAdUnit].Count > 1) {

					GUILayout.BeginVertical ();
					GUILayout.Space (_sectionMarginSize);
					GUILayout.Label ("Select a reward:");

					foreach (MoPubReward reward in _adUnitToRewardsMapping[rewardedRichMediaAdUnit]) {
						if (GUILayout.Button (reward.ToString ())) {
							MoPub.selectReward (rewardedRichMediaAdUnit, reward);
						}
					}

					GUILayout.Space (_sectionMarginSize);
					GUILayout.EndVertical ();
				}
				#endif
			}
		} else {
			GUILayout.Label ("No rewarded rich media AdUnits for " + _network, _smallerFont, null);
		}
	}


	private void CreateCustomDataField (string fieldName, ref string customDataValue)
	{
		GUI.SetNextControlName (fieldName);
		customDataValue = GUILayout.TextField (customDataValue, new GUILayoutOption[] { GUILayout.MinWidth(200) });
		if (UnityEngine.Event.current.type == EventType.Repaint) {
			if (GUI.GetNameOfFocusedControl () == fieldName && customDataValue == _customDataDefaultText) {
				// Clear default text when focused
				customDataValue = "";
			} else if (GUI.GetNameOfFocusedControl () != fieldName && customDataValue == "") {
				// Restore default text when unfocused and empty
				customDataValue = _customDataDefaultText;
			}
		}
	}


	private string GetCustomData (string customDataFieldValue)
	{
		return customDataFieldValue != _customDataDefaultText ? customDataFieldValue : null;
	}


	private void CreateActionsSection ()
	{
		GUILayout.Space (_sectionMarginSize);
		GUILayout.Label ("Actions");
		if (GUILayout.Button ("Report App Open")) {
			MoPub.reportApplicationOpen ();
		}
		if (GUILayout.Button ("Enable Location Support")) {
			MoPub.enableLocationSupport (true);
		}
	}


	private string CreateRequestButtonLabel (string adUnit) {
		return "Request " + adUnit.Substring (0, 10) + "...";
	}
}
