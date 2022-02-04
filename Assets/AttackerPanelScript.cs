using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AttackerPanelScript : MonoBehaviour
{

    public TMP_Dropdown startBase;
    public TMP_Dropdown targetBase;
    public Slider rocketCount;
    public TMP_Text rocketCountLabel;
    public Button startButton;

    private Dictionary<String, GameObject> rocketLauncherMap;
    private Dictionary<String, GameObject> targetMap;

    // Start is called before the first frame update
    void Start()
    {

        rocketLauncherMap = new Dictionary<string, GameObject>();
        targetMap = new Dictionary<string, GameObject>();
        
        var launchers = GameObject.FindGameObjectsWithTag("RocketLauncher");

        for (int i = 0; i < launchers.Length; i++)
        {
            rocketLauncherMap.Add($"Startrampe {i}", launchers[i]);
        } 
        
        startBase.options.Clear();
        foreach (var rocketLauncher in rocketLauncherMap.Keys)
        {
            startBase.options.Add(new TMP_Dropdown.OptionData(rocketLauncher));
        }
        
        
        var targets = GameObject.FindGameObjectsWithTag("Base");

        for (int i = 0; i < targets.Length; i++)
        {
            targetMap.Add($"Basis {i}", targets[i]);
        } 
        
        targetBase.options.Clear();
        foreach (var target in targetMap.Keys)
        {
            targetBase.options.Add(new TMP_Dropdown.OptionData(target));
        }
        
        rocketCount.onValueChanged.AddListener(value => rocketCountLabel.text = Mathf.FloorToInt(value).ToString());
        
        startButton.onClick.AddListener(Fire);
    }

    void Fire()
    {
        rocketLauncherMap.TryGetValue(startBase.options[startBase.value].text, out var launcherGameObject);

        var launcher = launcherGameObject.GetComponent<RocketLauncher>();

        targetMap.TryGetValue(targetBase.options[targetBase.value].text, out var target);
        launcher.target = target;

        StartCoroutine(launcher.Fire(Mathf.FloorToInt(rocketCount.value)));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
