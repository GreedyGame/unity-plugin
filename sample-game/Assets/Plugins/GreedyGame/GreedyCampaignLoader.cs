using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using GreedyGame.Runtime;
using GreedyGame.Platform;
using GreedyGame.Commons;

public class GreedyCampaignLoader : MonoBehaviour
{

    public List<GGUnitConfig> unitList;

    public bool AdMobMediation = false;

    public bool MoPubMediation = false;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (RuntimePlatform.Android == Application.platform || RuntimePlatform.IPhonePlayer == Application.platform)
        {
            GGConfig adConfig = new GGConfig();
            adConfig.SetAppId("14435775");
            adConfig.SetAdListener(new GreedyAgentListener());
            adConfig.EnableAdmobMediation(AdMobMediation);
            adConfig.EnableMopubMediation(MoPubMediation);
            //adConfig.addUnitList(unitList);
            GreedyGameAgent.Instance.Load(adConfig);
        }
        else
        {
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

    public class GreedyAgentListener : GGAdListener
    {

        public void onAvailable(string campaignId)
        {
            /**
         * TODO: New campaign is available and ready to use for the next scene.
         **/
            Debug.Log("GreedyAgentListener onAvailable");
            moveToNextScene();
        }

        public void onUnavailable()
        {
            /**
         * TODO: No campaign is available, proceed with normal flow of the game.
         **/
            Debug.Log("GreedyAgentListener onUnavailable");
            moveToNextScene();
        }

        public void onError(string error)
        {
            /**
         * TODO: No Campaign will be served since the initialization resulted in an error. 
         * If device api level is below 15 this callback is invoked.
         **/
            Debug.Log("GreedyAgentListener onError");
        }

    }
}
