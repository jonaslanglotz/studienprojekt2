using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleDay : MonoBehaviour
{

    public GameObject sun;
    public GameObject moon;

    // Start is called before the first frame update
    void Start()
    {
        var button = GetComponent<Button>();

        if (button != null)
        {
            button.onClick.AddListener(Toggle);
        }
    }

    private void Toggle()
    {
        sun.SetActive(!sun.activeSelf);
        moon.SetActive(!moon.activeSelf);
    }

}
