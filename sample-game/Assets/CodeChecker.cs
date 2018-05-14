using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GreedyGame.Runtime;
using GreedyGame.Commons;
using System;

public class CodeChecker : MonoBehaviour {

    RawImage rawImage;
    public Texture2D defaultTexture;
    public string unitId;

    // Use this for initialization
    void Start()
    {
        GreedyGameAgent.Instance
        .registerGameObject(this.gameObject,
                            defaultTexture as Texture2D,
                            unitId,
                            delegate (string unitId, Texture2D brandedTexture) {
                                //handling branding of units
                                if (brandedTexture != null)
                                {
                                    //Step Delegate-A
                                    rawImage = GetComponent<RawImage>();
                                    if (rawImage != null)
                                    {
                                        rawImage.texture = brandedTexture as Texture;
                                    }
                                }
                                // You should make sure that you are setting the unit texture 
                                // to the default one in case the delegate returns null.
                                else
                                {
                                    //Step Delegate-B 
                                    rawImage = GetComponent<RawImage>();
                                    if (rawImage != null)
                                    {
                                        rawImage.texture = defaultTexture;
                                    }
                                }
                            });




        GreedyGameAgent.Instance
            .registerGameObject(this.gameObject,
                                defaultTexture,
                                unitId,
                                true);

        // Use this API to register delegates which will send you the branded texture.
        // The below delegate is an example which uses raw image to render image.
        // You should make sure that the actual rendering of the object 
        // is done inside the delegate
        GreedyGameAgent.Instance
            .registerGameObject(this.gameObject,
                              defaultTexture as Texture2D,
                              unitId,
                              delegate (string unitId, Texture2D brandedTexture) {
                              //Handling branded units
                              if (brandedTexture != null)
                                  {
                                  //Step Delegate-A
                                  rawImage = GetComponent<RawImage>();
                                      if (rawImage != null)
                                      {
                                          rawImage.texture = brandedTexture as Texture;
                                      }
                                  }
                              // You should make sure that you are setting the unit texture 
                              // to the default one in case the delegate returns null.
                              else
                                  {
                                  //Step Delegate-B 
                                  rawImage = GetComponent<RawImage>();
                                      if (rawImage != null)
                                      {
                                          rawImage.texture = defaultTexture;
                                      }
                                  }
                              });
    }

    // Destroy
    void OnDestroy()
    {
        GreedyGameAgent.Instance.unregisterGameObject(this.gameObject);
    }

    public class GreedyAgentListener : IAgentListener
    {
        public void onFound()
        {
            // Campaign is found. onAvailable(string campaignId) will be 
            // invoked once campaign is ready.
        }
        public void onProceed()
        {
            // Use this call to switch from loading scene to the first scene.
            // Make sure that you perform checks to see if you are in the loading
            // scene and then only switch scene or else upon a refresh call you might
            // be switched from an intermediate scene to the next one. 
        }
        public void onAvailable(string campaignId)
        {
            // New campaign is available and ready to use for the next scene. 
            // The campaign id can be used for tracking purposes. 
            // This will also serve helpful in case of event based refresh.
        }

        public void onUnavailable()
        {
            // No campaign is available, proceed with normal flow of the game.
        }

        public void onError(string errorMessage)
        {
            // Error in fetching the campaign. Proceed with the normal game flow
        }

        public void onProgress(int progress)
        {
            //This is a deprecated method
        }
    }


}
