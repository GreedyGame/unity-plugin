using UnityEngine;
using System.Collections;
using GreedyGame.Runtime;

public class FloatIdentifier : MonoBehaviour {

    public Texture2D defaultTexture;
    Renderer renderer;
    void Start () {
        renderer = GetComponent<Renderer>();
    }
	
	public void updateTexture(Texture2D texture)
    {
        if (texture != null) {
            Debug.Log("GG[Identifier] texture non null ");
            
            if (renderer != null)
            {
                Debug.Log("GG[Identifier] renderer is null ");
                renderer.material.mainTexture = texture;
            }
            
        } else
        {
            if (renderer != null && defaultTexture!=null)
            {
                Debug.Log("GG[Identifier] texture is null and renderer non null defaulting");
                renderer.sharedMaterial.mainTexture = defaultTexture;
            }
        }
    }
}
