using System;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject focusedObject;
    public float distance;
    public Vector2 direction;
    public Vector3 offset;
    public float sensitivity;

    private float DEFAULT_DISTANCE = 50000;
    private Vector2 DEFAULT_DIRECTION = new Vector2(45,30);
    
    private Vector2 _lastMousePosition;
    private Vector3 _lastFocusPosition;

    private GameObject initialFocus;

    private void Start()
    {
        initialFocus = focusedObject;
    }

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

            if (Input.GetKey(KeyCode.LeftShift))
            {
                var drag = Quaternion.Euler(direction.y, direction.x, 0) * new Vector3(mouseDelta.x, mouseDelta.y);
                offset -= distance * 0.01f * drag;
            }
            else
            {
                direction = new Vector2(direction.x + mouseDelta.x, Mathf.Clamp(direction.y - mouseDelta.y, -90, 90));
            }

        }
        _lastMousePosition = Input.mousePosition;
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
        var focusedObjectPosition = focusedObject != null ? focusedObject.transform.position : _lastFocusPosition;

        Vector3 focusPosition;
        if (focusedObject != null && focusedObject.TryGetComponent<Rigidbody>(out _))
        {
            var lerpFactor = 0.1f + Mathf.Exp(-(((focusedObjectPosition - _lastFocusPosition).magnitude / 10) - 10) );
            focusPosition = Vector3.Lerp(_lastFocusPosition, focusedObjectPosition, lerpFactor);
        }
        else
        {
            focusPosition = Vector3.Lerp(_lastFocusPosition, focusedObjectPosition, 0.1f);
        }
        

        var distanceVector =  CalculateRotation() * Vector3.back * distance;
        var position = focusPosition + distanceVector + offset;

        _lastFocusPosition = focusPosition;
        
        return position;
    }

    public void SetFocus(GameObject focus)
    {
        focusedObject = focus;
        offset = Vector3.zero;
    }

    public void ResetCamera()
    {
        distance = DEFAULT_DISTANCE;
        direction = DEFAULT_DIRECTION;
        offset = Vector3.zero;
        focusedObject = initialFocus;
    }
}
