using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GreedyGame.Runtime;
using UnityEngine.UI;

public class NiksCustomRenderer : MonoBehaviour {

  public string unitId;
	// Use this for initialization
	void Start () {
		
		GreedyGameAgent.Instance.RegisterGameObject (unitId, this.gameObject,  delegate (string unitID, GGNativeUnit coreUnit) {
                    Debug.Log("Inside the delegate with unitid" + unitID);
                    Texture2D textureToApply = coreUnit.defaultTexture;
                    if(coreUnit.adTexture != null) {
                        textureToApply = coreUnit.adTexture;
                    }
                    if (textureToApply != null) {
                        /**
                          *TODO: Apply assigned Texture to the game object. 
                        The assigned texture can be the branded texture, the default texture or a transparent texture.
                         A transparent texture is returned in case you pass the default texture as null.
                          **/
                        GetComponent<RawImage> ().texture = textureToApply;

                    }
                });

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
