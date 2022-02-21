using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraResetScript : MonoBehaviour
{

    public CameraManager cameraManager;
    
    // Start is called before the first frame update
    void Start()
    {
        var button = GetComponent<Button>();

        if (button != null)
        {
            button.onClick.AddListener(cameraManager.ResetCamera);
        }
    }
}
