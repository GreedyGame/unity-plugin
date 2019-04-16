using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdCamera : MonoBehaviour {
    public SimpleTouchPad touchPad;
    public float rotationSpeed = 10.0f;
    public float speed = 10.0F;
    public GameObject target;
    private float currentTranslation = 0.0f;
    private Vector3 point;
    // Use this for initialization
    void Start () {
        point = target.transform.position;//get target's coords
        transform.LookAt(point);//makes the camera look to it
    }

    void FixedUpdate ()
    {
        Vector2 direction = touchPad.GetDirection ();
        float translation = direction.y * speed;
        if (currentTranslation != translation) {
            currentTranslation = translation;
            transform.RotateAround (point,new Vector3(0.0f,1.0f,0.0f),  5.0f *(direction.y >= 0.0f ? 1.0f : -1.0f));
        }
    }
}
