GreedyGame Unity Integration Guide
===================

This is a complete guide to integrate GreedyGame plugin within your unity game. You can download [GreedyGame_v5.7.2.unitypackage](current-sdk/GreedyGame_v5.7.2.unitypackage).

### Ads that people love

![SharedThemeUnit MonoBehaviour](https://raw.githubusercontent.com/GreedyGame/Unity-Sample/master/screen-shots/1_branded_game.png?raw=true "SharedThemeUnit MonoBehaviour attached to Stockcar/Body_Complete" )


## Steps

### 1. Import Plugin Package
* **TopMenu**: *Assets > Import Package > Custom Package*
* Import GreedyGame_v5.7.2.package into your unity project.

### 2. Select GameObject for branding
* Attach complie MonoBehaviour **ThemeUnit** or **SharedThemeUnit**  to GameObject having **Renderer**.
* Supported Renderers are Mesh, Plan, Cloth and Sprite (only with SharedThemeUnit).
* GameObject must having 2D texture.

> Preview: SharedThemeUnit MonoBehaviour attached to Stockcar/Body_Complete

> ![SharedThemeUnit MonoBehaviour](https://raw.githubusercontent.com/GreedyGame/Unity-Sample/master/screen-shots/2_attached_monobehaviour.png?raw=true "SharedThemeUnit MonoBehaviour attached to Stockcar/Body_Complete" )
 1. SharedThemeUnit Attached, yellow helpbox states it ready to build in unitlist
 2. 2D texture, will be used for branded assets, such as logo, product image etc.
 3. MeshRender will be used as renderer to blend branding image over object

### 3. Setting up with Server
* Using TopMenu: *GreedyGame > DynamicUnitManager*
* Login using panel's credential.
* Build and sync unit list.

> Preview: list of units to be used for branding. Left list post refresh action and right list after save action.

> ![Refresh UnitList](https://raw.githubusercontent.com/GreedyGame/Unity-Sample/master/screen-shots/5_refresh_save.png?raw=true "list of units to be used for branding" )
 1. **GameProfileId**, game-id from panel.greedygame.com
 2. **LoadingLevel**, will be used for fetching and loading campaign assets

 **Buttons**
 
| Refresh       | Save       | Export      |
| ------------- | ----------- | ----------- |
| Create list of units to be used for branding    | Sync list with server and register as ad-unit | Export ad-unit  as package rar |

**Indicators**
* **Yellow** indicates unit has been added
* **Green** indicates unit has synced to server
* **Red** indicates unit cannot to added or invalid 

### 4. Manage campagin fetching and post loading scene
* Attach sample script `GreedyCampaignLoader.cs` with loading scene's object.
	* PostScene : Scene to load after campaign get fetched
	* Loading : Loading asset's texture


### 5. Add Floating Ad-Head
* Attach `AdHeadLoader.cs` with respective scene's object to fetch floating AdHead on that scene.
	* Set-up ad-unit manually on panel.greedygame.com/games/<game_namespace>/units 
	* AdUnit : Panel's ad-unit to fetch specific unit from server. 

* For advance customization, see Documentation of **GreedyAdManager** object
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

#### Init
`init(String GameId, String[] AdUnits, OnGreedyEvent)`

Lookup for new native campaign from server.
* GameId - Unique game profile id from panel.greedygame.com
* AdUnits - Array register unit id. eg. Unit-XYZ
* OnGreedyEvent - Callback function for **RuntimeEvent** as follow:
	- CAMPAIGN_NOT_LOADED
	- CAMPAIGN_LOADED
	- CAMPAIGN_DOWNLOADED
	- UNIT_OPENED
	- UNIT_CLOSED

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
		ggAdManager.init (ggConfig.GameId, ggConfig.AdUnits.ToArray (), OnGreedyEvent);
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

	if (greedy_events == RuntimeEvent.UNIT_CLOSED){
		//Do game resume, if click unit is closed
	}else if(greedy_events == RuntimeEvent.UNIT_OPENED){
		//Do game resume, if click unit is opened
	}
}
```
---

## Checking runtime unit list

1. Goto loading scene, here Demo Scene.
2. Select **GreedyGameConfigObject**, and look for **GlobalConfig** component attached.
3. Validate value of **GlobalConfig** component, with values from *panel.greedygame.com*

> ![GreedyGameConfigObject](https://raw.githubusercontent.com/GreedyGame/Unity-Sample/master/screen-shots/6_global_config.png?raw=true "Checking runtime unit list" )



