using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class RocketIconScript : MonoBehaviour
{
    public GameObject IconContainer;
    public GameObject IconPrefab;

    public static Dictionary<GameObject, GameObject> IconsToRocketObject = new Dictionary<GameObject, GameObject>();

    // Update is called once per frame
    void Update()
    {
        var lastFrameIconsToRocketObjects = new Dictionary<GameObject, GameObject>(IconsToRocketObject);
        IconsToRocketObject.Clear();

        var rockets = GameObject.FindGameObjectsWithTag("Rocket");
        foreach (var rocket in rockets)
        {
            
            var mainCamera = Camera.main;

            if (mainCamera == null)
            {
                return;
            }
            
            var position = mainCamera.WorldToScreenPoint(rocket.transform.position);
            
            if (position.z < 0)
            {
                continue;
            }
            
            position.z = 0;
            
            GameObject icon = null;
            if (lastFrameIconsToRocketObjects.ContainsValue(rocket))
            {
                icon = lastFrameIconsToRocketObjects.Keys.FirstOrDefault(key => lastFrameIconsToRocketObjects[key] == rocket);

                
                if (icon != null)
                {
                    icon.transform.position = position;

                    IconsToRocketObject[icon] = rocket;
                    lastFrameIconsToRocketObjects.Remove(icon);
                
                    continue;
                }
            }
            
            icon = Instantiate(IconPrefab, position, Quaternion.Euler(0,0 , 0), IconContainer.transform);
            IconsToRocketObject[icon] = rocket;
        }
        
        foreach (var icon in lastFrameIconsToRocketObjects.Keys)
        {
            Debug.Log("Destroy");
            Destroy(icon);
        }
    }
}