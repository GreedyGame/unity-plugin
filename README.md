GreedyGame Unity Integration Guide
===================

This is a complete guide to integrate GreedyGame plugin within your unity game. You can download [GreedyGame_v5.7.1.package](current-sdk/GreedyGame_v5.7.1.package).

### Steps

#### Import Plugin Package
- Using TopMenu: *Assets > Import Package > Custom Package*
- Import GreedyGame_v5.7.1.package into your unity project.

#### Select GameObject for branding
- Attach complie MonoBehaviour **ThemeUnit** or **SharedThemeUnit**  to GameObject having **Renderer**.
- Supported Renderers are Mesh, Plan, Cloth and Sprite (only with SharedThemeUnit).
- GameObject must having 2D texture.
- ![SharedThemeUnit MonoBehaviour](screen_shots/2_attached_monobehaviour.png?raw=true "SharedThemeUnit MonoBehaviour attached to Stockcar/Body_Complete" )

#### Setting up with Server
- Using TopMenu: *GreedyGame > DynamicUnitManager*
- Login using panel's credential.
- Build and sync unit list.
  
| Refresh List  | Save List     |
| ------------- | ------------- |
| To create list of units to be used for branding | To sync list with server and register as ad-unit |
| ![SharedThemeUnit MonoBehaviour](screen_shots/5_post_refresh.png?raw=true "SharedThemeUnit MonoBehaviour attached to Stockcar/Body_Complete" )  | ![SharedThemeUnit MonoBehaviour](screen_shots/6_post_save.png?raw=true "SharedThemeUnit MonoBehaviour attached to Stockcar/Body_Complete" )  |
- <b style='color:yellow'>Yellow</b> indicates unit has been added
- <b style='color:green'>Green</b> indicates unit has synced to server
- <b style='color:red'>Red</b> indicates unit cannot to added or invalid 
