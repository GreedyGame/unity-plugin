using UnityEngine;
using System.Collections;
using GreedyGame.Runtime;

public class ButtonLoaderScene : MonoBehaviour {

	
	Texture btnTexture;

	void Start () {
    }

    void onGUI()
    {
        if (GUI.Button(new Rect(300, 100, 150, 50), "NEXT"))
        {
            if (Application.loadedLevel == 0)
            {
                Application.LoadLevel(1);
            }
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
