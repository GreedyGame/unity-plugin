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

        Debug.Log("UnityGG Start called inside custom renderer");
        GreedyGameAgent.Instance.registerGameObject(this.gameObject, defaultTexture as Texture2D, unitId, delegate (string unitID, Texture2D brandedTexture, bool isBrandedTexture)
         {
             Debug.Log("UnityGG Delegate called inside custom renderer");
             if (brandedTexture != null)
             {
                 Debug.Log("UnityGG branded texture found");
                 //Step Delegate-A
                 rawImage = GetComponent<RawImage>();
                 if(rawImage!=null)
                 {
                     rawImage.texture = brandedTexture as Texture;
                 }
             } 

             // If you want to enable or disable the gameobject based on whether the texture is branded or default
             // you can use isBrandedFlag
             if(!isBrandedTexture)
            {
                Debug.Log("UnityGG branded texture not found");
                 //Step Delegate-B
                 //you can disable the gameobject here if you dont want to show game object
                 // with default texture.
             }
         });
	}
	
	// Update is called once per frame
	void Update () {

	}

    void OnDestroy()
    {
        GreedyGameAgent.Instance.unregisterGameObject(this.gameObject);
    }
}
