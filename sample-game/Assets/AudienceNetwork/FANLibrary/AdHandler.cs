using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AOT;

namespace AudienceNetwork
{
    public class AdHandler : MonoBehaviour
    {
        private readonly static Queue<Action> executeOnMainThreadQueue = new Queue<Action>();

        public void executeOnMainThread(Action action)
        {
            executeOnMainThreadQueue.Enqueue(action);
        }

        void Update()
        {
            // dispatch stuff on main thread
            while (executeOnMainThreadQueue.Count > 0) {
                executeOnMainThreadQueue.Dequeue().Invoke();
            }
        }

        public void removeFromParent()
        {
#if UNITY_EDITOR
//          UnityEngine.Object.DestroyImmediate (this);
#else
            UnityEngine.Object.Destroy(this);
#endif
        }
    }

    public delegate void FBNativeAdHandlerValidationCallback(bool success);

    [RequireComponent(typeof(RectTransform))]
    public class NativeAdHandler : AdHandler
    {
        public int minViewabilityPercentage;
        public float minAlpha;
        public int maxRotation;
        public int checkViewabilityInterval;
#pragma warning disable 109
        public new Camera camera;
#pragma warning restore 109

        public FBNativeAdHandlerValidationCallback validationCallback;

        private float lastImpressionCheckTime;
        private bool impressionLogged;
        private bool shouldCheckImpression;

        public void startImpressionValidation()
        {
            if (!this.enabled) {
                this.enabled = true;
            }
            this.shouldCheckImpression = true;
        }

        public void stopImpressionValidation()
        {
            this.shouldCheckImpression = false;
        }

        void OnGUI()
        {
            this.checkImpression();
        }

        private bool checkImpression()
        {
            float currentTime = Time.time;
            float secondsSinceLastCheck = currentTime - this.lastImpressionCheckTime;

            if (this.shouldCheckImpression && !this.impressionLogged && (secondsSinceLastCheck > checkViewabilityInterval)) {
                this.lastImpressionCheckTime = currentTime;

                GameObject currentObject = this.gameObject;
                Camera camera = this.camera;
                if (camera == null) {
                    camera = this.GetComponent<Camera>();
                }
                if (camera == null) {
                    camera = Camera.main;
                }

                while (currentObject != null) {
                    Canvas canvas = currentObject.GetComponent<Canvas>();
                    if (canvas != null) {
                        // Break if the current object is a nested world canvas
                        if (canvas.renderMode == RenderMode.WorldSpace) {
                            break;
                        }
                    }

                    bool currentObjectViewable = this.checkGameObjectViewability(camera, currentObject);
                    if (!currentObjectViewable) {
                        if (this.validationCallback != null) {
                            this.validationCallback(false);
                        }
                        return false;
                    }
                    currentObject = null;
                };

                if (this.validationCallback != null) {
                    this.validationCallback(true);
                }
                this.impressionLogged = true;
            }
            return this.impressionLogged;
        }

        private bool logViewability(bool success, string message)
        {
            if (!success) {
                Debug.Log("Viewability validation failed: " + message);
            } else {
                Debug.Log("Viewability validation success! " + message);
            }
            return success;
        }

        private bool checkGameObjectViewability(Camera camera, GameObject gameObject)
        {
            // Check that we have all that we need to work with
            if (gameObject == null) {
                return this.logViewability(false, "GameObject is null.");
            }

            if (camera == null) {
                return this.logViewability(false, "Camera is null.");
            }

            if (!gameObject.activeInHierarchy) {
                return this.logViewability(false, "GameObject is not active in hierarchy.");
            }

            Canvas canvas = getCanvas(gameObject);
            if (canvas == null) {
                return this.logViewability(false, "GameObject is missing a Canvas parent.");
            }

            // Cull items that do not pass the alpha test
            CanvasGroup[] canvasGroups = gameObject.GetComponents<CanvasGroup>();
            foreach (CanvasGroup group in canvasGroups) {
                if (group.alpha < this.minAlpha) {
                    return this.logViewability(false, "GameObject has a CanvasGroup with less than the minimum alpha required.");
                }
            }

            RectTransform transform = gameObject.transform as RectTransform;

            // Check if the width / height are valid
            if (transform.rect.width <= 0 || transform.rect.height <= 0) {
                return this.logViewability(false, "GameObject's height/width is less than or equal to zero.");
            }

            Vector3[] worldCorners = new Vector3[4];
            transform.GetWorldCorners(worldCorners);
            Vector3 gameObjectBottomLeft = worldCorners [0];
            Vector3 gameObjectTopRight = worldCorners [2];
            Vector3 cameraBottomLeft = camera.pixelRect.min;
            Vector3 cameraTopRight = camera.pixelRect.max;

            if (canvas.renderMode != RenderMode.ScreenSpaceOverlay) {
                gameObjectBottomLeft = camera.WorldToScreenPoint(gameObjectBottomLeft);
                gameObjectTopRight = camera.WorldToScreenPoint(gameObjectTopRight);
            }

            // Check that gameObject has 100% width visible
            if (gameObjectBottomLeft.x < cameraBottomLeft.x || gameObjectTopRight.x > cameraTopRight.x) {
                return this.logViewability(false, "Less than 100% of the width of the GameObject is inside the viewport.");
            }

            // Check that gameObject height is bigger than minimum viewability precentage
            int verticalInvisibleThreshold = (int)(camera.pixelRect.height * (100 - this.minViewabilityPercentage) / 100);
            if (cameraTopRight.y - gameObjectTopRight.y > verticalInvisibleThreshold) {
                return this.logViewability(false, "Less than " + this.minViewabilityPercentage + "% visible from the top.");
            }
            if (gameObjectBottomLeft.y - cameraBottomLeft.y > verticalInvisibleThreshold) {
                return this.logViewability(false, "Less than " + this.minViewabilityPercentage + "% visible from the bottom.");
            }

            // Check that item is not rotated too much
            Vector3 rotation = transform.eulerAngles;
            int xRotation = Mathf.FloorToInt(rotation.x);
            int yRotation = Mathf.FloorToInt(rotation.y);
            int zRotation = Mathf.FloorToInt(rotation.z);

            int minRotation = 360 - this.maxRotation;
            int maxRotation = this.maxRotation;

            if (!(xRotation >= minRotation || xRotation <= maxRotation)) {
                return this.logViewability(false, "GameObject is rotated too much. (x axis)");
            } else if (!(yRotation >= minRotation || yRotation <= maxRotation)) {
                return this.logViewability(false, "GameObject is rotated too much. (y axis)");
            } else if (!(zRotation >= minRotation || zRotation <= maxRotation)) {
                return this.logViewability(false, "GameObject is rotated too much. (z axis)");
            }

            return this.logViewability(true, "--------------- VALID IMPRESSION REGISTERED! ----------------------");
        }

        private Canvas getCanvas(GameObject gameObject)
        {
            if (gameObject.GetComponent<Canvas>() != null) {
                return gameObject.GetComponent<Canvas>();
            } else {
                if (gameObject.transform.parent != null) {
                    return getCanvas(gameObject.transform.parent.gameObject);
                }
            }
            return null;
        }
    }
}
