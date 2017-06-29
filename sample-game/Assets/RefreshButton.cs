using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GreedyGame.Runtime;

public class RefreshButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(0, 400, 300, 200), "event")) {
            GreedyGameAgent.Instance.startEventRefresh();
            Debug.Log("GG[unity] start event refresh");
        }
    }
}
