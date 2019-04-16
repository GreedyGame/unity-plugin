using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using AudienceNetwork;

public class AdManager : MonoBehaviour
{
    public NativeAd nativeAd;
    public GameObject targetAdObject; // target ad object that will check for impression
    public Button targetButton; // target button that will check for click
    bool adLoaded;

    void Start () {
        adLoaded = false;
        LoadAd ();
    }

    void OnDestroy ()
    {
        // Dispose of native ad when the scene is destroyed
        if (this.nativeAd) {
            this.nativeAd.Dispose ();
        }
        Debug.Log ("NativeAdTest was destroyed!");
    }

    public bool IsAdLoaded ()
    {
        return adLoaded;
    }

    // Load Ad button
    public void LoadAd ()
    {
        // Create a native ad request with a unique placement ID (generate your own on the Facebook app settings).
        // Use different ID for each ad placement in your app.
        NativeAd nativeAd = new AudienceNetwork.NativeAd ("YOUR_PLACEMENT_ID");
        this.nativeAd = nativeAd;

        // Wire up GameObject with the native ad; the specified buttons will be clickable.
        if (targetAdObject) {
            if (targetButton) {
                nativeAd.RegisterGameObjectForImpression (targetAdObject, new Button[] { targetButton });
            } else {
                nativeAd.RegisterGameObjectForImpression (targetAdObject, new Button[] { });
            }
        } else {
            nativeAd.RegisterGameObjectForImpression (gameObject, new Button[] { });
        }

        // Set delegates to get notified on changes or when the user interacts with the ad.
        nativeAd.NativeAdDidLoad = (delegate() {
            adLoaded = true;
            Debug.Log ("Native ad loaded.");
            Debug.Log ("Loading images...");
            // Use helper methods to load images from native ad URLs
            StartCoroutine (nativeAd.LoadCoverImage (nativeAd.CoverImageURL));
            StartCoroutine (nativeAd.LoadIconImage (nativeAd.IconImageURL));
            Debug.Log ("Images loaded.");
        });
        nativeAd.NativeAdDidFailWithError = (delegate(string error) {
            Debug.Log ("Native ad failed to load with error: " + error);
        });
        nativeAd.NativeAdWillLogImpression = (delegate() {
            Debug.Log ("Native ad logged impression.");
        });
        nativeAd.NativeAdDidClick = (delegate() {
            Debug.Log ("Native ad clicked.");
        });

        // Initiate a request to load an ad.
        nativeAd.LoadAd ();

        Debug.Log ("Native ad loading...");
    }
}
