using UnityEngine;
using System.Collections;
using GreedyGame.Runtime;

public class ButtonCreator : MonoBehaviour {

	string floatUnitId = "float-2014";
	Texture btnTexture;

	void Start () {
		GreedyGameAgent.Instance.getFloatUnitTexture (floatUnitId, delegate(string unitId,Texture2D texture) {
			btnTexture = texture;
		});
        GreedyGameAgent.Instance.showEngagementWindow("float-2014");
	}
	
	void OnGUI() {
		if (btnTexture) {
			if (GUI.Button (new Rect (Screen.width-250, 0, 200, 100), btnTexture)) {
				GreedyGameAgent.Instance.showEngagementWindow (floatUnitId);
				Debug.Log ("Clicked the button with an image");
			}
		} else {
			if (GUI.Button (new Rect (Screen.width-250, 0, 200, 100), "Click")) {
				Debug.Log ("Clicked the button with text");
			}
		}
	}
}
