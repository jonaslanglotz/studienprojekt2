using UnityEngine;

public class CameraScript : MonoBehaviour
{

    public CameraManager cameraManager;
    private void Update()
    {
        if (!Input.GetMouseButtonDown(0))
        {
            return;
        }
        
        var results = Util.GetEventSystemRaycastResults();
        
        Debug.Log(results.Count);
        
        foreach (var raycastResult in results)
        {
            var hit = raycastResult.gameObject;
            
            if (hit.CompareTag("BaseIcon"))
            {
                var icon = BaseIconScript.IconsToBaseObject.TryGetValue(hit, out var obj);
                cameraManager.SetFocus(obj);
            }
            
            if (hit.CompareTag("RocketLauncherIcon"))
            {
                var icon = RocketLauncherIconScript.IconsToRocketLauncherObject.TryGetValue(hit, out var obj);
                cameraManager.SetFocus(obj);
            }
        }
    }
}