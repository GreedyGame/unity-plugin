using UnityEngine;
using System.Collections;
using GreedyGame.Runtime;
using GreedyGame.Runtime.Units;
using GreedyGame.Commons;

public class ButtonCarScene : MonoBehaviour {

	
	Texture nativeTexture,floatTexture;


	void Start () {
        GreedyGameAgent.Instance.setListener(new GreedyAgentListener());
	}
	
	    void OnGUI() {
			if (GUI.Button (new Rect (100 , 100,150,50 ), "INIT")) {
				Debug.Log ("GG[ButtonCarScene] INIT called ");
                GreedyGameAgent.Instance.init(new GreedyAgentListener()); 
			}

        if (GUI.Button(new Rect(300, 100, 150, 50), "SHOWFLOAT"))
        {
            Debug.Log("GG[ButtonCarScene] SHOWFLOAT called with id float-1877 ");
            GreedyGameAgent.Instance.fetchFloatUnit("float-1877");
        }

        if (GUI.Button(new Rect(500, 100, 150, 50), "REMOVEFLOAT"))
        {
            Debug.Log("GG[ButtonCarScene] REMOVEFLOAT called with id float-1877 ");
            GreedyGameAgent.Instance.removeFloatUnit("float-1877");
        }

        if (GUI.Button(new Rect(100, 170, 150, 50), "REMOVEALL"))
        {
            Debug.Log("GG[ButtonCarScene] fetching 1877 and after calling REMOVEALLFLOATS ");
            GreedyGameAgent.Instance.fetchFloatUnit("float-1877");
            GreedyGameAgent.Instance.removeAllFloatUnits();
        }

        if (GUI.Button(new Rect(300, 170, 150, 50), "SHOWUII"))
        {
            Debug.Log("GG[ButtonCarScene] calling SHOWENGAGEMENTWINDOW with id : 1877 ");
            GreedyGameAgent.Instance.showEngagementWindow("float-1877");
        }

        if (GUI.Button(new Rect(500, 170, 150, 50), "GETNATIVETEXTURE"))
        {
            Debug.Log("GG[ButtonCarScene] calling GETNATIVETEXTURE with id : 2334 ");
            GreedyGameAgent.Instance.getNativeUnitTexture("unit-2334", delegate (string unitID, Texture2D texture)
             {
                 if(unitID.Equals("unit-2334"))
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
            Debug.Log("GG[ButtonCarScene] calling GETFLOATTEXTURE with id : 1877 ");
            GreedyGameAgent.Instance.getFloatUnitTexture("float-1877", delegate (string unitID, Texture2D texture)
            {
                if (unitID.Equals("float-1877"))
                {
                    Debug.Log("GG[ButtonCarScene] callback to delegate after getNativeTexture success for float 1877");
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



    }






    public class GreedyAgentListener : IAgentListener
    {

        public void onAvailable(string campaignId)
        {
            /**
         * TODO: New campaign is available and ready to use for the next scene.
         **/
            Debug.Log("Inside onAvailable function");
            refreshNativeUnits();
            refreshFloatUnits();

            Debug.Log("GG[ButtonCarScene] onAvailable calling refreshUnitsWith IdentifierScript (floor textures");
            refreshNativeUnitsWithIdentifierScript();
            refreshFloatUnitsWithIdentifierScript();

        }

        public void onUnavailable()
        {
            /**
         * TODO: No campaign is available, proceed with normal follow of the game.
         **/
            refreshNativeUnits();
            refreshFloatUnits();
            Debug.Log("GG[ButtonCarScene] onUnavailable calling refreshUnitsWith IdentifierScript (floor textures");
            refreshNativeUnitsWithIdentifierScript();
            refreshFloatUnitsWithIdentifierScript();
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

        //see the following functions for reference for refreshing native and float units
        //and the implementation of the same should be done inside this function.
        //refreshNativeUnits();
        //refreshFloatUnits();
        private void refreshNativeUnits()
        {
            NativeUnit[] nativeScriptObjects = FindObjectsOfType(typeof(NativeUnit)) as NativeUnit[];
            foreach (NativeUnit nativeUnit in nativeScriptObjects)
            {
                nativeUnit.GG_SetUpTexture();
            }

            SharedNativeUnit[] sharedScriptObjects = FindObjectsOfType(typeof(SharedNativeUnit)) as SharedNativeUnit[];
            foreach (SharedNativeUnit sharedUnit in sharedScriptObjects)
            {
                sharedUnit.GG_SetUpTexture();
            }


        }

        private void refreshFloatUnits()
        {
            GreedyGameAgent.Instance.removeAllFloatUnits();
            GreedyGameAgent.Instance.fetchFloatUnit("float-1877");
            //replace with your float unit id
            //GreedyGameAgent.Instance.fetchFloatUnit("your float unit id");
        }

        private void refreshNativeUnitsWithIdentifierScript()
        {
            Debug.Log("GG[ButtonCarScene] refreshNativeUnitsWithIdentifierScript ");
            GreedyGameAgent.Instance.getNativeUnitTexture("unit-2334", delegate (string unitID, Texture2D texture)
            {
                if (unitID.Equals("unit-2334"))
                {
                    Debug.Log("GG[ButtonCarScene] callback to delegate after getNativeTexture success for unit-2334");
                    NativeIdentifier[] nativeIdentifier = FindObjectsOfType(typeof(NativeIdentifier)) as NativeIdentifier[];
                    foreach (NativeIdentifier nativeUnit in nativeIdentifier)
                    {
                        nativeUnit.updateTexture(texture);
                    }
                }
            });
        }


        private void refreshFloatUnitsWithIdentifierScript()
        {
            Debug.Log("GG[ButtonCarScene] refreshFloatsUnitsWithIdentifierScript ");
            GreedyGameAgent.Instance.getFloatUnitTexture("float-1877", delegate (string unitID, Texture2D texture)
            {
                if (unitID.Equals("float-1877"))
                {
                    Debug.Log("GG[ButtonCarScene] callback to delegate after getNativeTexture success for float-1877");
                    FloatIdentifier[] floatIdentifier = FindObjectsOfType(typeof(FloatIdentifier)) as FloatIdentifier[];
                    foreach (FloatIdentifier floatUnit in floatIdentifier)
                    {
                        floatUnit.updateTexture(texture);
                    }
                }
            });
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
