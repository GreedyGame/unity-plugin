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

---
#### Checking runtime unit list

1. Goto loading scene, here Demo Scene.
2. Select **GreedyGameConfigObject**, and look for **GlobalConfig** component attached.
3. Validate value of **GlobalConfig** component, with values from *panel.greedygame.com*
 ![GreedyGameConfigObject](screen_shots/7_global_config.png?raw=true "Checking runtime unit list" )



