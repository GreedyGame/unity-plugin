using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchScene : MonoBehaviour {

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(this);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnGUI() {
		if(GUI.Button(new Rect( 300, 0, 125, 50), "Switch")) {
			int i = Application.loadedLevel;
			i++;
			i %= 8;
			Application.LoadLevel(i);
		}
	}

}
