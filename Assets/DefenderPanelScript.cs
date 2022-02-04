using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DefenderPanelScript : MonoBehaviour
{

    public TMP_Dropdown BaseDropdown;
    public Slider rocketCountSlider;
    public TMP_Text rocketCountLabel;
    public Button startButton;
    
    private Dictionary<String, GameObject> baseMap;
    
    // Start is called before the first frame update
    void Start()
    {
        baseMap = new Dictionary<string, GameObject>();
        
        var bases = GameObject.FindGameObjectsWithTag("Base");

        for (int i = 0; i < bases.Length; i++)
        {
            baseMap.Add($"Startrampe {i}", bases[i]);
        } 
        
        BaseDropdown.options.Clear();
        foreach (var startBase in baseMap.Keys)
        {
            BaseDropdown.options.Add(new TMP_Dropdown.OptionData(startBase));
        }
        
        rocketCountSlider.onValueChanged.AddListener(value => rocketCountLabel.text = Mathf.FloorToInt(value).ToString());
        
        startButton.onClick.AddListener(Fire);
        
    }
    
    void Fire()
    {
        baseMap.TryGetValue(BaseDropdown.options[BaseDropdown.value].text, out var baseGameObject);

        var launcher = baseGameObject.transform.GetComponentInChildren<IronDomeBattery>();

        StartCoroutine(launcher.Fire(Mathf.FloorToInt(rocketCountSlider.value)));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
