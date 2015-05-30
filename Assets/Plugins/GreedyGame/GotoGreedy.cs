using UnityEngine;
using System.Collections;

public class GotoGreedy : MonoBehaviour {

	static bool isLoad = false;
	void Awake(){
		
		Debug.Log ("Awake GotoGreedy " + isLoad + gameObject.name);
		
		DontDestroyOnLoad(gameObject);
	}

	void Start () {
		Debug.Log ("GotoGreedy " + isLoad);
		if (isLoad == false) {
			isLoad = true;
			Application.LoadLevel(2);
		}
	}
}
