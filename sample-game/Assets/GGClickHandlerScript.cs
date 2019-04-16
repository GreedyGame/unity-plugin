using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GreedyGame.Runtime;

public class GGClickHandlerScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void GGShowUII (string unitId) {
        GreedyGameAgent.Instance.showEngagementWindow(unitId);
    }

    public void GGRefresh() {
        GreedyGameAgent.Instance.startEventRefresh();
    }

}
