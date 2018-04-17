using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using AudienceNetwork;
using UnityEngine.SceneManagement;

public class AdViewTest : MonoBehaviour
{

    private AdView adView;
    private AdPosition currentAdViewPosition;

    void Awake ()
    {
        // Create a banner's ad view with a unique placement ID (generate your own on the Facebook app settings).
        // Use different ID for each ad placement in your app.
        AdView adView = new AdView ("YOUR_PLACEMENT_ID", AdSize.BANNER_HEIGHT_50);
        this.adView = adView;
        this.adView.Register (this.gameObject);
        this.currentAdViewPosition = AdPosition.CUSTOM;


        // Set delegates to get notified on changes or when the user interacts with the ad.
        this.adView.AdViewDidLoad = (delegate() {
            Debug.Log ("Ad view loaded.");
            this.adView.Show (100);
        });
        adView.AdViewDidFailWithError = (delegate(string error) {
            Debug.Log ("Ad view failed to load with error: " + error);
        });
        adView.AdViewWillLogImpression = (delegate() {
            Debug.Log ("Ad view logged impression.");
        });
        adView.AdViewDidClick = (delegate() {
            Debug.Log ("Ad view clicked.");
        });

        // Initiate a request to load an ad.
        adView.LoadAd ();
    }

    void OnDestroy ()
    {
        // Dispose of banner ad when the scene is destroyed
        if (this.adView) {
            this.adView.Dispose ();
        }
        Debug.Log ("AdViewTest was destroyed!");
    }

    // Next button
    public void NextScene ()
    {
        SceneManager.LoadScene ("NativeAdScene");
    }

    // Change button
    // Change the position of the ad view when button is clicked
    // ad view is at top: move it to bottom
    // ad view is at bottom: move it to 100 pixels along y-axis
    // ad view is at custom position: move it to the top
    public void ChangePosition ()
    {
        switch (this.currentAdViewPosition) {
        case AdPosition.TOP:
            this.adView.Show (AdPosition.BOTTOM);
            this.currentAdViewPosition = AdPosition.BOTTOM;
            break;
        case AdPosition.BOTTOM:
            this.adView.Show (100);
            this.currentAdViewPosition = AdPosition.CUSTOM;
            break;
        case AdPosition.CUSTOM:
            this.adView.Show (AdPosition.TOP);
            this.currentAdViewPosition = AdPosition.TOP;
            break;
        }
    }
}
