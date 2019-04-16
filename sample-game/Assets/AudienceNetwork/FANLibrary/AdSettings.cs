//#define UNITY_ANDROID
//#define UNITY_IOS
using UnityEngine;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AOT;

namespace AudienceNetwork
{
    public class AdSettings
    {

        public static void AddTestDevice (string deviceID)
        {
            AdSettingsBridge.Instance.addTestDevice (deviceID);
        }

        public static void SetUrlPrefix (string urlPrefix)
        {
            AdSettingsBridge.Instance.setUrlPrefix (urlPrefix);
        }
    }

    internal class AdLogger
    {

        private enum AdLogLevel
        {
            None,
            Notification,
            Error,
            Warning,
            Log,
            Debug,
            Verbose
        }
        ;

        private static AdLogLevel logLevel = AdLogLevel.Log;
        private static readonly string logPrefix = "Audience Network Unity ";

        internal static void Log (string message)
        {
            AdLogLevel level = AdLogLevel.Log;
            if (AdLogger.logLevel >= level) {
                Debug.Log (AdLogger.logPrefix + AdLogger.levelAsString (level) + message);
            }
        }

        internal static void LogWarning (string message)
        {
            AdLogLevel level = AdLogLevel.Warning;
            if (AdLogger.logLevel >= level) {
                Debug.LogWarning (AdLogger.logPrefix + AdLogger.levelAsString (level) + message);
            }
        }

        internal static void LogError (string message)
        {
            AdLogLevel level = AdLogLevel.Error;
            if (AdLogger.logLevel >= level) {
                Debug.LogError (AdLogger.logPrefix + AdLogger.levelAsString (level) + message);
            }
        }

        // NSLog(@"[FBAudienceNetworkLog/%@:%d%@] %@", fileName, lineNumber, [FBAdLogger levelAsString:level], body);

        private static string levelAsString (AdLogLevel logLevel)
        {
            switch (logLevel) {
            case AdLogLevel.Notification:
                {
                    return "";
                }
            case AdLogLevel.Error:
                {
                    return "<error>: ";
                }
            case AdLogLevel.Warning:
                {
                    return "<warn>: ";
                }
            case AdLogLevel.Log:
                {
                    return "<log>: ";
                }
            case AdLogLevel.Debug:
                {
                    return "<debug>: ";
                }
            case AdLogLevel.Verbose:
                {
                    return "<verbose>: ";
                }
            }
            return "";

        }
    }

    internal interface IAdSettingsBridge
    {
        void addTestDevice (string deviceID);

        void setUrlPrefix (string urlPrefix);
    }

    internal class AdSettingsBridge : IAdSettingsBridge
    {

        public static readonly IAdSettingsBridge Instance;

        internal AdSettingsBridge ()
        {
        }

        static AdSettingsBridge ()
        {
            Instance = AdSettingsBridge.createInstance ();
        }

        private static IAdSettingsBridge createInstance ()
        {
            if (Application.platform != RuntimePlatform.OSXEditor) {
                #if UNITY_IOS
                return new AdSettingsBridgeIOS ();
                #elif UNITY_ANDROID
                return new AdSettingsBridgeAndroid ();
                #else
                return new AdSettingsBridge ();
                #endif
            } else {
                return new AdSettingsBridge ();
            }
        }

        public virtual void addTestDevice (string deviceID)
        {
        }

        public virtual void setUrlPrefix (string urlPrefix)
        {
        }
    }

    #if UNITY_ANDROID
    internal class AdSettingsBridgeAndroid : AdSettingsBridge {

        public override void addTestDevice (string deviceID)
        {
            AndroidJavaClass adSettings = this.getAdSettingsObject ();
            adSettings.CallStatic ("addTestDevice", deviceID);
        }

        public override void setUrlPrefix (string urlPrefix)
        {
            AndroidJavaClass adSettings = this.getAdSettingsObject ();
            adSettings.CallStatic ("setUrlPrefix", urlPrefix);
        }

        private AndroidJavaClass getAdSettingsObject ()
        {
            return new AndroidJavaClass("com.facebook.ads.AdSettings");
        }

    }
    #endif

    #if UNITY_IOS
    internal class AdSettingsBridgeIOS : AdSettingsBridge {

        [DllImport ("__Internal")]
        private static extern void FBAdSettingsBridgeAddTestDevice (string deviceID);

        [DllImport ("__Internal")]
        private static extern void FBAdSettingsBridgeSetURLPrefix (string urlPrefix);

        public override void addTestDevice (string deviceID)
        {
            FBAdSettingsBridgeAddTestDevice (deviceID);
        }

        public override void setUrlPrefix (string urlPrefix)
        {
            FBAdSettingsBridgeSetURLPrefix (urlPrefix);
        }

    }
    #endif
}
