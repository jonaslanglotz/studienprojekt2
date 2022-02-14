using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RocketLauncherIconScript:MonoBehaviour
{
        
    public GameObject IconContainer;
    public GameObject IconPrefab;
    
    public static Dictionary<GameObject, GameObject> IconsToRocketLauncherObject = new Dictionary<GameObject, GameObject>();

    // Update is called once per frame
    void Update()
    {
        var lastFrameIconsToRocketLauncherObjects = new Dictionary<GameObject, GameObject>(IconsToRocketLauncherObject);
        IconsToRocketLauncherObject.Clear();

        var rocketLaunchers = GameObject.FindGameObjectsWithTag("RocketLauncher");
        foreach (var rocketLauncher in rocketLaunchers)
        {

            var mainCamera = Camera.main;

            if (mainCamera == null)
            {
                return;
            }
            
            var position = mainCamera.WorldToScreenPoint(rocketLauncher.transform.position);
            position.z = 0;

            GameObject icon = null;
            if (lastFrameIconsToRocketLauncherObjects.ContainsValue(rocketLauncher))
            {
                icon = lastFrameIconsToRocketLauncherObjects.Keys.FirstOrDefault(key => lastFrameIconsToRocketLauncherObjects[key] == rocketLauncher);

                if (icon != null)
                {
                    icon.transform.position = position;

                    IconsToRocketLauncherObject[icon] = rocketLauncher;
                    lastFrameIconsToRocketLauncherObjects.Remove(icon);
                
                    continue;
                }
            }
            
            icon = Instantiate(IconPrefab, position, Quaternion.identity, IconContainer.transform);
            IconsToRocketLauncherObject[icon] = rocketLauncher;
        }
        
        foreach (var icon in lastFrameIconsToRocketLauncherObjects.Keys)
        {
            Destroy(icon);
        }
    }
}