using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GreedyGame.Runtime;

public class GGRenderer : MonoBehaviour {

    public Texture2D texture;
    public string unitId;


	// Use this for initialization
	void Start () {
        // Attach this script to an object that needs branding. Make sure that the object has 
        // mesh or sprite renderer attached to it.
        GreedyGameAgent.Instance.registerGameObject(this.gameObject, texture, unitId,true);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // Destroy
    void OnDestroy()
    {
        GreedyGameAgent.Instance.unregisterGameObject(this.gameObject);
    }
}
