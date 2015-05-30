GreedyGame Unity Integration Guide
===================

This is a complete guide to integrate GreedyGame plugin within your unity game. You can download [GreedyGame_v5.7.1.package](current-sdk/GreedyGame_v5.7.1.package).

#### Steps

1. Import GreedyGame_v5.7.1.package into your unity project.
   - Using TopMenu: *Assets > Import Package > Custom Package*

2. Attached **ThemeUnit** or **SharedThemeUnit** monobehaviour to GameObject having **Renderer** (Mesh, Sprite, Cloth). And must having texture.
![SharedThemeUnit MonoBehaviour](screen_shots/2_attached_monobehaviour.png?raw=true "SharedThemeUnit MonoBehaviour attached to Stockcar/Body_Complete" )

3. Login using panel's credential.
   - Using TopMenu: *GreedyGame > DynamicUnitManager*

4. Build and sync unit list.
  - Using TopMenu: *GreedyGame > DynamicUnitManager*
  
| Refresh List  | Save List     |
| ------------- | ------------- |
| To create list of units to be used for branding | To sync list with server and register as ad-unit |
| ![SharedThemeUnit MonoBehaviour](screen_shots/5_post_refresh.png?raw=true "SharedThemeUnit MonoBehaviour attached to Stockcar/Body_Complete" )  | ![SharedThemeUnit MonoBehaviour](screen_shots/6_post_save.png?raw=true "SharedThemeUnit MonoBehaviour attached to Stockcar/Body_Complete" )  |
- Yellow indicates unit has been added
- Green indicates unit has synced to server
- Red indicates unit cannot to added or invalid 
