GreedyGame Android's SDK Reference
---------------------
 * [Introduction](#introduction)
 * [Requirements](#requirements)
 * [Integration](#integration)
 * [Documentation](#documentation)
    * [GreedyAdManager](#greedyadmanager)
    * [interface IAgentListener](#interface-iagentlistener)
 * [Android Setup](#android-setup) 
	 * [Proguard Settings](#proguard-settings)
 * [FAQ](#faq)

 
# Introduction
Before we get started with the detailed reference, let’s brush through the definitions of some important terms that we’ll see referenced regularly. [Here at greedygame.github.io] (http://greedygame.github.io/)

# Requirements
* Android API Version: 14
* Unity3d 4 or lastest

# Integration
If you have gone through the definitions of important keywords. To make the rest of the integration an absolute breeze for you, we’ve set up an integration wizard on your [publisher panel](http://publisher.greedygame.com).

Once you’ve logged in, on the top of your page and select **SDK Integration Wizard** and we’ll walk you through the integration from the comfort of your own publisher panel.

![PublisherPanel's top menu](http://greedygame.github.io/images/wizard.png "SDK Integration Wizard")


# Documentation
### GreedyAdManager
#### Class Overview
Singleton monobehaviour class encapsulating the overall GreedyGame ad flow and model.


#### Public Methods
##### `public static GreedyAdManager.Instance`
Create and return singleton instance of GreedyAdManager class.

##### `public void init(bool isDebug, bool isLazyLoad, IAgentListener agentListener)`
Lookup for an active campaign from the server.
* **isDebug** : make debug logs visible in logcat
* **isLazyLoad** : if true, nativeunits will refect branding as soon as campaign get downloaded
* **agentListener** : instance of IAgentListener's implemented class


##### `public string[] NativeUnitIds`
Return array of all [nativeunit](http://greedygame.github.io/#nativeunits)'s id used in the game

##### `public string CampaignPath`
Return path of folder, where assets of current campaign is stored.

##### `public void fetchFloatUnit(String unit_id) `
Fetch [floatunit](http://greedygame.github.io/#floatunits) and add view to current context.

##### `public void removeAllFloatUnits() `
Remove all fetched [floatunit](http://greedygame.github.io/#floatunits).

##### `public void showEngagementWindow(string unit_id) `
Open [engagement window](http://greedygame.github.io/#engagementwindow) attached with provided floatunit

----
### interface IAgentListener
#### Class Overview
It is used as a callback listener argument for GreedyGameAgent class

#### Public Methods
##### `void onAvailable()`
When a new campaign is available and ready to use for the next scene.

##### `void onUnavailable()`
When no campaign is available

##### `void onProgress(int progress)`
Gives progress of campaign being downloaded as an integer.

##### `void onPermissionsUnavailable(string[] permissions)`
Gives a list of permission unavailable or revoked by the user.

**Permissions that are checked**
```
Manifest.permission.ACCESS_COARSE_LOCATION
Manifest.permission.WRITE_EXTERNAL_STORAGE
Manifest.permission.READ_PHONE_STATE
```
**Interface Example**
```csharp
public class GreedyAgentListener : IAgentListener {

    public void onAvailable() {
        /**
         * TODO: New campaign is available and ready to use for the next scene.
         **/
    }

    public void onUnavailable() {
        /**
         * TODO: No campaign is available, proceed with normal follow of the game.
         **/
    }

    public void onProgress(int progress) {
        /**
         * TODO: Progress bar can be shown using progress value from 0 to 100.
         **/
    }

    public void onPermissionsUnavailable(string[] permissions) {
        /**
         * TODO: Prompt user to give required permission
         **/
        for(int i = 0; i < permissions.Length; i++) {
            string p = permissions[i];
            Debug.Log(String.Format("permission unavailable = {0}", p));
        }
    }
}
```

# Android Setup
1. [For users running a version of Unity earlier than 5.0] Navigate to Temp/StagingArea of your project directory and copy AndroidManifest.xml to Assets/Plugins/Android. Add the following <meta-data> tag to the AndroidManifest.xml file:
  
  ```xml
  <activity android:name="com.unity3d.player.UnityPlayerActivity" ...>
    ...
    <meta-data android:name="unityplayer.ForwardNativeEventsToDalvik" android:value="true" />
  </activity>
  ```
2. And set application *hardwareAccelerated=true* as
  ```xml
  <application 
     android:hardwareAccelerated="true"
  ....
  </application>
  
  ```


## Proguard Settings

If you are using Proguard add the following to your Proguard settings ! 
```
-keep class com.greedygame.android.** { *;}
-keepattributes JavascriptInterface
-keepclassmembers class * {
    @android.webkit.JavascriptInterface <methods>;
 }
```
 
# FAQ
For more help please see [FAQ](https://github.com/GreedyGame/unity-plugin/wiki/FAQs)
