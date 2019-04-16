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
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace AudienceNetwork.Editor
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using UnityEditor;
    using UnityEngine;

    public class AudienceNetworkBuild
    {
        public const string AudienceNetworkPath = "Assets/AudienceNetwork/";
        public const string AudienceNetworkPluginsPath = AudienceNetworkPath + "Plugins/";
        public const string AudienceNetworkPluginiOSPath = AudienceNetworkPluginsPath + "iOS/libs/FBAudienceNetwork.framework/FBAudienceNetwork";
        public const string AudienceNetworkPluginAndroidPath = AudienceNetworkPluginsPath + "Android/libs/ads-release.aar";
        public static string PluginsPath = "Assets/Plugins/";

        public enum Target
        {
            DEBUG,
            RELEASE
        }

        private static string PackageName
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    "audience-network-unity-sdk-{0}.unitypackage",
                    SdkVersion.Build);
            }
        }

        private static string OutputPath
        {
            get
            {
                DirectoryInfo projectRoot = Directory.GetParent(Directory.GetCurrentDirectory());
                var outputDirectory = new DirectoryInfo(Path.Combine(projectRoot.FullName, "out"));

                // Create the directory if it doesn't exist
                outputDirectory.Create();
                return Path.Combine(outputDirectory.FullName, AudienceNetworkBuild.PackageName);
            }
        }

        // Exporting the *.unityPackage for Asset store
        public static string ExportPackage ()
        {
            Debug.Log ("Exporting Audience Network Unity Package...");

            // Check that SDKs are built
            bool iOSFound = File.Exists(AudienceNetworkPluginiOSPath);
            bool androidFound = File.Exists(AudienceNetworkPluginAndroidPath);
            if (!iOSFound || !androidFound) {
                Debug.Log ("Exporting failed, no AN SDK build found. Found SDKS - iOS: " +  iOSFound + " Android: " + androidFound);
                throw new FileNotFoundException();
            }

            var path = AudienceNetworkBuild.OutputPath;

            try {
                AssetDatabase.DeleteAsset (PluginsPath + "Android/AndroidManifest.xml");
                AssetDatabase.DeleteAsset (PluginsPath + "Android/AndroidManifest.xml.meta");
                AssetDatabase.DeleteAsset (AudienceNetworkPluginsPath + "Android/AndroidManifest.xml");
                AssetDatabase.DeleteAsset (AudienceNetworkPluginsPath + "Android/AndroidManifest.xml.meta");

                string[] facebookFiles = (string[])Directory.GetFiles (AudienceNetworkPath, "*.*", SearchOption.AllDirectories);
                string[] pluginsFiles = (string[])Directory.GetFiles (AudienceNetworkPluginsPath, "*.*", SearchOption.AllDirectories);
                string[] files = new string[facebookFiles.Length + pluginsFiles.Length];

                facebookFiles.CopyTo (files, 0);
                pluginsFiles.CopyTo (files, facebookFiles.Length);

                AssetDatabase.ExportPackage (
                    files,
                    path,
                    ExportPackageOptions.IncludeDependencies | ExportPackageOptions.Recurse);
            } finally {
                // regenerate the manifest
                AudienceNetwork.Editor.ManifestMod.GenerateManifest ();
            }
            Debug.Log ("Finished exporting!");
            return path;
        }

        public delegate void SDKBuildCallback(bool success, string version, string message, string buildOutput, string buildError);

        public static IEnumerable<SDKBuildStatus> RunSDKBuild(string version, bool skipBuild, SDKBuildCallback callback)
        {
            DirectoryInfo projectRoot = Directory.GetParent(Directory.GetCurrentDirectory());
            string workingDirectory = Path.Combine(projectRoot.FullName, "ads/scripts/");
            string script = Path.Combine(workingDirectory, "build_distribution.sh");
            string scriptPlusArgs = script + " -v " + version;
            if (skipBuild) {
                scriptPlusArgs = scriptPlusArgs + " -s";
            }

            ProcessStartInfo processStartInfo = new ProcessStartInfo("sh", scriptPlusArgs);
            processStartInfo.RedirectStandardOutput = true;
            processStartInfo.RedirectStandardError = true;
            processStartInfo.UseShellExecute = false;
            processStartInfo.WorkingDirectory = workingDirectory;

            StringBuilder outputBuilder = new StringBuilder();
            StringBuilder errorBuilder = new StringBuilder();
            List<string> outputList = new List<string>();

            Process process = new Process();
            process.StartInfo = processStartInfo;
            process.EnableRaisingEvents = true;

            outputBuilder.Append("Build Starting...\n");
            yield return new SDKBuildStatus(true, outputList, process);

            using (AutoResetEvent outputWaitHandle = new AutoResetEvent(false))
            using (AutoResetEvent errorWaitHandle = new AutoResetEvent(false))
            {
                process.OutputDataReceived += (sender, e) => {
                    if (e.Data == null) {
                        outputWaitHandle.Set();
                    } else {
                        outputBuilder.AppendLine(e.Data);
                        outputList.Add(e.Data);
                    }
                };
                process.ErrorDataReceived += (sender, e) => {
                    if (e.Data == null) {
                        errorWaitHandle.Set();
                    } else {
                        errorBuilder.AppendLine(e.Data);
                    }
                };

                bool exited = false;

                process.Exited += (sender, e) => {
                    string output = outputBuilder.ToString();
                    string error = errorBuilder.ToString();
                    bool success = (process.ExitCode == 0);
                    string message = success ? "Completed successfully." : "Build script returned an error, check console log.";
                    exited = true;
                    callback(success, version, message, output, error);
                };

                process.Start();

                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                while (true) {
                    if (!exited) {
                        yield return new SDKBuildStatus(true, outputList, process);
                    } else {
                        yield break;
                    }
                }
            }
        }
    }

    public class SDKBuildStatus
    {
        public SDKBuildStatus(bool BuildInProgress, List<string> CurrentLogOutput, Process process)
        {
            this.BuildInProgress = BuildInProgress;
            this.CurrentLogOutput = CurrentLogOutput;
            this.process = process;
        }

        public bool BuildInProgress;
        public List<string> CurrentLogOutput;
        public Process process;
    }
}
