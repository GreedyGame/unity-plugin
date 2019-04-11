using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GreedyGame.Runtime;
using UnityEngine.UI;

public class GGCustomRenderer : MonoBehaviour {

    RawImage rawImage;
    public Texture defaultTexture;
    public string unitId = null;

	// Use this for initialization
	void Start () {
        // Use this api to register delegates which will send you the branded texture.
        // The below delegate is an example which uses raw image to render image.
        // You should make sure that the actual rendering of the object is done inside the delegate
        GreedyGameAgent.Instance.RegisterGameObject(unitId, this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {

	}

    void OnDestroy()
    {
        GreedyGameAgent.Instance.UnregisterGameObject(this.gameObject);
    }
}
