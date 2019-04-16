/**
 * Copyright (c) 2014-present, Facebook, Inc. All rights reserved.
 *
 * You are hereby granted a non-exclusive, worldwide, royalty-free license to use,
 * copy, modify, and distribute this software in source code or binary form for use
 * in connection with the web services and APIs provided by Facebook.
 *
 * As with any software that integrates with the Facebook platform, your use of
 * this software is subject to the Facebook Developer Principles and Policies
 * [http://developers.facebook.com/policy/]. This copyright notice shall be
 * included in all copies or substantial portions of the software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
 * FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
 * COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
 * IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
 * CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */
using System.IO;
using System.Text;
using System.Threading;
using UnityEditor.Callbacks;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System;

namespace AudienceNetwork.Editor
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection;
    using UnityEditor;
    using UnityEngine;
    using AudienceNetwork.Editor;

    public class AudienceNetworkSettingsEditor : UnityEditor.Editor
    {
        private static string title = "Audience Network SDK";

        [MenuItem("Tools/Audience Network/About")]
        private static void AboutGUI ()
        {
            string aboutString = System.String.Format ("Facebook Audience Network Unity SDK Version {0}",
                                               AudienceNetwork.SdkVersion.Build);
            EditorUtility.DisplayDialog (title,
                                         aboutString,
                                         "Okay");
        }

        [MenuItem("Tools/Audience Network/Regenerate Android Manifest")]
        private static void RegenerateManifest ()
        {
            bool updateManifest = EditorUtility.DisplayDialog (title,
                                                               "Are you sure you want to regenerate your Android Manifest.xml?",
                                                               "Okay",
                                                               "Cancel");

            if (updateManifest) {
                AudienceNetwork.Editor.ManifestMod.GenerateManifest ();
                EditorUtility.DisplayDialog (title, "Android Manifest updated. \n \n If interstitial ads still throw ActivityNotFoundException, " +
                    "you may need to copy the generated manifest at " + ManifestMod.AndroidManifestPath + " to /Assets/Plugins/Android.", "Okay");
            }
        }

        [MenuItem("Tools/Audience Network/Internal/Build SDK Package")]
        private static void BuildGUI ()
        {
            try {
                string exportedPath = AudienceNetworkBuild.ExportPackage ();
                EditorUtility.DisplayDialog (title, "Exported to " + exportedPath, "Okay");

            } catch (System.Exception e) {
                EditorUtility.DisplayDialog (title, e.Message, "Okay");
            }
        }

        [MenuItem("Tools/Audience Network/Internal/Build SDKs from Source")]
        private static void BuildSDKs ()
        {
            try {
                SDKVersionWindow window = ScriptableObject.CreateInstance<SDKVersionWindow>();
                window.minSize = new Vector2(400, 150);
                window.ShowUtility();
            } catch (System.Exception e) {
                EditorUtility.DisplayDialog (title, e.Message, "Okay");
            }
        }
    }

    public class SDKVersionWindow : EditorWindow {

        public string version = "";
        public bool skipBuild = false;
        private bool building = false;
        private IEnumerator<SDKBuildStatus> buildStatusEnumerator;

        private const float maxHeight = float.MaxValue;

        void OnEnable() {
            this.titleContent.text = "Audience Network Unity SDK Build Generator";
        }

        void OnGUI() {
            EditorGUILayout.LabelField("SDK Base Version: (4.19.0)", version, GUILayout.MinWidth(600));
            this.version = GUILayout.TextField(this.version);

            this.skipBuild = GUILayout.Toggle(this.skipBuild, "Skip build?");

            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Generate Build") && this.version.Length > 0) {
                this.building = true;
            }

            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Cancel")) {
                this.EndBuild();
            }
        }

        void OnInspectorUpdate() {
            Repaint();
        }

        void Update()
        {
            if (building) {
                if (this.buildStatusEnumerator == null) {
                    IEnumerable<SDKBuildStatus> buildStatusEnumerable = AudienceNetwork.Editor.AudienceNetworkBuild.RunSDKBuild(SdkVersion.Build, false, (delegate(bool success, string version, string message, string buildOutput, string buildError) {
                        UnityEngine.Debug.Log("Build Complete for " + version + ".\nSuccess? " + success.ToString());
                        this.building = false;
                    }));

                    this.buildStatusEnumerator = buildStatusEnumerable.GetEnumerator();
                }

                if (buildStatusEnumerator.MoveNext()) {
                    SDKBuildStatus buildStatus = buildStatusEnumerator.Current;
                    IList<string> logs = buildStatus.CurrentLogOutput;
                    if (logs.Count > 0) {
                        UnityEngine.Debug.Log(logs.Pop());
                    }

                    if (buildStatus != null && !buildStatus.BuildInProgress) {
                        building = false;
                    }
                }
            }
        }

        void OnDisable() {
            this.EndBuild();
        }

        void EndBuild() {
            this.building = false;
            if (this.buildStatusEnumerator != null && buildStatusEnumerator.MoveNext()) {
                SDKBuildStatus buildStatus = buildStatusEnumerator.Current;
                if (buildStatus != null && buildStatus.process != null) {
                    KillProcess(buildStatus.process);
                }
                this.buildStatusEnumerator = null;
                UnityEngine.Debug.Log("Build cancelled.");
            }
        }

        private static void KillProcess(Process processToBeKilled) {
            if (processToBeKilled != null) {
                Process[] processes = Process.GetProcessesByName("xcodebuild");
                foreach (Process process in processes) {
                    process.Kill();
                }
                processToBeKilled.Kill();
            }
        }

    }
}
