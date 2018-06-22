using UnityEngine;
using System.Collections;
using GreedyGame.Runtime;

public class ButtonLoaderScene : MonoBehaviour {

	
	Texture btnTexture;

	void Start () {
	}
	
	




    private static void moveToNextScene()
    {
        if (Application.loadedLevel == 0)
        {
            Application.LoadLevel(1);
        }
    }
}
