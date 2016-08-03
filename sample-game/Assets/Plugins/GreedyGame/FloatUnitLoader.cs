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
		greedyGameAgent.fetchFloatUnit (FloatUnit);
		greedyGameAgent.setActionListener(FloatUnit, new GreedyOnActionPerformedListener());
	}
	
	void OnDestroy (){
		greedyGameAgent.removeCurrentFloatUnit ();
	}
	

	public class GreedyOnActionPerformedListener : IActionListener {
		public bool onActionPerformed(String action) {
			/**
         * TODO: The reward action has been completed. The reward point can be parsed from string action
         **/
			return false;
		}
	}
}
