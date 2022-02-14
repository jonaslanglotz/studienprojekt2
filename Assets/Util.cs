using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Util
{

    public static Quaternion GetQuaternionTwist(Quaternion quaternion, Vector3 projectionAxis)
    {
        quaternion.ToAngleAxis(out var angle, out var axis);
        
        var projection = Vector3.Dot(axis, projectionAxis)* projectionAxis;
        return new Quaternion(projection.x, projection.y, projection.z, quaternion.w).normalized;
    }
    
    public static List<RaycastResult> GetEventSystemRaycastResults()
    {
        var eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        var raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raycastResults);
        return raycastResults;
    }
        
}