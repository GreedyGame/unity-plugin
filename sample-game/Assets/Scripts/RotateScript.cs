using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnGUI() {
        if(GUI.Button(new Rect(0,0,200,100),"Rotate")) {
            this.gameObject.GetComponent<Camera>().transform.rotation *= Quaternion.Euler(0, 180, 0);
        }
    }
}
