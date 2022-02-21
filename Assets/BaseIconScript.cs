using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BaseIconScript : MonoBehaviour {
    public GameObject iconContainer;
    public GameObject iconPrefab;

    public static readonly Dictionary<GameObject, GameObject> IconsToBaseObject = new Dictionary<GameObject, GameObject>();

    // Update is called once per frame
    private void Update()
    {
        var lastFrameIconsToBaseObjects = new Dictionary<GameObject, GameObject>(IconsToBaseObject);
        IconsToBaseObject.Clear();

        var bases = GameObject.FindGameObjectsWithTag("Base");
        foreach (var baseObject in bases)
        {
            
            var mainCamera = Camera.main;

            if (mainCamera == null)
            {
                return;
            }
            
            var position = mainCamera.WorldToScreenPoint(baseObject.transform.position);
            
            if (position.z < 0)
            {
                continue;
            }

            position.z = 0;

            GameObject icon;
            if (lastFrameIconsToBaseObjects.ContainsValue(baseObject))
            {
                icon = lastFrameIconsToBaseObjects.Keys.FirstOrDefault(key => lastFrameIconsToBaseObjects[key] == baseObject);

                if (icon != null)
                {
                    icon.transform.position = position;

                    IconsToBaseObject[icon] = baseObject;
                    lastFrameIconsToBaseObjects.Remove(icon);
                
                    continue;
                }
            }
            
            icon = Instantiate(iconPrefab, position, Quaternion.identity, iconContainer.transform);
            IconsToBaseObject[icon] = baseObject;
        }
        
        
        foreach (var icon in lastFrameIconsToBaseObjects.Keys)
        {
            Debug.Log("Destroy");
            Destroy(icon);
        }
    }
}