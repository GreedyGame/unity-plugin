//#define UNITY_ANDROID
//#define UNITY_IOS
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AOT;
using AudienceNetwork.Utility;

namespace AudienceNetwork
{
    public delegate void FBNativeAdBridgeCallback ();

    public delegate void FBNativeAdBridgeErrorCallback (string error);

    internal delegate void FBNativeAdBridgeExternalCallback (int uniqueId);

    internal delegate void FBNativeAdBridgeErrorExternalCallback (int uniqueId,string error);

    public sealed class NativeAd : IDisposable
    {

        private int uniqueId;
        private bool isLoaded;
        private int minViewabilityPercentage;
        internal const float MIN_ALPHA = 0.9f;
        internal const int MAX_ROTATION = 45;
        internal const int CHECK_VIEWABILITY_INTERVAL = 1;
        private NativeAdHandler handler;

        public string PlacementId { get; private set; }

        public string Title { get; private set; }

        public string Subtitle { get; private set; }

        public string Body { get; private set; }

        public string CallToAction { get; private set; }

        public string SocialContext { get; private set; }

        public string IconImageURL { get; private set; }

        public string CoverImageURL { get; private set; }

        public string AdChoicesImageURL { get; private set; }

        public Sprite IconImage { get; private set; }

        public Sprite CoverImage { get; private set; }

        public Sprite AdChoicesImage { get; private set; }

        public string AdChoicesText { get; private set; }

        public string AdChoicesLinkURL { get; private set; }

        public FBNativeAdBridgeCallback NativeAdDidLoad {
            internal get {
                return this.nativeAdDidLoad;
            }
            set {
                this.nativeAdDidLoad = value;
                NativeAdBridge.Instance.OnLoad (uniqueId, nativeAdDidLoad);
            }
        }

        public FBNativeAdBridgeCallback NativeAdWillLogImpression {
            internal get {
                return this.nativeAdWillLogImpression;
            }
            set {
                this.nativeAdWillLogImpression = value;
                NativeAdBridge.Instance.OnImpression (uniqueId, nativeAdWillLogImpression);
            }
        }

        public FBNativeAdBridgeErrorCallback NativeAdDidFailWithError {
            internal get {
                return this.nativeAdDidFailWithError;
            }
            set {
                this.nativeAdDidFailWithError = value;
                NativeAdBridge.Instance.OnError (uniqueId, nativeAdDidFailWithError);
            }
        }

        public FBNativeAdBridgeCallback NativeAdDidClick {
            internal get {
                return this.nativeAdDidClick;
            }
            set {
                this.nativeAdDidClick = value;
                NativeAdBridge.Instance.OnClick (uniqueId, nativeAdDidClick);
            }
        }

        public FBNativeAdBridgeCallback NativeAdDidFinishHandlingClick {
            internal get {
                return this.nativeAdDidFinishHandlingClick;
            }
            set {
                this.nativeAdDidFinishHandlingClick = value;
                NativeAdBridge.Instance.OnFinishedClick (uniqueId, nativeAdDidFinishHandlingClick);
            }
        }

        private FBNativeAdBridgeCallback nativeAdDidLoad;
        private FBNativeAdBridgeCallback nativeAdWillLogImpression;
        private FBNativeAdBridgeErrorCallback nativeAdDidFailWithError;
        private FBNativeAdBridgeCallback nativeAdDidClick;
        private FBNativeAdBridgeCallback nativeAdDidFinishHandlingClick;

        public NativeAd (string placementId)
        {
            this.PlacementId = placementId;

            uniqueId = NativeAdBridge.Instance.Create (placementId, this);

            NativeAdBridge.Instance.OnLoad (uniqueId, NativeAdDidLoad);
            NativeAdBridge.Instance.OnImpression (uniqueId, NativeAdWillLogImpression);
            NativeAdBridge.Instance.OnClick (uniqueId, NativeAdDidClick);
            NativeAdBridge.Instance.OnError (uniqueId, NativeAdDidFailWithError);
            NativeAdBridge.Instance.OnFinishedClick (uniqueId, NativeAdDidFinishHandlingClick);
        }

        ~NativeAd ()
        {
            Dispose (false);
        }

        public void Dispose ()
        {
            Dispose (true);
            GC.SuppressFinalize (this);
        }

        private void Dispose (Boolean iAmBeingCalledFromDisposeAndNotFinalize)
        {
            if (this.handler) {
                this.handler.stopImpressionValidation();
                this.handler.removeFromParent ();
            }
            Debug.Log ("Native Ad Disposed.");
            NativeAdBridge.Instance.Release (uniqueId);
        }

        public override string ToString ()
        {
            return string.Format (
                "[NativeAd: " +
                "PlacementId={0}, " +
                "Title={1}, " +
                "Subtitle={2}, " +
                "Body={3}, " +
                "CallToAction={4}, " +
                "SocialContext={5}, " +
                "IconImageURL={6}, " +
                "CoverImageURL={7}, " +
                "IconImage={8}, " +
                "CoverImage={9}, " +
                "NativeAdDidLoad={10}, " +
                "NativeAdWillLogImpression={11}, " +
                "NativeAdDidFailWithError={12}, " +
                "NativeAdDidClick={13}, " +
                "NativeAdDidFinishHandlingClick={14}]",
                PlacementId,
                Title,
                Subtitle,
                Body,
                CallToAction,
                SocialContext,
                IconImageURL,
                CoverImageURL,
                IconImage,
                CoverImage,
                NativeAdDidLoad,
                NativeAdWillLogImpression,
                NativeAdDidFailWithError,
                NativeAdDidClick,
                NativeAdDidFinishHandlingClick);
        }

        private static TextureFormat imageFormat ()
        {
            return TextureFormat.RGBA32;
        }

        public IEnumerator LoadIconImage (string url)
        {
            Texture2D texture = new Texture2D (4, 4, NativeAd.imageFormat (), false);
            WWW www = new WWW (url);
            yield return www;
            www.LoadImageIntoTexture (texture);
            if (texture) {
                this.IconImage = Sprite.Create (texture, new Rect (0, 0, texture.width, texture.height), new Vector2 (0.5f, 0.5f));
            }
        }

        public IEnumerator LoadCoverImage (string url)
        {
            Texture2D texture = new Texture2D (4, 4, NativeAd.imageFormat (), false);
            WWW www = new WWW (url);
            yield return www;
            www.LoadImageIntoTexture (texture);

            if (texture) {
                this.CoverImage = Sprite.Create (texture, new Rect (0, 0, texture.width, texture.height), new Vector2 (0.5f, 0.5f));
            }
        }

        public IEnumerator LoadAdChoicesImage (string url)
        {
            Texture2D texture = new Texture2D (4, 4, NativeAd.imageFormat (), false);
            WWW www = new WWW (url);
            yield return www;
            www.LoadImageIntoTexture (texture);

            if (texture) {
                this.AdChoicesImage = Sprite.Create (texture, new Rect (0, 0, texture.width, texture.height), new Vector2 (0.5f, 0.5f));
            }
        }

        /* Signals the native ad to load data from the server */

        public void LoadAd ()
        {
            NativeAdBridge.Instance.Load (this.uniqueId);
        }

        public bool IsValid ()
        {
            return (this.isLoaded && NativeAdBridge.Instance.IsValid (this.uniqueId));
        }

        private void RegisterGameObjectForManualImpression (GameObject gameObject)
        {
            this.createHandler (gameObject);
        }

        public void RegisterGameObjectForImpression (GameObject gameObject,
                                                     Button[] clickableButtons)
        {
            this.RegisterGameObjectForImpression (gameObject, clickableButtons, Camera.main);
        }

        public void RegisterGameObjectForImpression (GameObject gameObject,
                                                     Button[] clickableButtons,
                                                     Camera camera)
        {
            // Register click handler
            foreach (Button button in clickableButtons) {
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener (delegate() {
                    AdLogger.Log ("Native ad with unique id " + this.uniqueId + " clicked!");
                    this.ExternalClick ();
                });
            }

            // Release the current handler and register for new handler
            // Whether or not the currentl handler has finished impression validation,
            // start the impression validation for the new handler.
            if (this.handler) {
                this.handler.stopImpressionValidation ();
                this.handler.removeFromParent ();
                this.createHandler (camera, gameObject);
                this.handler.startImpressionValidation ();
            } else {
                this.createHandler (camera, gameObject);
            }
        }

        private void createHandler (GameObject gameObject)
        {
            this.createHandler (null, gameObject);
        }

        private void createHandler (Camera camera,
                                    GameObject gameObject)
        {
            this.handler = gameObject.AddComponent<NativeAdHandler> ();
            this.handler.camera = camera;
            this.handler.minAlpha = NativeAd.MIN_ALPHA;
            this.handler.maxRotation = NativeAd.MAX_ROTATION;
            this.handler.checkViewabilityInterval = NativeAd.CHECK_VIEWABILITY_INTERVAL;
            this.handler.validationCallback = (delegate(bool success) {
                Debug.Log("Native ad viewability check for unique id " + this.uniqueId + " returned success? " + success);
                if (success) {
                    AdLogger.Log ("Native ad with unique id " + this.uniqueId + " registered impression!");
                    this.ExternalLogImpression ();
                    this.handler.stopImpressionValidation();
                }
            });
        }

        private void ManualLogImpression ()
        {
            NativeAdBridge.Instance.ManualLogImpression (this.uniqueId);
        }

        private void ManualClick ()
        {
            NativeAdBridge.Instance.ManualLogClick (this.uniqueId);
        }

        internal void ExternalLogImpression ()
        {
            NativeAdBridge.Instance.ExternalLogImpression (this.uniqueId);
        }

        internal void ExternalClick ()
        {
            NativeAdBridge.Instance.ExternalLogClick (this.uniqueId);
        }

        internal void loadAdFromData ()
        {
            if (this.handler == null) {
                throw new InvalidOperationException ("Native ad was loaded before it was registered. " +
                    "Ensure RegisterGameObjectForManualImpression () or RegisterGameObjectForImpression () are called.");
            }
            int uniqueId = this.uniqueId;
            this.Title = NativeAdBridge.Instance.GetTitle (uniqueId);
            this.Subtitle = NativeAdBridge.Instance.GetSubtitle (uniqueId);
            this.Body = NativeAdBridge.Instance.GetBody (uniqueId);
            this.CallToAction = NativeAdBridge.Instance.GetCallToAction (uniqueId);
            this.SocialContext = NativeAdBridge.Instance.GetSocialContext (uniqueId);
            this.CoverImageURL = NativeAdBridge.Instance.GetCoverImageURL (uniqueId);
            this.IconImageURL = NativeAdBridge.Instance.GetIconImageURL (uniqueId);
            this.AdChoicesImageURL = NativeAdBridge.Instance.GetAdChoicesImageURL (uniqueId);
            this.AdChoicesText = NativeAdBridge.Instance.GetAdChoicesText (uniqueId);
            this.AdChoicesLinkURL = NativeAdBridge.Instance.GetAdChoicesLinkURL (uniqueId);
            this.isLoaded = true;
            this.minViewabilityPercentage = NativeAdBridge.Instance.GetMinViewabilityPercentage (uniqueId);
            this.handler.minViewabilityPercentage = this.minViewabilityPercentage;

            if (this.NativeAdDidLoad != null) {
                this.handler.executeOnMainThread (() => {
                    this.NativeAdDidLoad ();
                });
            }

            this.handler.executeOnMainThread (() => {
                this.handler.startImpressionValidation ();
            });
        }

        internal void executeOnMainThread (Action action)
        {
            if (this.handler) {
                this.handler.executeOnMainThread (action);
            }
        }

        public static implicit operator bool (NativeAd obj)
        {
            return !(object.ReferenceEquals (obj, null));
        }
    }

    internal interface INativeAdBridge
    {
        int Create (string placementId,
                    NativeAd nativeAd);

        int Load (int uniqueId);

        bool IsValid (int uniqueId);

        string GetTitle (int uniqueId);

        string GetSubtitle (int uniqueId);

        string GetBody (int uniqueId);

        string GetCallToAction (int uniqueId);

        string GetSocialContext (int uniqueId);

        string GetIconImageURL (int uniqueId);

        string GetCoverImageURL (int uniqueId);

        string GetAdChoicesImageURL (int uniqueId);

        string GetAdChoicesText (int uniqueId);

        string GetAdChoicesLinkURL (int uniqueId);

        int GetMinViewabilityPercentage (int uniqueId);

        void ManualLogImpression (int uniqueId);

        void ManualLogClick (int uniqueId);

        void ExternalLogImpression (int uniqueId);

        void ExternalLogClick (int uniqueId);

        void Release (int uniqueId);

        void OnLoad (int uniqueId,
                     FBNativeAdBridgeCallback callback);

        void OnImpression (int uniqueId,
                           FBNativeAdBridgeCallback callback);

        void OnClick (int uniqueId,
                      FBNativeAdBridgeCallback callback);

        void OnError (int uniqueId,
                      FBNativeAdBridgeErrorCallback callback);

        void OnFinishedClick (int uniqueId,
                              FBNativeAdBridgeCallback callback);
    }

    internal class NativeAdBridge : INativeAdBridge
    {

        /* Interface to native implementation */

        internal static readonly string source =
            "AudienceNetworkUnityBridge " + AudienceNetwork.SdkVersion.Build + " (Unity " + Application.unityVersion + ")";
        public static readonly INativeAdBridge Instance;
        private FBNativeAdBridgeCallback onImpressionCallback;
        private FBNativeAdBridgeCallback onClickCallback;
        private List<NativeAd> nativeAds = new List<NativeAd> ();

        internal NativeAdBridge ()
        {
        }

        static NativeAdBridge ()
        {
            Instance = NativeAdBridge.createInstance ();
        }

        private static INativeAdBridge createInstance ()
        {
            if (Application.platform != RuntimePlatform.OSXEditor) {
                #if UNITY_IOS
                return new NativeAdBridgeIOS ();
                #elif UNITY_ANDROID
                return new NativeAdBridgeAndroid ();
                #else
                return new NativeAdBridge ();
                #endif
            } else {
                return new NativeAdBridge ();
            }

        }

        public virtual int Create (string placementId,
                                   NativeAd nativeAd)
        {
            nativeAds.Add (nativeAd);
            return nativeAds.Count - 1;
        }

        public virtual int Load (int uniqueId)
        {
            NativeAd nativeAd = this.nativeAds [uniqueId];
            nativeAd.loadAdFromData ();
            return uniqueId;
        }

        public virtual bool IsValid (int uniqueId)
        {
            return true;
        }

        public virtual string GetTitle (int uniqueId)
        {
            return "Facebook Test Ad";
        }

        public virtual string GetSubtitle (int uniqueId)
        {
            return "An ad for Facebook";
        }

        public virtual string GetBody (int uniqueId)
        {
            return "Your ad integration works. Woohoo!";
        }

        public virtual string GetCallToAction (int uniqueId)
        {
            return "Install Now";
        }

        public virtual string GetSocialContext (int uniqueId)
        {
            return "Available on the App Store";
        }

        public virtual string GetIconImageURL (int uniqueId)
        {
            return "https://www.facebook.com/images/ad_network/audience_network_icon.png";
        }

        public virtual string GetCoverImageURL (int uniqueId)
        {
            return "https://www.facebook.com/images/ad_network/audience_network_test_cover.png";
        }

        public virtual string GetAdChoicesImageURL (int uniqueId)
        {
                return "https://www.facebook.com/images/ad_network/ad_choices.png";
        }

        public virtual string GetAdChoicesText (int uniqueId)
        {
            return "AdChoices";
        }

        public virtual string GetAdChoicesLinkURL (int uniqueId)
        {
            return "https://m.facebook.com/ads/ad_choices/";
        }

        public virtual int GetMinViewabilityPercentage (int uniqueId)
        {
            return 1;
        }

        public virtual void ManualLogImpression (int uniqueId)
        {
            var callback = this.onImpressionCallback;
            if (callback != null) {
                callback ();
            }
        }

        public virtual void ManualLogClick (int uniqueId)
        {
            var callback = this.onClickCallback;
            if (callback != null) {
                callback ();
            }
        }

        public virtual void ExternalLogImpression (int uniqueId)
        {
            var callback = this.onImpressionCallback;
            if (callback != null) {
                callback ();
            }
        }

        public virtual void ExternalLogClick (int uniqueId)
        {
            var callback = this.onClickCallback;
            if (callback != null) {
                callback ();
            }
        }

        public virtual void Release (int uniqueId)
        {
        }

        public virtual void OnLoad (int uniqueId,
                                    FBNativeAdBridgeCallback callback)
        {
        }

        public virtual void OnImpression (int uniqueId,
                                          FBNativeAdBridgeCallback callback)
        {
            this.onImpressionCallback = callback;
        }

        public virtual void OnClick (int uniqueId,
                                     FBNativeAdBridgeCallback callback)
        {
            this.onClickCallback = callback;
        }

        public virtual void OnError (int uniqueId,
                                     FBNativeAdBridgeErrorCallback callback)
        {
        }

        public virtual void OnFinishedClick (int uniqueId,
                                             FBNativeAdBridgeCallback callback)
        {
        }

    }

    #if UNITY_ANDROID
    internal class NativeAdBridgeAndroid : NativeAdBridge {

        private static Dictionary<int, NativeAdContainer> nativeAds = new Dictionary<int, NativeAdContainer>();
        private static int lastKey = 0;

        private AndroidJavaObject nativeAdForNativeAdId (int uniqueId)
        {
            NativeAdContainer nativeAdContainer = null;
            bool success = NativeAdBridgeAndroid.nativeAds.TryGetValue (uniqueId, out nativeAdContainer);
            if (success) {
                return nativeAdContainer.bridgedNativeAd;
            } else {
                return null;
            }
        }

        private string getStringForNativeAdId (int uniqueId,
                                               string method)
        {
            AndroidJavaObject nativeAd = this.nativeAdForNativeAdId (uniqueId);
            if (nativeAd != null) {
                return nativeAd.Call<string> (method);
            } else {
                return null;
            }
        }

        private string getImageURLForNativeAdId (int uniqueId,
                                                 string method)
        {
            AndroidJavaObject nativeAd = this.nativeAdForNativeAdId (uniqueId);
            if (nativeAd != null) {
                AndroidJavaObject image = nativeAd.Call<AndroidJavaObject> (method);
                if (image != null) {
                    return image.Call<string> ("getUrl");
                }
            }
            return null;
        }

        public override int Create (string placementId,
                                    NativeAd nativeAd)
        {
            AdUtility.prepare ();
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject context = currentActivity.Call<AndroidJavaObject>("getApplicationContext");

            AndroidJavaObject bridgedNativeAd = new AndroidJavaObject("com.facebook.ads.NativeAd", context, placementId);

            NativeAdBridgeListenerProxy proxy = new NativeAdBridgeListenerProxy (nativeAd, bridgedNativeAd);
            bridgedNativeAd.Call ("setAdListener", proxy);

            NativeAdContainer nativeAdContainer = new NativeAdContainer (nativeAd);
            nativeAdContainer.bridgedNativeAd = bridgedNativeAd;
            nativeAdContainer.listenerProxy = proxy;

            int key = NativeAdBridgeAndroid.lastKey;
            NativeAdBridgeAndroid.nativeAds.Add(key, nativeAdContainer);
            NativeAdBridgeAndroid.lastKey++;
            return key;
        }

        public override int Load (int uniqueId)
        {
            AdUtility.prepare ();
            AndroidJavaObject nativeAd = this.nativeAdForNativeAdId (uniqueId);
            if (nativeAd != null) {
                nativeAd.Call ("registerExternalLogReceiver", NativeAdBridge.source);
                nativeAd.Call ("loadAd");
            }
            return uniqueId;
        }

        public override bool IsValid (int uniqueId)
        {
            AndroidJavaObject nativeAd = this.nativeAdForNativeAdId (uniqueId);
            if (nativeAd != null) {
                return nativeAd.Call<bool> ("isAdLoaded");
            } else {
                return false;
            }
        }

        public override string GetTitle (int uniqueId)
        {
            return this.getStringForNativeAdId (uniqueId, "getAdTitle");
        }

        public override string GetSubtitle (int uniqueId)
        {
            return this.getStringForNativeAdId (uniqueId, "getAdSubtitle");
        }

        public override string GetBody (int uniqueId)
        {
            return this.getStringForNativeAdId (uniqueId, "getAdBody");
        }

        public override string GetCallToAction (int uniqueId)
        {
            return this.getStringForNativeAdId (uniqueId, "getAdCallToAction");
        }

        public override string GetSocialContext (int uniqueId)
        {
            return this.getStringForNativeAdId (uniqueId, "getAdSocialContext");
        }

        public override string GetIconImageURL (int uniqueId)
        {
            return this.getImageURLForNativeAdId (uniqueId, "getAdIcon");
        }

        public override string GetCoverImageURL (int uniqueId)
        {
            return this.getImageURLForNativeAdId (uniqueId, "getAdCoverImage");
        }

        public override string GetAdChoicesImageURL (int uniqueId)
        {
            return this.getImageURLForNativeAdId (uniqueId, "getAdChoicesIcon");
        }

        public override string GetAdChoicesText (int uniqueId)
        {
            return this.getStringForNativeAdId (uniqueId, "getAdChoicesText");
        }

        public override string GetAdChoicesLinkURL (int uniqueId)
        {
            return this.getStringForNativeAdId (uniqueId, "getAdChoicesLinkUrl");
        }

        public override int GetMinViewabilityPercentage (int uniqueId)
        {
            AndroidJavaObject nativeAd = this.nativeAdForNativeAdId (uniqueId);
            if (nativeAd != null) {
                return nativeAd.Call<int> ("getMinViewabilityPercentage");
            }
            return 1;
        }

        private string getId (int uniqueId)
        {
            AndroidJavaObject nativeAd = this.nativeAdForNativeAdId (uniqueId);
            if (nativeAd != null) {
                return nativeAd.Call<string> ("getId");
            } else {
                return null;
            }
        }

        public override void ManualLogImpression (int uniqueId)
        {
            AndroidJavaObject nativeAd = this.nativeAdForNativeAdId (uniqueId);
            if (nativeAd != null) {
                this.sendIntentToBroadcastManager(uniqueId, "com.facebook.ads.native.impression");
            }
        }

        public override void ManualLogClick (int uniqueId)
        {
            AndroidJavaObject nativeAd = this.nativeAdForNativeAdId (uniqueId);
            if (nativeAd != null) {
                this.sendIntentToBroadcastManager(uniqueId, "com.facebook.ads.native.click");
            }
        }

        public override void ExternalLogImpression (int uniqueId)
        {
            AndroidJavaObject nativeAd = this.nativeAdForNativeAdId (uniqueId);
            if (nativeAd != null) {
                nativeAd.Call ("logExternalImpression");
            }
        }

        public override void ExternalLogClick (int uniqueId)
        {
            AndroidJavaObject nativeAd = this.nativeAdForNativeAdId (uniqueId);
            if (nativeAd != null) {
                nativeAd.Call ("logExternalClick", NativeAdBridge.source);
            }
        }

        private bool sendIntentToBroadcastManager (int uniqueId,
                                                   string intent)
        {
            if (intent != null) {
                AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

                AndroidJavaObject clickIntent = new AndroidJavaObject ("android.content.Intent", intent + ":" + getId(uniqueId));
                AndroidJavaClass localBroadcastManagerClass = new AndroidJavaClass ("android.support.v4.content.LocalBroadcastManager");
                AndroidJavaObject localBroadcastManager =
                    localBroadcastManagerClass.CallStatic<AndroidJavaObject> ("getInstance", currentActivity);
                return localBroadcastManager.Call<bool> ("sendBroadcast", clickIntent);
            }
            return false;
        }

        public override void Release (int uniqueId)
        {
            NativeAdBridgeAndroid.nativeAds.Remove (uniqueId);
        }

        public override void OnLoad (int uniqueId, FBNativeAdBridgeCallback callback) {}
        public override void OnImpression (int uniqueId, FBNativeAdBridgeCallback callback) {}
        public override void OnClick (int uniqueId, FBNativeAdBridgeCallback callback) {}
        public override void OnError (int uniqueId, FBNativeAdBridgeErrorCallback callback) {}
        public override void OnFinishedClick (int uniqueId, FBNativeAdBridgeCallback callback) {}

    }

    #endif

    #if UNITY_IOS
    internal class NativeAdBridgeIOS : NativeAdBridge {

        private static Dictionary<int, NativeAdContainer> nativeAds = new Dictionary<int, NativeAdContainer>();

        private static NativeAdContainer nativeAdContainerForNativeAdId (int uniqueId)
        {
            NativeAdContainer nativeAd = null;
            bool success = NativeAdBridgeIOS.nativeAds.TryGetValue (uniqueId, out nativeAd);
            if (success) {
                return nativeAd;
            } else {
                return null;
            }
        }

        [DllImport ("__Internal")]
        private static extern int FBNativeAdBridgeCreate (string placementId);

        [DllImport ("__Internal")]
        private static extern int FBNativeAdBridgeLoad (int uniqueId);

        [DllImport ("__Internal")]
        private static extern bool FBNativeAdBridgeIsValid (int uniqueId);

        [DllImport ("__Internal")]
        private static extern string FBNativeAdBridgeGetTitle (int uniqueId);

        [DllImport ("__Internal")]
        private static extern string FBNativeAdBridgeGetSubtitle (int uniqueId);

        [DllImport ("__Internal")]
        private static extern string FBNativeAdBridgeGetBody (int uniqueId);

        [DllImport ("__Internal")]
        private static extern string FBNativeAdBridgeGetCallToAction (int uniqueId);

        [DllImport ("__Internal")]
        private static extern string FBNativeAdBridgeGetSocialContext (int uniqueId);

        [DllImport ("__Internal")]
        private static extern string FBNativeAdBridgeGetIconImageURL (int uniqueId);

        [DllImport ("__Internal")]
        private static extern string FBNativeAdBridgeGetCoverImageURL (int uniqueId);

        [DllImport ("__Internal")]
        private static extern string FBNativeAdBridgeGetAdChoicesImageURL (int uniqueId);

        [DllImport ("__Internal")]
        private static extern string FBNativeAdBridgeGetAdChoicesText (int uniqueId);

        [DllImport ("__Internal")]
        private static extern string FBNativeAdBridgeGetAdChoicesLinkURL (int uniqueId);

        [DllImport ("__Internal")]
        private static extern int FBNativeAdBridgeGetMinViewabilityPercentage (int uniqueId);

        [DllImport ("__Internal")]
        private static extern void FBNativeAdBridgeManualLogImpression (int uniqueId);

        [DllImport ("__Internal")]
        private static extern void FBNativeAdBridgeManualClick (int uniqueId);

        [DllImport ("__Internal")]
        private static extern void FBNativeAdBridgeExternalLogImpression (int uniqueId,
                                                                          string source);

        [DllImport ("__Internal")]
        private static extern void FBNativeAdBridgeExternalClick (int uniqueId,
                                                                  string source);

        [DllImport ("__Internal")]
        private static extern void FBNativeAdBridgeRelease (int uniqueId);

        [DllImport ("__Internal")]
        private static extern void FBNativeAdBridgeOnLoad(int uniqueId,
                                                          FBNativeAdBridgeExternalCallback callback);

        [DllImport ("__Internal")]
        private static extern void FBNativeAdBridgeOnImpression(int uniqueId,
                                                                FBNativeAdBridgeExternalCallback callback);

        [DllImport ("__Internal")]
        private static extern void FBNativeAdBridgeOnClick(int uniqueId,
                                                           FBNativeAdBridgeExternalCallback callback);

        [DllImport ("__Internal")]
        private static extern void FBNativeAdBridgeOnError(int uniqueId,
                                                           FBNativeAdBridgeErrorExternalCallback callback);

        [DllImport ("__Internal")]
        private static extern void FBNativeAdBridgeOnFinishedClick(int uniqueId,
                                                                   FBNativeAdBridgeExternalCallback callback);

        public override int Create (string placementId, NativeAd nativeAd)
        {
            int uniqueId = NativeAdBridgeIOS.FBNativeAdBridgeCreate (placementId);
            NativeAdBridgeIOS.nativeAds.Add (uniqueId, new NativeAdContainer(nativeAd));
            NativeAdBridgeIOS.FBNativeAdBridgeOnLoad (uniqueId, nativeAdDidLoadBridgeCallback);
            NativeAdBridgeIOS.FBNativeAdBridgeOnImpression (uniqueId, nativeAdWillLogImpressionBridgeCallback);
            NativeAdBridgeIOS.FBNativeAdBridgeOnClick (uniqueId, nativeAdDidClickBridgeCallback);
            NativeAdBridgeIOS.FBNativeAdBridgeOnError (uniqueId, nativeAdDidFailWithErrorBridgeCallback);
            NativeAdBridgeIOS.FBNativeAdBridgeOnFinishedClick (uniqueId, nativeAdDidFinishHandlingClickBridgeCallback);

            return uniqueId;
        }

        public override int Load (int uniqueId)
        {
            return NativeAdBridgeIOS.FBNativeAdBridgeLoad (uniqueId);
        }

        public override bool IsValid (int uniqueId)
        {
            return NativeAdBridgeIOS.FBNativeAdBridgeIsValid (uniqueId);
        }

        public override string GetTitle (int uniqueId)
        {
            return NativeAdBridgeIOS.FBNativeAdBridgeGetTitle (uniqueId);
        }

        public override string GetSubtitle (int uniqueId)
        {
            return NativeAdBridgeIOS.FBNativeAdBridgeGetSubtitle (uniqueId);
        }

        public override string GetBody (int uniqueId)
        {
            return NativeAdBridgeIOS.FBNativeAdBridgeGetBody (uniqueId);
        }

        public override string GetCallToAction (int uniqueId)
        {
            return NativeAdBridgeIOS.FBNativeAdBridgeGetCallToAction (uniqueId);
        }

        public override string GetSocialContext (int uniqueId)
        {
            return NativeAdBridgeIOS.FBNativeAdBridgeGetSocialContext (uniqueId);
        }

        public override string GetIconImageURL (int uniqueId)
        {
            return NativeAdBridgeIOS.FBNativeAdBridgeGetIconImageURL (uniqueId);
        }

        public override string GetCoverImageURL (int uniqueId)
        {
            return NativeAdBridgeIOS.FBNativeAdBridgeGetCoverImageURL (uniqueId);
        }

        public override string GetAdChoicesImageURL (int uniqueId)
        {
            return NativeAdBridgeIOS.FBNativeAdBridgeGetAdChoicesImageURL (uniqueId);
        }

        public override string GetAdChoicesText (int uniqueId)
        {
            return NativeAdBridgeIOS.FBNativeAdBridgeGetAdChoicesText (uniqueId);
        }

        public override string GetAdChoicesLinkURL (int uniqueId)
        {
            return NativeAdBridgeIOS.FBNativeAdBridgeGetAdChoicesLinkURL (uniqueId);
        }

        public override int GetMinViewabilityPercentage (int uniqueId)
        {
            return NativeAdBridgeIOS.FBNativeAdBridgeGetMinViewabilityPercentage(uniqueId);
        }

        public override void ManualLogImpression (int uniqueId)
        {
            NativeAdBridgeIOS.FBNativeAdBridgeManualLogImpression (uniqueId);
        }

        public override void ManualLogClick (int uniqueId)
        {
            NativeAdBridgeIOS.FBNativeAdBridgeManualClick (uniqueId);
        }

        public override void ExternalLogImpression (int uniqueId)
        {
            NativeAdBridgeIOS.FBNativeAdBridgeExternalLogImpression (uniqueId, NativeAdBridge.source);
        }

        public override void ExternalLogClick (int uniqueId)
        {
            NativeAdBridgeIOS.FBNativeAdBridgeExternalClick (uniqueId, NativeAdBridge.source);
        }

        public override void Release (int uniqueId)
        {
            NativeAdBridgeIOS.nativeAds.Remove (uniqueId);
            NativeAdBridgeIOS.FBNativeAdBridgeRelease (uniqueId);
        }

        // Sets up internal managed callbacks

        public override void OnLoad (int uniqueId,
                                     FBNativeAdBridgeCallback callback)
        {
            NativeAdContainer container = NativeAdBridgeIOS.nativeAdContainerForNativeAdId (uniqueId);
            if (container) {
                container.onLoad = (delegate() {
                    container.nativeAd.loadAdFromData ();
                });
            }
        }

        public override void OnImpression (int uniqueId,
                                           FBNativeAdBridgeCallback callback)
        {
            NativeAdContainer container = NativeAdBridgeIOS.nativeAdContainerForNativeAdId (uniqueId);
            if (container) {
                container.onImpression = callback;
            }
        }

        public override void OnClick (int uniqueId,
                                      FBNativeAdBridgeCallback callback)
        {
            NativeAdContainer container = NativeAdBridgeIOS.nativeAdContainerForNativeAdId (uniqueId);
            if (container) {
                container.onClick = callback;
            }
        }

        public override void OnError (int uniqueId,
                                      FBNativeAdBridgeErrorCallback callback)
        {
            NativeAdContainer container = NativeAdBridgeIOS.nativeAdContainerForNativeAdId (uniqueId);
            if (container) {
                container.onError = callback;
            }
        }

        public override void OnFinishedClick (int uniqueId,
                                              FBNativeAdBridgeCallback callback)
        {
            NativeAdContainer container = NativeAdBridgeIOS.nativeAdContainerForNativeAdId (uniqueId);
            if (container) {
                container.onFinishedClick = callback;
            }
        }

        // External unmanaged callbacks (must be static)

        [MonoPInvokeCallback (typeof (FBNativeAdBridgeExternalCallback))]
        private static void nativeAdDidLoadBridgeCallback(int uniqueId)
        {
            NativeAdContainer container = NativeAdBridgeIOS.nativeAdContainerForNativeAdId (uniqueId);
            if (container && container.onLoad != null) {
                container.onLoad ();
            }
        }

        [MonoPInvokeCallback (typeof (FBNativeAdBridgeExternalCallback))]
        private static void nativeAdWillLogImpressionBridgeCallback(int uniqueId)
        {
            NativeAdContainer container = NativeAdBridgeIOS.nativeAdContainerForNativeAdId (uniqueId);
            if (container && container.onImpression != null) {
                container.onImpression ();
            }
        }

        [MonoPInvokeCallback (typeof (FBNativeAdBridgeErrorExternalCallback))]
        private static void nativeAdDidFailWithErrorBridgeCallback(int uniqueId, string error)
        {
            NativeAdContainer container = NativeAdBridgeIOS.nativeAdContainerForNativeAdId (uniqueId);
            if (container && container.onError != null) {
                container.onError (error);
            }
        }

        [MonoPInvokeCallback (typeof (FBNativeAdBridgeExternalCallback))]
        private static void nativeAdDidClickBridgeCallback(int uniqueId)
        {
            NativeAdContainer container = NativeAdBridgeIOS.nativeAdContainerForNativeAdId (uniqueId);
            if (container && container.onClick != null) {
                container.onClick ();
            }
        }

        [MonoPInvokeCallback (typeof (FBNativeAdBridgeExternalCallback))]
        private static void nativeAdDidFinishHandlingClickBridgeCallback(int uniqueId)
        {
            NativeAdContainer container = NativeAdBridgeIOS.nativeAdContainerForNativeAdId (uniqueId);
            if (container && container.onFinishedClick != null) {
                container.onFinishedClick ();
            }
        }

    }
    #endif

    internal class NativeAdContainer
    {
        internal NativeAd nativeAd { get; set; }

        // iOS
        internal FBNativeAdBridgeCallback onLoad { get; set; }

        internal FBNativeAdBridgeCallback onImpression { get; set; }

        internal FBNativeAdBridgeCallback onClick { get; set; }

        internal FBNativeAdBridgeErrorCallback onError { get; set; }

        internal FBNativeAdBridgeCallback onFinishedClick { get; set; }

        // Android
        #if UNITY_ANDROID
        internal AndroidJavaProxy listenerProxy;
        internal AndroidJavaObject bridgedNativeAd;
        #endif

        internal NativeAdContainer (NativeAd nativeAd)
        {
            this.nativeAd = nativeAd;
        }

        public static implicit operator bool (NativeAdContainer obj)
        {
            return !(object.ReferenceEquals (obj, null));
        }
    }

    #if UNITY_ANDROID
    internal class NativeAdBridgeListenerProxy : AndroidJavaProxy
    {
        private NativeAd nativeAd;
        #pragma warning disable 0414
        private AndroidJavaObject bridgedNativeAd;
        #pragma warning restore 0414

        public NativeAdBridgeListenerProxy(NativeAd nativeAd, AndroidJavaObject bridgedNativeAd)
            : base("com.facebook.ads.AdListener")
        {
            this.nativeAd = nativeAd;
            this.bridgedNativeAd = bridgedNativeAd;
        }

        void onError (AndroidJavaObject ad, AndroidJavaObject error)
        {
            string errorMessage = error.Call<string> ("getErrorMessage");
            this.nativeAd.executeOnMainThread(() => {
                if (nativeAd.NativeAdDidFailWithError != null) {
                    nativeAd.NativeAdDidFailWithError (errorMessage);
                }
            });
        }

        void onAdLoaded (AndroidJavaObject ad)
        {
            this.nativeAd.executeOnMainThread(() => {
                nativeAd.loadAdFromData ();
            });
        }

        void onAdClicked (AndroidJavaObject ad)
        {
            this.nativeAd.executeOnMainThread(() => {
                if (nativeAd.NativeAdDidClick != null) {
                    nativeAd.NativeAdDidClick ();
                }
            });
        }

        void onLoggingImpression (AndroidJavaObject ad)
        {
            this.nativeAd.executeOnMainThread(() => {
                if (nativeAd.NativeAdWillLogImpression != null) {
                    nativeAd.NativeAdWillLogImpression ();
                }
            });
        }

    }

    #endif


}
