using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using GreedyGame.Runtime;
using GreedyGame.Platform;
using GreedyGame.Runtime.Units;
using GreedyGame.Commons;

public class GreedyCampaignLoader : SingletoneBase<GreedyCampaignLoader>
{

    public List<string> unitList;

    public bool AdMobMediation = false;

    public bool MoPubMediation = false;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (RuntimePlatform.Android == Application.platform)
        {
            GGAdConfig adConfig = new GGAdConfig();
            adConfig.setListener(new GreedyAgentListener());
            adConfig.enableAdmobMediation(AdMobMediation);
            adConfig.enableMopubMediation(MoPubMediation);
            adConfig.addUnitList(unitList);
            GreedyGameAgent.Instance.init(adConfig);
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

    public class GreedyAgentListener : IAgentListener
    {

        public void onAvailable(string campaignId)
        {
            /**
         * TODO: New campaign is available and ready to use for the next scene.
         **/
            moveToNextScene();

        }

        public void onUnavailable()
        {
            /**
         * TODO: No campaign is available, proceed with normal flow of the game.
         **/
            moveToNextScene();
        }

        public void onFound()
        {
            /**
         * TODO: Campaign is found. Starting download of assets. This will be followed by onAvailable callback once download completes successfully.
         **/
        }

        public void onError(string error)
        {
            /**
         * TODO: No Campaign will be served since the initialization resulted in an error. 
         * If device api level is below 15 this callback is invoked.
         **/
        }

    }

    public static void showFloat(String f_id)
    {
        Debug.Log(String.Format("Fetching FloatUnit {0}", f_id));
        GreedyGameAgent.Instance.fetchFloatUnit(f_id);
    }

    public static void removeFloatAd(string FloatUnit)
    {
        Debug.Log(String.Format("Remove FloatUnit"));
        GreedyGameAgent.Instance.removeFloatUnit(FloatUnit);
    }

    public static void removeAllFloatAds()
    {
        Debug.Log(String.Format("Remove AllFloatUnits"));
        GreedyGameAgent.Instance.removeAllFloatUnits();

    }

}
