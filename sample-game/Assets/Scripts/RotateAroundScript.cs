using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAroundScript : MonoBehaviour {

    public GameObject gameObject;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.RotateAround(gameObject.transform.position, Vector3.up, 20 * Time.deltaTime);
	}
}
