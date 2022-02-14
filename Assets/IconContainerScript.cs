using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconContainerScript : MonoBehaviour
{
    public GameObject iconContainer;
    
    void Update()
    {
        iconContainer.SetActive(CameraManager.IsMainCamera());
    }
}
