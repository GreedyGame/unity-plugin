using UnityEngine;
using System.Collections;
using GreedyGame.Runtime.Common;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GreedyAdManager.Instance.FetchAdHead("float-123");
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void Destroy()
	{
		GreedyAdManager.Instance.RemoveAllAdHead();
	}



}
