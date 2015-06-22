using UnityEngine;
using System.Collections;

public class RotateAround : MonoBehaviour {

	public Transform target;

	// Update is called once per frame
	void Update ()
	{
		transform.LookAt(target);
		transform.Translate(Vector3.right * Time.deltaTime);
	}
}
