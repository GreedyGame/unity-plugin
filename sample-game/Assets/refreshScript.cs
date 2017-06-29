using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GreedyGame.Runtime;
using GreedyGame.Platform;

public class refreshScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    void Awake ()
    {
        GreedyGameAgent.Instance.startEventRefresh();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
