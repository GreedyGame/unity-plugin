using UnityEngine;
using System.Collections;
using GreedyGame.Runtime;

public class NativeIdentifier : MonoBehaviour {

    public Texture2D defaultTexture;
    Renderer renderer;

	void Start () {
        renderer = GetComponent<Renderer>();
    }
	
	public void updateTexture(Texture2D texture)
    {
        if (texture != null) {
            Debug.Log("GG[NIdentifier] texture non null ");
            Renderer renderer = this.gameObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                Debug.Log("GG[NIdentifier] renderer is not null ");
                renderer.material.mainTexture = texture;
            }
            
        } else
        {
            if (renderer != null && defaultTexture!=null)
            {
                Debug.Log("GG[NIdentifier] texture is null and renderer non null defaulting");
                renderer.sharedMaterial.mainTexture = defaultTexture;
            }
            
        }
    }
}
