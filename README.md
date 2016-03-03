GreedyGame Unity Integration Guide
===================

This is a complete guide for integrating GreedyGame plugin within your unity game. You can download *GreedyGame_vX.Y.Z.unitypackage* from [github release](https://github.com/GreedyGame/unity-plugin/releases).

## Setting up
* In panel.greedygame.com
   1. Make account on panel.greedygame.com
   2. Create game profile to get **GAME_PROFILE_ID**
* In UnityEngine
   1. Download the latest GreedyGame unity package from github release
   2. Import GreedyGame_vX.Y.Z.package into your unity project. Once you do that you’ll see an icon “GreedyGame” appear in the top menu.
   3. In top menu goto GreedyGame then AdUnitManager and use panel.greedygame.com's credential to login.
   4. In the Inspector window, use the **GAMEPROFILE_ID** assigned to you on the panel for the game you created and select LoadingLevel and save. 
  
	>**LoadingLevel** is the scene where the plugin will initiate. Only post that scene will you see the branded units.


---

## Declaration of Native Ads
![SharedAdUnit MonoBehaviour](https://raw.githubusercontent.com/GreedyGame/Unity-Sample/master/screen-shots/1_branded_game.png?raw=true "SharedAdUnit MonoBehaviour attached to Stockcar/Body_Complete" )

### 1. Select GameObject for branding
1. Select the GameObject from your scene which can be used for branding.

 > GameOjects should **not** be from the **LoadingLevel** as plugin initiates during this level. Only GameObject with renderer with 2D textures can be used.
2. From Inspector panel  add component **AdUnit** or **Shared AdUnit**
3. Now you should see a component added with a yellow colour help box.
4. Similarly repeat steps 2 and 3 for all game objects you want.
5. You can use AdUnit and Shared AdUnit to change [material](http://docs.unity3d.com/ScriptReference/Renderer-material.html) and [sharedMaterials](http://docs.unity3d.com/ScriptReference/Renderer-sharedMaterial.html).


> Preview: SharedAdUnit MonoBehaviour attached to Stockcar/Body_Complete

> ![SharedAdUnit MonoBehaviour](https://raw.githubusercontent.com/GreedyGame/Unity-Sample/master/screen-shots/2_attached_monobehaviour.png?raw=true "SharedAdUnit MonoBehaviour attached to Stockcar/Body_Complete" )
 1. SharedAdUnit Attached, yellow helpbox states it ready to build in unitlist
 2. 2D texture, will be used for branded assets, such as logo, product image etc.
 3. MeshRender will be used as renderer to blend branding image over object

### 2. Setting up with Server
1. Using TopMenu: GreedyGame > AdUnitManager
2. Click Save button to build list and save changes.
3. Check and make sure that all the native ad units you added in Part 1 are listed in this window as shown. 

> Preview: list of units to be used for branding.

> ![Refresh UnitList](https://raw.githubusercontent.com/GreedyGame/Unity-Sample/master/screen-shots/5_refresh_save.png?raw=true "list of units to be used for branding" )
 1. **GameProfileId**, game-id from panel.greedygame.com
 2. **LoadingLevel**, will be used for fetching and loading campaign assets
 3. **Save**, will upload images to server and create GlobalConfig objects at LoadingLevel

### 3. Manage campagin fetching and post LoadingLevel
1. After Step 2 open your LoadingLevel scene.
2. On any GameObject from LoadingLevel Scene attached script GreedyCampaignLoader.cs 


---
## Declaration of FloatAd-Unit

![SharedAdUnit MonoBehaviour](https://raw.githubusercontent.com/GreedyGame/Unity-Sample/master/screen-shots/7_float_ad.png?raw=true "SharedAdUnit MonoBehaviour attached to Stockcar/Body_Complete" )

**In panel.greedygame.com**

1. For creating a FloatAd-Unit go back to the panel
2. If you’ve already selected the game then open the tab “Ad Units” on the left side on the panel.
3. Create a new FloatAd-Unit in the panel by clicking on the button “Add Floating Unit”. 
4. Note down the **FloatAd_ID** number generated. ( this will be later used for calling the floatad)

**In UnityEngine**

5. Select the scene where you need the FloatAd-Unit to be displayed.
6. Wherever you want to fetch or hide the FloatAd-Unit,  just use the following function from `GreedyGame.Runtime.Common`
  * To show FloatAd-Unit
  
	```csharp
	GreedyAdManager.Instance.FetchAdHead (FloatAd_ID);
	```
  * To remove FloatAd-Unit
 
  	```csharp
  	GreedyAdManager.Instance.RemoveAllAdHead ();
  	```


---
## Android Setup
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

---
## Running on device

### Checking runtime unit list

1. Goto LoadingLevel scene and select **GreedyGameConfigPrefab**
2. Look for **GlobalConfig** component attached.
3. Validate value of **GlobalConfig** component, with values from *panel.greedygame.com*

  > ![GreedyGameConfigObject](https://raw.githubusercontent.com/GreedyGame/Unity-Sample/master/screen-shots/6_global_config.png?raw=true "Checking runtime unit list" )

4. During the ‘debug’ mode you’ll see debug branded objects. (Colored grid texture)
5. To disable ‘debug’, uncheck isDebug. 
6. Before building your final project setup a demo campaign with help from your GreedyGame POC or contact integration@greedygame.com to check how a campaign looks within your game.

---
## Advance Customization Documentation

### GreedyAdManager
#### Class Overview
Contains high-level classes encapsulating the overall GreedyGame ad flow and model.

#### GreedyGame's headers 
```csharp
using GreedyGame.Runtime.Common;
using GreedyGame.Platform;
```
---

#### Public Singleton Constructors
`GreedyAdManager.Instance`

*Example*
```csharp
private GreedyAdManager ggAdManager = null;
void Awake(){
//Initialization as singleton
  ggAdManager = GreedyAdManager.Instance;
}
```
---

##### CALLBACK METHOD IN WRAPPER : unAvailablePermissions(ArrayList<String> permissions)
* This method needs to be used only if your game is targetting SDK version 23 or
  higher. This callback gives a list of permissions that are not available at runtime and is invoked after GreedyGameAgent initialization.

   IMPORTANT : Unity takes care of dangerous permissions on its own at runtime. Use this callback function only if you are    not using Unity Runtime Permission facility.
   NB : Only performs check for 4 dangerous permissions that are required by GreedyGameSDK. 

  Permissions that are checked : 

   * Manifest.permission.ACCESS_COARSE_LOCATION
   * Manifest.permission.WRITE_EXTERNAL_STORAGE
   * Manifest.permission.GET_ACCOUNTS
   * Manifest.permission.READ_PHONE_STATE

   NB : The above strings itself are returned in the argument if they are not available.

#### Init
`init(String GameId, String[] AdUnits, Boolean isDebug, Boolean isLazyLoad, OnGreedyEvent)`

Lookup for new native campaign from server.
* GameId - Unique game profile id from panel.greedygame.com
* AdUnits - Array register unit id. eg. Unit-XYZ
* isDebug- To build debug app for testing
* isLazyLoad- In case of true, it will show branded assets as soon as downloaded 
* OnGreedyEvent - Callback function for **RuntimeEvent** as follow:
  - CAMPAIGN_NOT_LOADED
  - CAMPAIGN_LOADED
  - CAMPAIGN_DOWNLOADED

*Example*
```csharp
void Start() {
  if (isSupported) {
    GlobalConfig[] ggLoaders = Resources.FindObjectsOfTypeAll<GlobalConfig> ();
    if(ggLoaders != null && ggLoaders.Length != 1){
      isSupported = false;
      Debug.LogError("None or multuple occurrence of GlobalConfig object found!");
      return;
    }
    GlobalConfig ggConfig = ggLoaders [0];
    ggAdManager.init (ggConfig.GameId, ggConfig.AdUnits.ToArray (), ggConfig.isDebug, ggConfig.isLazyLoad, OnGreedyEvent);
  }
}
```
---

#### OnGreedyEvent
`void OnGreedyEvent(RuntimeEvent greedy_events)`

Callback function for **RuntimeEvent**

*Example*
```csharp
void OnGreedyEvent(RuntimeEvent greedy_events){
  if (greedy_events == RuntimeEvent.CAMPAIGN_LOADED || 
      greedy_events == RuntimeEvent.CAMPAIGN_NOT_LOADED) {
  //Goto play scene if server reponse is recevied
    Application.LoadLevel (PostLevel);
  }
}
```
---

### Video Tutorial
Check out the following youtube link for a video tutorial which contains the entire walkthrough for GreedyGame Integration in Unity ! (https://www.youtube.com/watch?v=L8Lq5UIbd68)

### For more help please see [FAQ] (https://github.com/GreedyGame/unity-plugin/wiki/FAQs)

