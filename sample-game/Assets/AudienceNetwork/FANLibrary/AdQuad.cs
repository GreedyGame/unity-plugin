using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using AudienceNetwork;

[RequireComponent (typeof(CanvasRenderer))]
[RequireComponent (typeof(RectTransform))]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class AdQuad : MonoBehaviour {

    public AdManager adManager;
    public bool useIconImage;
    public bool useCoverImage;
    private bool adRendered;

    void Start () {
        // Hide game object before ad is loaded
        Renderer renderer = GetComponent<Renderer>();
        renderer.enabled = false;
        adRendered = false;
    }

    void OnGUI ()
    {
        NativeAd nativeAd = adManager.nativeAd;

        // Set ad texture to the quad when nativeAd is created, loaded
        // and not set before
        if (nativeAd && adManager.IsAdLoaded() && !adRendered) {
            Sprite adImage = null;
            if (useCoverImage) {
                adImage = nativeAd.CoverImage;
            } else if (useIconImage) {
                adImage = nativeAd.IconImage;
            }

            if (adImage) {
                // Unhide the game object after ad is loaded
                MeshRenderer meshRenderer = GetComponent<MeshRenderer> ();
                Renderer renderer = GetComponent<Renderer> ();
                renderer.enabled = true;

                // Set ad texture with gameobject material
                Texture2D adTexture = adImage.texture;
                Material material = new Material (Shader.Find ("Sprites/Default"));
                material.color = Color.white;
                material.SetTexture ("texture", adTexture);

                meshRenderer.sharedMaterial = material;
                renderer.material.mainTexture = adTexture;
                adRendered = true;
            }
        }
    }
}
