//#define UNITY_ANDROID
//#define UNITY_IOS
using UnityEngine;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AOT;

namespace AudienceNetwork.Utility
{
    public class AdUtility
    {

        internal static double width ()
        {
            return AdUtilityBridge.Instance.width ();
        }

        internal static double height ()
        {
            return AdUtilityBridge.Instance.height ();
        }

        internal static double convert (double deviceSize)
        {
            return AdUtilityBridge.Instance.convert (deviceSize);
        }

        internal static void prepare ()
        {
            AdUtilityBridge.Instance.prepare ();
        }

    }

    internal interface IAdUtilityBridge
    {
        double deviceWidth ();

        double deviceHeight ();

        double width ();

        double height ();

        double convert (double deviceSize);

        void prepare ();
    }

    internal class AdUtilityBridge : IAdUtilityBridge
    {

        public static readonly IAdUtilityBridge Instance;

        internal AdUtilityBridge ()
        {
        }

        static AdUtilityBridge ()
        {
            Instance = AdUtilityBridge.createInstance ();
        }

        private static IAdUtilityBridge createInstance ()
        {
            if (Application.platform != RuntimePlatform.OSXEditor) {
                #if UNITY_IOS
                return new AdUtilityBridgeIOS ();
                #elif UNITY_ANDROID
                return new AdUtilityBridgeAndroid ();
                #else
                return new AdUtilityBridge ();
                #endif
            } else {
                return new AdUtilityBridge ();
            }
        }

        public virtual double deviceWidth ()
        {
            return 2208;
        }

        public virtual double deviceHeight ()
        {
            return 1242;
        }

        public virtual double width ()
        {
            return 1104;
        }

        public virtual double height ()
        {
            return 621;
        }

        public virtual double convert (double deviceSize)
        {
            return 2;
        }

        public virtual void prepare ()
        {
        }
    }

    #if UNITY_ANDROID
    internal class AdUtilityBridgeAndroid : AdUtilityBridge {

        private T getPropertyOfDisplayMetrics<T> (string property)
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
            AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject> ("currentActivity");
            AndroidJavaObject context = activity.Call<AndroidJavaObject> ("getApplicationContext");
            AndroidJavaObject resources = context.Call<AndroidJavaObject> ("getResources");
            AndroidJavaObject displayMetrics = resources.Call<AndroidJavaObject> ("getDisplayMetrics");
            return displayMetrics.Get<T> (property);
        }

        private double density ()
        {
            return this.getPropertyOfDisplayMetrics<float> ("density");
        }

        public override double deviceWidth ()
        {
            return this.getPropertyOfDisplayMetrics<int> ("widthPixels");
        }

        public override double deviceHeight ()
        {
            return this.getPropertyOfDisplayMetrics<int> ("heightPixels");
        }

        public override double width ()
        {
            return this.convert (this.deviceWidth ());
        }

        public override double height ()
        {
            return this.convert (this.deviceHeight ());
        }

        public override double convert (double deviceSize)
        {
            return (deviceSize / this.density ());
        }

        public override void prepare ()
        {
            #if UNITY_ANDROID
            try {
                AndroidJavaClass displayAdControllerClass = new AndroidJavaClass("com.facebook.ads.internal.DisplayAdController");
                displayAdControllerClass.CallStatic("setMainThreadForced", true);
            } catch (Exception) {

            }

            try {
                AndroidJavaClass looperClass = new AndroidJavaClass("android.os.Looper");
                looperClass.CallStatic ("prepare");
            } catch (Exception) {

            }
            #endif
        }
    }
    #endif

    #if UNITY_IOS
    internal class AdUtilityBridgeIOS : AdUtilityBridge {

        [DllImport ("__Internal")]
        private static extern double FBAdUtilityBridgeGetDeviceWidth ();

        [DllImport ("__Internal")]
        private static extern double FBAdUtilityBridgeGetDeviceHeight ();

        [DllImport ("__Internal")]
        private static extern double FBAdUtilityBridgeGetWidth ();

        [DllImport ("__Internal")]
        private static extern double FBAdUtilityBridgeGetHeight ();

        [DllImport ("__Internal")]
        private static extern double FBAdUtilityBridgeConvertFromDeviceSize (double deviceSize);

        public override double deviceWidth ()
        {
            return FBAdUtilityBridgeGetDeviceWidth ();
        }

        public override double deviceHeight ()
        {
            return FBAdUtilityBridgeGetDeviceHeight ();
        }

        public override double width ()
        {
            return FBAdUtilityBridgeGetWidth ();
        }

        public override double height ()
        {
            return FBAdUtilityBridgeGetHeight ();
        }

        public override double convert (double deviceSize)
        {
            return FBAdUtilityBridgeConvertFromDeviceSize (deviceSize);
        }

    }
    #endif
}
