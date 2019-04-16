using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class SimpleTouchPad : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler {

    public float smoothing;

    private Vector2 origin;
    private Vector2 direction;
    private Vector2 smoothDirection;
    private bool touched;
    private int pointerId;

    void Awake () {
        direction = Vector2.zero;
        touched = false;
    }

    public void OnPointerDown (PointerEventData data) {
        if (!touched) {
            // Set our start poing
            touched = true;
            pointerId = data.pointerId;
            origin = data.position;
        }
    }

    public void OnPointerUp (PointerEventData data) {
        if (pointerId == data.pointerId) {
            // Reset everything
            direction = Vector2.zero;
            touched = false;
        }
    }

    public void OnDrag (PointerEventData data) {
        if (pointerId == data.pointerId) {
            // Compare the difference between our start point and current pointer pos
            Vector2 currentPosition = data.position;
            Vector2 directionRaw = currentPosition - origin;
            direction = directionRaw.normalized;
            Debug.Log (direction);
        }
    }

    public Vector2 GetDirection () {
        smoothDirection = Vector2.MoveTowards (smoothDirection, direction, smoothing);
        return smoothDirection;
    }
}
