//#define UNITY_ANDROID
//#define UNITY_IOS
using UnityEngine;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AOT;
using AudienceNetwork.Utility;

namespace AudienceNetwork
{
    public delegate void FBInterstitialAdBridgeCallback ();

    public delegate void FBInterstitialAdBridgeErrorCallback (string error);

    internal delegate void FBInterstitialAdBridgeExternalCallback (int uniqueId);

    internal delegate void FBInterstitialAdBridgeErrorExternalCallback (int uniqueId,string error);

    public sealed class InterstitialAd : IDisposable
    {

        private int uniqueId;
        private bool isLoaded;
        private AdHandler handler;

        public string PlacementId { get; private set; }

        public FBInterstitialAdBridgeCallback InterstitialAdDidLoad {
            internal get {
                return this.interstitialAdDidLoad;
            }
            set {
                this.interstitialAdDidLoad = value;
                InterstitialAdBridge.Instance.OnLoad (uniqueId, interstitialAdDidLoad);
            }
        }

        public FBInterstitialAdBridgeCallback InterstitialAdWillLogImpression {
            internal get {
                return this.interstitialAdWillLogImpression;
            }
            set {
                this.interstitialAdWillLogImpression = value;
                InterstitialAdBridge.Instance.OnImpression (uniqueId, interstitialAdWillLogImpression);
            }
        }

        public FBInterstitialAdBridgeErrorCallback InterstitialAdDidFailWithError {
            internal get {
                return this.interstitialAdDidFailWithError;
            }
            set {
                this.interstitialAdDidFailWithError = value;
                InterstitialAdBridge.Instance.OnError (uniqueId, interstitialAdDidFailWithError);
            }
        }

        public FBInterstitialAdBridgeCallback InterstitialAdDidClick {
            internal get {
                return this.interstitialAdDidClick;
            }
            set {
                this.interstitialAdDidClick = value;
                InterstitialAdBridge.Instance.OnClick (uniqueId, interstitialAdDidClick);
            }
        }

        public FBInterstitialAdBridgeCallback InterstitialAdWillClose {
            internal get {
                return this.interstitialAdWillClose;
            }
            set {
                this.interstitialAdWillClose = value;
                InterstitialAdBridge.Instance.OnWillClose (uniqueId, interstitialAdWillClose);
            }
        }

        public FBInterstitialAdBridgeCallback InterstitialAdDidClose {
            internal get {
                return this.interstitialAdDidClose;
            }
            set {
                this.interstitialAdDidClose = value;
                InterstitialAdBridge.Instance.OnDidClose (uniqueId, interstitialAdDidClose);
            }
        }

        public FBInterstitialAdBridgeCallback interstitialAdDidLoad;
        public FBInterstitialAdBridgeCallback interstitialAdWillLogImpression;
        public FBInterstitialAdBridgeErrorCallback interstitialAdDidFailWithError;
        public FBInterstitialAdBridgeCallback interstitialAdDidClick;
        public FBInterstitialAdBridgeCallback interstitialAdWillClose;
        public FBInterstitialAdBridgeCallback interstitialAdDidClose;

        public InterstitialAd (string placementId)
        {
            this.PlacementId = placementId;

            if (Application.platform != RuntimePlatform.OSXEditor) {
                uniqueId = InterstitialAdBridge.Instance.Create (placementId, this);

                InterstitialAdBridge.Instance.OnLoad (uniqueId, InterstitialAdDidLoad);
                InterstitialAdBridge.Instance.OnImpression (uniqueId, InterstitialAdWillLogImpression);
                InterstitialAdBridge.Instance.OnClick (uniqueId, InterstitialAdDidClick);
                InterstitialAdBridge.Instance.OnError (uniqueId, InterstitialAdDidFailWithError);
                InterstitialAdBridge.Instance.OnWillClose (uniqueId, InterstitialAdWillClose);
                InterstitialAdBridge.Instance.OnDidClose (uniqueId, InterstitialAdDidClose);
            }
        }

        ~InterstitialAd ()
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
                this.handler.removeFromParent ();
            }
            Debug.Log ("Interstitial Ad Disposed.");
            InterstitialAdBridge.Instance.Release (uniqueId);
        }

        public override string ToString ()
        {
            return string.Format (
                "[InterstitialAd: " +
                "PlacementId={0}, " +
                "InterstitialAdDidLoad={1}, " +
                "InterstitialAdWillLogImpression={2}, " +
                "InterstitialAdDidFailWithError={3}, " +
                "InterstitialAdDidClick={4}, " +
                "InterstitialAdWillClose={5}, " +
                "InterstitialAdDidClose={6}]",
                PlacementId,
                InterstitialAdDidLoad,
                InterstitialAdWillLogImpression,
                InterstitialAdDidFailWithError,
                InterstitialAdDidClick,
                InterstitialAdWillClose,
                InterstitialAdDidClose);
        }

        public void Register (GameObject gameObject)
        {
            this.handler = gameObject.AddComponent<AdHandler> ();
        }

        public void LoadAd ()
        {
            if (Application.platform != RuntimePlatform.OSXEditor) {
                InterstitialAdBridge.Instance.Load (this.uniqueId);
            } else {
                this.InterstitialAdDidLoad ();
            }
        }

        public bool IsValid ()
        {
            if (Application.platform != RuntimePlatform.OSXEditor) {
                return (this.isLoaded && InterstitialAdBridge.Instance.IsValid (this.uniqueId));
            } else {
                return true;
            }
        }

        internal void loadAdFromData ()
        {
            this.isLoaded = true;
        }

        public bool Show ()
        {
            return InterstitialAdBridge.Instance.Show (uniqueId);
            ;
        }

        internal void executeOnMainThread (Action action)
        {
            if (this.handler) {
                this.handler.executeOnMainThread (action);
            }
        }

        public static implicit operator bool (InterstitialAd obj)
        {
            return !(object.ReferenceEquals (obj, null));
        }
    }

    internal interface IInterstitialAdBridge
    {
        int Create (string placementId,
                    InterstitialAd interstitialAd);

        int Load (int uniqueId);

        bool IsValid (int uniqueId);

        bool Show (int uniqueId);

        void Release (int uniqueId);

        void OnLoad (int uniqueId,
                     FBInterstitialAdBridgeCallback callback);

        void OnImpression (int uniqueId,
                           FBInterstitialAdBridgeCallback callback);

        void OnClick (int uniqueId,
                      FBInterstitialAdBridgeCallback callback);

        void OnError (int uniqueId,
                      FBInterstitialAdBridgeErrorCallback callback);

        void OnWillClose (int uniqueId,
                          FBInterstitialAdBridgeCallback callback);

        void OnDidClose (int uniqueId,
                         FBInterstitialAdBridgeCallback callback);
    }

    internal class InterstitialAdBridge : IInterstitialAdBridge
    {

        /* Interface to Interstitial implementation */

        public static readonly IInterstitialAdBridge Instance;

        internal InterstitialAdBridge ()
        {
        }

        static InterstitialAdBridge ()
        {
            Instance = InterstitialAdBridge.createInstance ();
        }

        private static IInterstitialAdBridge createInstance ()
        {
            if (Application.platform != RuntimePlatform.OSXEditor) {
                #if UNITY_IOS
                return new InterstitialAdBridgeIOS ();
                #elif UNITY_ANDROID
                return new InterstitialAdBridgeAndroid ();
                #else
                return new InterstitialAdBridge ();
                #endif
            } else {
                return new InterstitialAdBridge ();
            }

        }

        public virtual int Create (string placementId,
                                   InterstitialAd InterstitialAd)
        {
            return 123;
        }

        public virtual int Load (int uniqueId)
        {
            return 123;
        }

        public virtual bool IsValid (int uniqueId)
        {
            return true;
        }

        public virtual bool Show (int uniqueId)
        {
            return true;
        }

        public virtual void ManualLogImpression (int uniqueId)
        {
        }

        public virtual void ManualLogClick (int uniqueId)
        {
        }

        public virtual void Release (int uniqueId)
        {
        }

        public virtual void OnLoad (int uniqueId,
                                    FBInterstitialAdBridgeCallback callback)
        {
        }

        public virtual void OnImpression (int uniqueId,
                                          FBInterstitialAdBridgeCallback callback)
        {
        }

        public virtual void OnClick (int uniqueId,
                                     FBInterstitialAdBridgeCallback callback)
        {
        }

        public virtual void OnError (int uniqueId,
                                     FBInterstitialAdBridgeErrorCallback callback)
        {
        }

        public virtual void OnWillClose (int uniqueId,
                                         FBInterstitialAdBridgeCallback callback)
        {
        }

        public virtual void OnDidClose (int uniqueId,
                                        FBInterstitialAdBridgeCallback callback)
        {
        }

    }

    #if UNITY_ANDROID
    internal class InterstitialAdBridgeAndroid : InterstitialAdBridge {

        private static Dictionary<int, InterstitialAdContainer> interstitialAds = new Dictionary<int, InterstitialAdContainer>();
        private static int lastKey = 0;

        private AndroidJavaObject interstitialAdForuniqueId (int uniqueId)
        {
            InterstitialAdContainer interstitialAdContainer = null;
            bool success = InterstitialAdBridgeAndroid.interstitialAds.TryGetValue (uniqueId, out interstitialAdContainer);
            if (success) {
                return interstitialAdContainer.bridgedInterstitialAd;
            } else {
                return null;
            }
        }

        private string getStringForuniqueId (int uniqueId,
                                             string method)
        {
            AndroidJavaObject interstitialAd = this.interstitialAdForuniqueId (uniqueId);
            if (interstitialAd != null) {
                return interstitialAd.Call<string> (method);
            } else {
                return null;
            }
        }

        private string getImageURLForuniqueId (int uniqueId,
                                               string method)
        {
            AndroidJavaObject interstitialAd = this.interstitialAdForuniqueId (uniqueId);
            if (interstitialAd != null) {
                AndroidJavaObject image = interstitialAd.Call<AndroidJavaObject> (method);
                if (image != null) {
                    return image.Call<string> ("getUrl");
                }
            }
            return null;
        }

        public override int Create (string placementId,
                                    InterstitialAd interstitialAd)
        {
            AdUtility.prepare ();
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject context = currentActivity.Call<AndroidJavaObject>("getApplicationContext");

            AndroidJavaObject bridgedInterstitialAd = new AndroidJavaObject("com.facebook.ads.InterstitialAd", context, placementId);

            InterstitialAdBridgeListenerProxy proxy = new InterstitialAdBridgeListenerProxy (interstitialAd, bridgedInterstitialAd);
            bridgedInterstitialAd.Call ("setAdListener", proxy);

            InterstitialAdContainer interstitialAdContainer = new InterstitialAdContainer (interstitialAd);
            interstitialAdContainer.bridgedInterstitialAd = bridgedInterstitialAd;
            interstitialAdContainer.listenerProxy = proxy;

            int key = InterstitialAdBridgeAndroid.lastKey;
            InterstitialAdBridgeAndroid.interstitialAds.Add(key, interstitialAdContainer);
            InterstitialAdBridgeAndroid.lastKey++;
            return key;
        }

        public override int Load (int uniqueId)
        {
            AdUtility.prepare ();
            AndroidJavaObject interstitialAd = this.interstitialAdForuniqueId (uniqueId);
            if (interstitialAd != null) {
                interstitialAd.Call("loadAd");
            }
            return uniqueId;
        }

        public override bool IsValid (int uniqueId)
        {
            AndroidJavaObject interstitialAd = this.interstitialAdForuniqueId (uniqueId);
            if (interstitialAd != null) {
                return interstitialAd.Call<bool> ("isAdLoaded");
            } else {
                return false;
            }
        }

        public override bool Show (int uniqueId)
        {
            AndroidJavaObject interstitialAd = this.interstitialAdForuniqueId (uniqueId);
            if (interstitialAd != null) {
                return interstitialAd.Call<bool> ("show");
            } else {
                return false;
            }
        }

        public override void Release (int uniqueId)
        {
            AndroidJavaObject interstitialAd = this.interstitialAdForuniqueId (uniqueId);
            if (interstitialAd != null) {
                interstitialAd.Call ("destroy");
            }
            InterstitialAdBridgeAndroid.interstitialAds.Remove (uniqueId);
        }

        public override void OnLoad (int uniqueId, FBInterstitialAdBridgeCallback callback) {}
        public override void OnImpression (int uniqueId, FBInterstitialAdBridgeCallback callback) {}
        public override void OnClick (int uniqueId, FBInterstitialAdBridgeCallback callback) {}
        public override void OnError (int uniqueId, FBInterstitialAdBridgeErrorCallback callback) {}
        public override void OnWillClose (int uniqueId, FBInterstitialAdBridgeCallback callback) {}
        public override void OnDidClose (int uniqueId, FBInterstitialAdBridgeCallback callback) {}

    }

    #endif

    #if UNITY_IOS
    internal class InterstitialAdBridgeIOS : InterstitialAdBridge {

        private static Dictionary<int, InterstitialAdContainer> interstitialAds = new Dictionary<int, InterstitialAdContainer>();

        private static InterstitialAdContainer interstitialAdContainerForuniqueId (int uniqueId)
        {
            InterstitialAdContainer interstitialAd = null;
            bool success = InterstitialAdBridgeIOS.interstitialAds.TryGetValue (uniqueId, out interstitialAd);
            if (success) {
                return interstitialAd;
            } else {
                return null;
            }
        }

        [DllImport ("__Internal")]
        private static extern int FBInterstitialAdBridgeCreate (string placementId);

        [DllImport ("__Internal")]
        private static extern int FBInterstitialAdBridgeLoad (int uniqueId);

        [DllImport ("__Internal")]
        private static extern bool FBInterstitialAdBridgeIsValid (int uniqueId);

        [DllImport ("__Internal")]
        private static extern bool FBInterstitialAdBridgeShow (int uniqueId);

        [DllImport ("__Internal")]
        private static extern void FBInterstitialAdBridgeRelease (int uniqueId);

        [DllImport ("__Internal")]
        private static extern void FBInterstitialAdBridgeOnLoad(int uniqueId,
                                                                FBInterstitialAdBridgeExternalCallback callback);

        [DllImport ("__Internal")]
        private static extern void FBInterstitialAdBridgeOnImpression(int uniqueId,
                                                                      FBInterstitialAdBridgeExternalCallback callback);

        [DllImport ("__Internal")]
        private static extern void FBInterstitialAdBridgeOnClick(int uniqueId,
                                                                 FBInterstitialAdBridgeExternalCallback callback);

        [DllImport ("__Internal")]
        private static extern void FBInterstitialAdBridgeOnError(int uniqueId,
                                                                 FBInterstitialAdBridgeErrorExternalCallback callback);

        [DllImport ("__Internal")]
        private static extern void FBInterstitialAdBridgeOnDidClose(int uniqueId,
                                                                    FBInterstitialAdBridgeExternalCallback callback);

        [DllImport ("__Internal")]
        private static extern void FBInterstitialAdBridgeOnWillClose(int uniqueId,
                                                                     FBInterstitialAdBridgeExternalCallback callback);

        public override int Create (string placementId,
                                    InterstitialAd interstitialAd)
        {
            int uniqueId = InterstitialAdBridgeIOS.FBInterstitialAdBridgeCreate (placementId);
            InterstitialAdBridgeIOS.interstitialAds.Add (uniqueId, new InterstitialAdContainer(interstitialAd));
            InterstitialAdBridgeIOS.FBInterstitialAdBridgeOnLoad (uniqueId, interstitialAdDidLoadBridgeCallback);
            InterstitialAdBridgeIOS.FBInterstitialAdBridgeOnImpression (uniqueId, interstitialAdWillLogImpressionBridgeCallback);
            InterstitialAdBridgeIOS.FBInterstitialAdBridgeOnClick (uniqueId, interstitialAdDidClickBridgeCallback);
            InterstitialAdBridgeIOS.FBInterstitialAdBridgeOnError (uniqueId, interstitialAdDidFailWithErrorBridgeCallback);
            InterstitialAdBridgeIOS.FBInterstitialAdBridgeOnDidClose (uniqueId, interstitialAdDidCloseBridgeCallback);
            InterstitialAdBridgeIOS.FBInterstitialAdBridgeOnWillClose (uniqueId, interstitialAdWillCloseBridgeCallback);

            return uniqueId;
        }

        public override int Load (int uniqueId)
        {
            return InterstitialAdBridgeIOS.FBInterstitialAdBridgeLoad (uniqueId);
        }

        public override bool IsValid (int uniqueId)
        {
            return InterstitialAdBridgeIOS.FBInterstitialAdBridgeIsValid (uniqueId);
        }

        public override bool Show (int uniqueId)
        {
            return InterstitialAdBridgeIOS.FBInterstitialAdBridgeShow (uniqueId);
        }

        public override void Release (int uniqueId)
        {
            InterstitialAdBridgeIOS.interstitialAds.Remove (uniqueId);
            InterstitialAdBridgeIOS.FBInterstitialAdBridgeRelease (uniqueId);
        }

        // Sets up internal managed callbacks

        public override void OnLoad (int uniqueId,
                                     FBInterstitialAdBridgeCallback callback)
        {
            InterstitialAdContainer container = InterstitialAdBridgeIOS.interstitialAdContainerForuniqueId (uniqueId);
            if (container) {
                container.onLoad = (delegate() {
                    container.interstitialAd.loadAdFromData ();
                    if (callback != null) {
                        callback ();
                    }
                });
            }
        }

        public override void OnImpression (int uniqueId,
                                           FBInterstitialAdBridgeCallback callback)
        {
            InterstitialAdContainer container = InterstitialAdBridgeIOS.interstitialAdContainerForuniqueId (uniqueId);
            if (container) {
                container.onImpression = callback;
            }
        }

        public override void OnClick (int uniqueId,
                                      FBInterstitialAdBridgeCallback callback)
        {
            InterstitialAdContainer container = InterstitialAdBridgeIOS.interstitialAdContainerForuniqueId (uniqueId);
            if (container) {
                container.onClick = callback;
            }
        }

        public override void OnError (int uniqueId,
                                      FBInterstitialAdBridgeErrorCallback callback)
        {
            InterstitialAdContainer container = InterstitialAdBridgeIOS.interstitialAdContainerForuniqueId (uniqueId);
            if (container) {
                container.onError = callback;
            }
        }

        public override void OnDidClose (int uniqueId,
                                         FBInterstitialAdBridgeCallback callback)
        {
            InterstitialAdContainer container = InterstitialAdBridgeIOS.interstitialAdContainerForuniqueId (uniqueId);
            if (container) {
                container.onDidClose = callback;
            }
        }

        public override void OnWillClose (int uniqueId,
                                          FBInterstitialAdBridgeCallback callback)
        {
            InterstitialAdContainer container = InterstitialAdBridgeIOS.interstitialAdContainerForuniqueId (uniqueId);
            if (container) {
                container.onWillClose = callback;
            }
        }

        // External unmanaged callbacks (must be static)

        [MonoPInvokeCallback (typeof (FBInterstitialAdBridgeExternalCallback))]
        private static void interstitialAdDidLoadBridgeCallback(int uniqueId)
        {
            InterstitialAdContainer container = InterstitialAdBridgeIOS.interstitialAdContainerForuniqueId (uniqueId);
            if (container && container.onLoad != null) {
                container.onLoad ();
            }
        }

        [MonoPInvokeCallback (typeof (FBInterstitialAdBridgeExternalCallback))]
        private static void interstitialAdWillLogImpressionBridgeCallback(int uniqueId)
        {
            InterstitialAdContainer container = InterstitialAdBridgeIOS.interstitialAdContainerForuniqueId (uniqueId);
            if (container && container.onImpression != null) {
                container.onImpression ();
            }
        }

        [MonoPInvokeCallback (typeof (FBInterstitialAdBridgeErrorExternalCallback))]
        private static void interstitialAdDidFailWithErrorBridgeCallback(int uniqueId, string error)
        {
            InterstitialAdContainer container = InterstitialAdBridgeIOS.interstitialAdContainerForuniqueId (uniqueId);
            if (container && container.onError != null) {
                container.onError (error);
            }
        }

        [MonoPInvokeCallback (typeof (FBInterstitialAdBridgeExternalCallback))]
        private static void interstitialAdDidClickBridgeCallback(int uniqueId)
        {
            InterstitialAdContainer container = InterstitialAdBridgeIOS.interstitialAdContainerForuniqueId (uniqueId);
            if (container && container.onClick != null) {
                container.onClick ();
            }
        }

        [MonoPInvokeCallback (typeof (FBInterstitialAdBridgeExternalCallback))]
        private static void interstitialAdDidCloseBridgeCallback(int uniqueId)
        {
            InterstitialAdContainer container = InterstitialAdBridgeIOS.interstitialAdContainerForuniqueId (uniqueId);
            if (container && container.onDidClose != null) {
                container.onDidClose ();
            }
        }

        [MonoPInvokeCallback (typeof (FBInterstitialAdBridgeExternalCallback))]
        private static void interstitialAdWillCloseBridgeCallback(int uniqueId)
        {
            InterstitialAdContainer container = InterstitialAdBridgeIOS.interstitialAdContainerForuniqueId (uniqueId);
            if (container && container.onWillClose != null) {
                container.onWillClose ();
            }
        }

    }
    #endif

    internal class InterstitialAdContainer
    {
        internal InterstitialAd interstitialAd { get; set; }

        // iOS
        internal FBInterstitialAdBridgeCallback onLoad { get; set; }

        internal FBInterstitialAdBridgeCallback onImpression { get; set; }

        internal FBInterstitialAdBridgeCallback onClick { get; set; }

        internal FBInterstitialAdBridgeErrorCallback onError { get; set; }

        internal FBInterstitialAdBridgeCallback onDidClose { get; set; }

        internal FBInterstitialAdBridgeCallback onWillClose { get; set; }

        // Android
        #if UNITY_ANDROID
        internal AndroidJavaProxy listenerProxy;
        internal AndroidJavaObject bridgedInterstitialAd;
        #endif

        internal InterstitialAdContainer (InterstitialAd interstitialAd)
        {
            this.interstitialAd = interstitialAd;
        }

        public override string ToString ()
        {
            return string.Format ("[InterstitialAdContainer: interstitialAd={0}, onLoad={1}]", interstitialAd, onLoad);
        }

        public static implicit operator bool (InterstitialAdContainer obj)
        {
            return !(object.ReferenceEquals (obj, null));
        }
    }

    #if UNITY_ANDROID
    internal class InterstitialAdBridgeListenerProxy : AndroidJavaProxy
    {
        private InterstitialAd interstitialAd;
        #pragma warning disable 0414
        private AndroidJavaObject bridgedInterstitialAd;
        #pragma warning restore 0414

        public InterstitialAdBridgeListenerProxy(InterstitialAd interstitialAd,
                                                 AndroidJavaObject bridgedInterstitialAd)
            : base("com.facebook.ads.InterstitialAdListener")
        {
            this.interstitialAd = interstitialAd;
            this.bridgedInterstitialAd = bridgedInterstitialAd;
        }

        void onError (AndroidJavaObject ad,
                      AndroidJavaObject error)
        {
            string errorMessage = error.Call<string> ("getErrorMessage");
            this.interstitialAd.executeOnMainThread(() => {
                if (interstitialAd.InterstitialAdDidFailWithError != null) {
                    interstitialAd.InterstitialAdDidFailWithError (errorMessage);
                }
            });
        }

        void onAdLoaded (AndroidJavaObject ad)
        {
            this.interstitialAd.executeOnMainThread(() => {
                if (interstitialAd.InterstitialAdDidLoad != null) {
                    interstitialAd.InterstitialAdDidLoad ();
                }
            });
        }

        void onAdClicked (AndroidJavaObject ad)
        {
            this.interstitialAd.executeOnMainThread(() => {
                if (interstitialAd.InterstitialAdDidClick != null) {
                    interstitialAd.InterstitialAdDidClick ();
                }
            });
        }

        void onInterstitialDisplayed (AndroidJavaObject ad)
        {
            this.interstitialAd.executeOnMainThread(() => {
                if (interstitialAd.InterstitialAdWillLogImpression != null) {
                    interstitialAd.InterstitialAdWillLogImpression ();
                }
            });
        }

        void onInterstitialDismissed (AndroidJavaObject ad)
        {
            this.interstitialAd.executeOnMainThread(() => {
                if (interstitialAd.InterstitialAdDidClose != null) {
                    interstitialAd.InterstitialAdDidClose ();
                }
            });
        }

        void onLoggingImpression (AndroidJavaObject ad)
        {
            this.interstitialAd.executeOnMainThread(() => {
                if (interstitialAd.InterstitialAdWillLogImpression != null) {
                    interstitialAd.InterstitialAdWillLogImpression ();
                }
            });
        }
    }

    #endif


}
