using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using AudienceNetwork;
using UnityEngine.SceneManagement;

[RequireComponent (typeof(CanvasRenderer))]
[RequireComponent (typeof(RectTransform))]
public class AdPanel : MonoBehaviour
{
    public AdManager adManager;

    // UI elements in scene
    [Header("Text:")]
    public Text
    title;
    public Text socialContext;
    //public Text status; // For testing purposes
    [Header("Images:")]
    public Image
    coverImage;
    public Image iconImage;
    [Header("Buttons:")]
    public Text
    callToAction;
    public Button callToActionButton;

    private bool adIconContentFilled;
    private bool adCoverContentFilled;
    private bool adTextContentFilled;

    void Awake ()
    {
        adIconContentFilled = false;
        adCoverContentFilled = false;
        adTextContentFilled = false;
    }

    void Update ()
    {
        NativeAd nativeAd = adManager.nativeAd;
        if (adManager.IsAdLoaded() && nativeAd != null) {
            // Update GUI from native ad
            if (nativeAd.CoverImage != null && !adCoverContentFilled) {
                adCoverContentFilled = true;
                coverImage.sprite = nativeAd.CoverImage;
            }
            if (nativeAd.IconImage != null && !adIconContentFilled) {
                adIconContentFilled = true;
                iconImage.sprite = nativeAd.IconImage;
            }

            if (!adTextContentFilled) {
                adTextContentFilled = true;
                title.text = nativeAd.Title;
                socialContext.text = nativeAd.SocialContext;
                callToAction.text = nativeAd.CallToAction;
            }
        }
    }

    public void registerGameObjectForImpression () {
        NativeAd nativeAd = adManager.nativeAd;
        if (nativeAd != null && gameObject.GetComponent<NativeAdHandler> () == null) {
            // Wire up GameObject with the native ad; the specified buttons will be clickable
            // if the ad panel is not registed with native ad
            nativeAd.RegisterGameObjectForImpression (gameObject, new Button[] { callToActionButton });
        }
    }
}
