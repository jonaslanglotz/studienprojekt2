using System;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject focusedObject;
    public GameObject defaultFocusedObject;
    public float distance;
    public Vector2 direction;
    public Vector3 offset;
    public float sensitivity;

    private float DEFAULT_DISTANCE = 50000;
    private Vector2 DEFAULT_DIRECTION = new Vector2(45,30);
    
    private Vector2 _lastMousePosition;

    private void Update()
    {

        ApplyInput();
        
        transform.position = CalculatePosition();
        transform.rotation = CalculateRotation();
    }

    private void ApplyInput()
    {
        HandleDrag();
        HandleScroll();
    }

    private void HandleDrag()
    {
        if (Input.GetMouseButton(2))
        {
            Vector2 mousePosition = Input.mousePosition;

            var mouseDelta = (mousePosition - _lastMousePosition) * sensitivity;

            direction = new Vector2(direction.x + mouseDelta.x, Mathf.Clamp(direction.y - mouseDelta.y, -90, 90));

            _lastMousePosition = mousePosition;
        }
        else
        {
            _lastMousePosition = Input.mousePosition;
        }
    }

    private void HandleScroll()
    {
        distance = Mathf.Clamp(distance - distance * 0.1f * Input.mouseScrollDelta.y, 0.1f, 80000f);
    }

    private Quaternion CalculateRotation()
    {
        return Quaternion.Euler(direction.y, direction.x, 0);
    }

    private Vector3 CalculatePosition()
    {
        var focusedObjectPosition = focusedObject != null ? focusedObject.transform.position : defaultFocusedObject.transform.position;

        var distanceVector =  CalculateRotation() * Vector3.back * distance;
        var position = focusedObjectPosition + distanceVector + offset;
        
        return position;
    }

    public void SetFocus(GameObject gameObject)
    {
        focusedObject = gameObject;
    }

    public void ResetCamera()
    {
        distance = DEFAULT_DISTANCE;
        direction = DEFAULT_DIRECTION;
        focusedObject = null;
    }
}
