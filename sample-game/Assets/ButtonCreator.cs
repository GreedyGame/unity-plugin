using UnityEngine;
using System.Collections;
using GreedyGame.Runtime;

public class ButtonCreator : MonoBehaviour {

	string floatUnitId = "float-1271";
	Texture btnTexture;

	void Start () {
		GreedyGameAgent.Instance.getFloatUnitTexture (floatUnitId, delegate(string unitId,Texture2D texture) {
			btnTexture = texture;
		});
	}
	
	void OnGUI() {
		if (btnTexture) {
			if (GUI.Button (new Rect (Screen.width-250, 0, 200, 100), btnTexture)) {
				GreedyGameAgent.Instance.showUII (floatUnitId);
				Debug.Log ("Clicked the button with an image");
			}
		} else {
			if (GUI.Button (new Rect (Screen.width-250, 0, 200, 100), "Click")) {
				Debug.Log ("Clicked the button with text");
			}
		}
	}
}
