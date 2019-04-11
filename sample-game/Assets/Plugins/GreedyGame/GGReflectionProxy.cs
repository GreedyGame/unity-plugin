using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

//for IL2CPP internal engine code stripping to avoid stripping of classes.
public class GGReflectionProxy : MonoBehaviour {

    private static string TEXTURE_PATH = "";

    // Use this for initialization
    void Start()
    {
#if UNITY_5_4_OR_NEWER
        UnityWebRequest request = new UnityWebRequest();
        DownloadHandlerTexture.GetContent(request);
#endif

#if UNITY_2017_2_OR_NEWER
        UnityWebRequestTexture.GetTexture(TEXTURE_PATH);
#endif
    }

    // Update is called once per frame
    void Update () {
		
	}
}
