using UnityEngine;
using System.Collections;
using GreedyGame.Runtime;
using UnityEngine.SceneManagement;

public class RefreshScene : MonoBehaviour {

	string floatUnitId = "float-2014";
	Texture btnTexture;

	void Start () {
		GreedyGameAgent.Instance.getFloatUnitTexture (floatUnitId, delegate(string unitId,Texture2D texture) {
			btnTexture = texture;
		});
        GreedyGameAgent.Instance.showEngagementWindow("float-2014");
	}
	
	void OnGUI() {
			if (GUI.Button (new Rect (0, 0, 200, 100), "Reload")) {
            SceneManager.LoadScene(1);
				Debug.Log ("Clicked the button with text");
			}
		}
}
