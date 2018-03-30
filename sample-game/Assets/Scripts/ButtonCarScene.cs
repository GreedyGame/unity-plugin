using UnityEngine;
using System.Collections;
using GreedyGame.Runtime;
using GreedyGame.Runtime.Units;
using GreedyGame.Commons;
using System;
using UnityEngine.SceneManagement;

public class ButtonCarScene : MonoBehaviour {

	
	Texture nativeTexture,floatTexture;


	void Start () {
        GreedyGameAgent.Instance.setListener(new GreedyAgentListener());
	}
	
	    void OnGUI() {
			if (GUI.Button (new Rect (100 , 100,150,50 ), "INIT")) {
				Debug.Log ("GG[ButtonCarScene] INIT called ");
            GreedyGameAgent.Instance.enableAdmobMediation(true);
                GreedyGameAgent.Instance.init(new GreedyAgentListener()); 
			}

        if (GUI.Button(new Rect(300, 100, 150, 50), "SHOWFLOAT"))
        {
            Debug.Log("GG[ButtonCarScene] SHOWFLOAT called with id float-3160 ");
            GreedyGameAgent.Instance.fetchFloatUnit("float-3804");
        }

        if (GUI.Button(new Rect(500, 100, 150, 50), "REMOVEFLOAT"))
        {
            Debug.Log("GG[ButtonCarScene] REMOVEFLOAT called with id float-1877 ");
            GreedyGameAgent.Instance.removeFloatUnit("float-3804");
        }

        if (GUI.Button(new Rect(100, 170, 150, 50), "REMOVEALL"))
        {
            Debug.Log("GG[ButtonCarScene] fetching 3160 and after calling REMOVEALLFLOATS ");
            //GreedyGameAgent.Instance.fetchFloatUnit("float-3804");
            GreedyGameAgent.Instance.removeAllFloatUnits();
        }

        if (GUI.Button(new Rect(300, 170, 150, 50), "SHOWUII"))
        {
            Debug.Log("GG[ButtonCarScene] calling SHOWENGAGEMENTWINDOW with id : 3804 ");
            GreedyGameAgent.Instance.showEngagementWindow("float-3804");
        }

        if (GUI.Button(new Rect(500, 170, 150, 50), "GETNATIVETEXTURE"))
        {
            Debug.Log("GG[ButtonCarScene] calling GETNATIVETEXTURE with id : 2334 ");
            GreedyGameAgent.Instance.getNativeUnitTexture("unit-3804", delegate (string unitID, Texture2D texture)
             {
                 if(unitID.Equals("unit-3804"))
                 {
                     Debug.Log("GG[ButtonCarScene] callback to delegate after getNativeTexture success for unit-2334");
                     NativeIdentifier[] nativeIdentifier = FindObjectsOfType(typeof(NativeIdentifier)) as NativeIdentifier[];
                     foreach (NativeIdentifier nativeUnit in nativeIdentifier)
                     {
                         Debug.Log("GG[ButtonCarScene] update texture inside loop ");
                         nativeUnit.updateTexture(texture);
                     }
                 }
             });
        }

        if (GUI.Button(new Rect(100, 240, 150, 50), "GETFLOATTEXTURE"))
        {
            Debug.Log("GG[ButtonCarScene] calling GETFLOATTEXTURE with id : 3160 ");
            GreedyGameAgent.Instance.getFloatUnitTexture("float-3804", delegate (string unitID, Texture2D texture)
            {
                if (unitID.Equals("float-3804"))
                {
                    Debug.Log("GG[ButtonCarScene] callback to delegate after getNativeTexture success for float 3160");
                    FloatIdentifier[] floatIdentifier = FindObjectsOfType(typeof(FloatIdentifier)) as FloatIdentifier[];
                    foreach (FloatIdentifier floatUnit in floatIdentifier)
                    {
                        Debug.Log("GG[ButtonCarScene] update texture inside loop ");
                        floatUnit.updateTexture(texture);
                    }
                }
            });
        }

        if (GUI.Button(new Rect(300, 240, 150, 50), "REFRESH"))
        {
            Debug.Log("GG[ButtonCarScene] calling REFRESH ");
            GreedyGameAgent.Instance.startEventRefresh();
        }

        if (GUI.Button(new Rect(300, 320, 150, 50), "LEVEL 1"))
        {
            Debug.Log("GG[ButtonCarScene] calling REFRESH ");
            SceneManager.LoadScene(1);
        }

        if (GUI.Button(new Rect(500, 320, 150, 50), "LEVEL 2"))
        {
            Debug.Log("GG[ButtonCarScene] calling REFRESH ");
            SceneManager.LoadScene(2);
        }

        if (GUI.Button(new Rect(500, 240, 150, 50), "CRASH"))
        {
            try
            {
                Debug.Log("GG[ButtonCarScene] calling REFRESH ");
                NullReferenceException exception = new NullReferenceException("GG CUSTOM CRASH FOR TESTING");
                throw exception;
            } catch (NullReferenceException ex)
            {
                GreedyGameAgent.Instance.sendCrashReport(ex.Message + ex.StackTrace, false);
                Debug.Log("GG[CRASH] Message : " + ex.Message);
                Debug.Log("GG[CRASH] stacktrace : " + ex.StackTrace);
            }
        }



    }






    public class GreedyAgentListener : IAgentListener
    {

        public void onAvailable(string campaignId)
        {
            /**
         * TODO: New campaign is available and ready to use for the next scene.
         **/
            Debug.Log("Inside onAvailable function");

            Debug.Log("GG[ButtonCarScene] onAvailable calling refreshUnitsWith IdentifierScript (floor textures");

        }

        public void onUnavailable()
        {
            /**
         * TODO: No campaign is available, proceed with normal follow of the game.
         **/
            Debug.Log("GG[ButtonCarScene] onUnavailable calling refreshUnitsWith IdentifierScript (floor textures");
        }

        public void onProceed()
        {
            /**
         * TODO: Campaign is found. Starting download of assets. This will be followed by onAvailable callback once download completes successfully.
         **/
          
        }

        public void onFound()
        {
            /**
         * TODO: Campaign is found. Starting download of assets. This will be followed by onAvailable callback once download completes successfully.
         **/
            //moveToNextScene();
        }

        public void onProgress(int progress)
        {
            /**
         * TODO: Campaign progress from 1-100.
         **/
        }

        public void onError(string error)
        {
            /**
         * TODO: No Campaign will be served since the initialization resulted in an error. 
         * If device api level is below 15 this callback is invoked.
         **/
            moveToNextScene();
        }
        
    }


    private static void moveToNextScene()
    {
        if (Application.loadedLevel == 0)
        {
            Application.LoadLevel(1);
        }
    }
}
