GreedyGame Unity Integration Guide
===================

This is a complete guide to integrate GreedyGame plugin within your unity game. You can download [GreedyGame_v5.7.1.package](current-sdk/GreedyGame_v5.7.1.package).

### Steps

#### 1. Import Plugin Package
- Using TopMenu: *Assets > Import Package > Custom Package*
- Import GreedyGame_v5.7.1.package into your unity project.

#### 2. Select GameObject for branding
- Attach complie MonoBehaviour **ThemeUnit** or **SharedThemeUnit**  to GameObject having **Renderer**.
- Supported Renderers are Mesh, Plan, Cloth and Sprite (only with SharedThemeUnit).
- GameObject must having 2D texture.
- ![SharedThemeUnit MonoBehaviour](screen_shots/2_attached_monobehaviour.png?raw=true "SharedThemeUnit MonoBehaviour attached to Stockcar/Body_Complete" )

#### 3. Setting up with Server
- Using TopMenu: *GreedyGame > DynamicUnitManager*
- Login using panel's credential.
- Build and sync unit list.
  
| Refresh List  | Save List     |
| ------------- | ------------- |
| To create list of units to be used for branding | To sync list with server and register as ad-unit |
| ![Refresh UnitList](screen_shots/5_post_refresh.png?raw=true "list of units to be used for branding" )  | ![Save UnitList](screen_shots/6_post_save.png?raw=true "sync list with server and register as ad-unit" )  |
- **Yellow** indicates unit has been added
- **Green** indicates unit has synced to server
- **Red** indicates unit cannot to added or invalid 

#### 4. Manage campagin fetching and post loading scene
- For quick action, sample script `GreedyCampaignLoader.cs` can be attached to loading scene's object.
- For advance customization, here explanation of `GreedyCampaignLoader.cs`
  - GreedyGame's headers 
```csharp
//Headers
using GreedyGame.Runtime.Common;
using GreedyGame.Platform;
```
  - GreedyAdManager's method. (Singleton object)
```csharp
private GreedyAdManager ggAdManager = null;
void Awake(){
//Initialization as singleton
	ggAdManager = GreedyAdManager.Instance;
}

void Start() {
//Taking values from GlobalConfig
	GlobalConfig[] ggLoaders = Resources.FindObjectsOfTypeAll<GlobalConfig> ();
	GlobalConfig ggConfig = ggLoaders [0];
	
//Making server request for suitable campaign
	ggAdManager.init (ggConfig.GameId, ggConfig.AdUnits.ToArray(), OnGreedyEvent);
}
```
  - Event's listener
```csharp
void OnGreedyEvent(RuntimeEvent greedy_events){
	if (greedy_events == RuntimeEvent.CAMPAIGN_LOADED || 
	    greedy_events == RuntimeEvent.CAMPAIGN_NOT_LOADED) {
	//Goto play scene if server reponse is recevied
		Application.LoadLevel (1);
	}

	if (greedy_events == RuntimeEvent.UNIT_CLOSED){
		//Do game resume, if click unit is closed
	}else if(greedy_events == RuntimeEvent.UNIT_OPENED){
		//Do game resume, if click unit is opened
	}
}
```

---
#### Checking runtime unit list

1. Goto loading scene, here Demo Scene.
2. Select **GreedyGameConfigObject**, and look for **GlobalConfig** component attached.
3. Validate value of **GlobalConfig** component, with values from *panel.greedygame.com*
 ![GreedyGameConfigObject](screen_shots/7_global_config.png?raw=true "Checking runtime unit list" )



