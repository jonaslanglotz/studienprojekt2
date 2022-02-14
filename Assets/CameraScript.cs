using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        
        var raycastResults = Util.GetEventSystemRaycastResults();
            
        if (raycastResults.Count == 0)
        {
            CameraManager.ResetCamera();
        }
    }
}