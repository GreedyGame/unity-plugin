using UnityEngine;
using System.Collections;
using GreedyGame.Runtime;
using GreedyGame.Platform;

public class showScriptGG : MonoBehaviour {

	public string FloatUnit = null;
	private GreedyGameAgent ggAdManager = null;

	void Awake (){	
		ggAdManager = GreedyGameAgent.Instance;
	}

	public void showEngage() {
		ggAdManager.showEngagementWindow (FloatUnit);

	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
