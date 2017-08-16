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


}
