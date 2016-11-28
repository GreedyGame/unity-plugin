using UnityEngine;
using System;
using System.Collections;
using GreedyGame.Runtime;
using GreedyGame.Platform;

public class FloatUnitLoader : MonoBehaviour {

	public string FloatUnit = null;
	private GreedyGameAgent greedyGameAgent = null;
	
	void Awake (){	
		Debug.Log(String.Format("FloatUnitLoader - awake {0}", FloatUnit));
		greedyGameAgent = GreedyGameAgent.Instance;
	}

	void Start (){
		greedyGameAgent.showFloat(FloatUnit);
	}
	
	void OnDestroy (){
		greedyGameAgent.removeFloat (FloatUnit);
	}

}
