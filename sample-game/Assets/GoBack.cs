using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoBack : MonoBehaviour {

    public int loadLevel = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (GUI.Button(new Rect(500, 0, 300, 200), "Go back"))
        {
            if (Application.loadedLevel != loadLevel)
            {
                Application.LoadLevel(loadLevel);
            }
        }
    }
}
