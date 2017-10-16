using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GreedyGame.Runtime;
using GreedyGame.Runtime.Units;

public class LevelChange : MonoBehaviour {

    public int level;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnGUI()
    {
        if (GUI.Button(new Rect(Screen.width-200, Screen.height-150, 150, 70), "CHANGE LEVEL"))
        {
            Debug.Log("GG[LevelChange]  loadlevel " + level);
            GreedyGameAgent.Instance.removeListener();
            SceneManager.LoadScene(level);
        }
    }
}
