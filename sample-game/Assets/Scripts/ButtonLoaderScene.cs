using UnityEngine;
using System.Collections;
using GreedyGame.Runtime;

public class ButtonLoaderScene : MonoBehaviour {

	
	Texture btnTexture;

	void Start () {
	}
	
	void OnGUI() {
			if (GUI.Button (new Rect (Screen.width / 2 , Screen.height / 2,100,30 ), "Start Game")) {
				Debug.Log ("GG[ButtonLoaderScene] Start Game ");
                moveToNextScene();
                
			}
		}




    private static void moveToNextScene()
    {
        if (Application.loadedLevel == 0)
        {
            Application.LoadLevel(1);
        }
    }
}
