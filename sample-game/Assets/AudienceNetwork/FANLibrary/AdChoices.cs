using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AudienceNetwork {

    public class AdChoices : MonoBehaviour {
        [Header("Ad Choices:")]
        [SerializeField]
        private Image image;
        [SerializeField]
        private Text text;
        [SerializeField]
        private CanvasGroup canvasGroup;
        private string linkURL;

        void Awake() {
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
        }

        public void SetNativeAd(NativeAd nativeAd) {
            image.sprite = nativeAd.AdChoicesImage;
            text.text = nativeAd.AdChoicesText;

            linkURL = nativeAd.AdChoicesLinkURL;

            canvasGroup.alpha = 1;
            canvasGroup.interactable = true;
        }

        public void AdChoicesTapped() {
            Application.OpenURL (linkURL);
        }
    }
}
