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
    
    public Button startCameraButton;
    public Button targetCameraButton;

    private Dictionary<string, GameObject> _rocketLauncherMap;
    private Dictionary<string, GameObject> _targetMap;

    // Start is called before the first frame update
    private void Start()
    {

        _rocketLauncherMap = new Dictionary<string, GameObject>();
        _targetMap = new Dictionary<string, GameObject>();
        
        var launchers = GameObject.FindGameObjectsWithTag("RocketLauncher");

        for (var i = 0; i < launchers.Length; i++)
        {
            _rocketLauncherMap.Add($"Startrampe {i+1}", launchers[i]);
        } 
        
        startBase.options.Clear();
        foreach (var rocketLauncher in _rocketLauncherMap.Keys)
        {
            startBase.options.Add(new TMP_Dropdown.OptionData(rocketLauncher));
        }
        
        
        var targets = GameObject.FindGameObjectsWithTag("Base");

        for (var i = 0; i < targets.Length; i++)
        {
            _targetMap.Add($"Basis {i+1}", targets[i]);
        } 
        
        targetBase.options.Clear();
        foreach (var target in _targetMap.Keys)
        {
            targetBase.options.Add(new TMP_Dropdown.OptionData(target));
        }
        
        rocketCount.onValueChanged.AddListener(value => rocketCountLabel.text = Mathf.FloorToInt(value).ToString());
        
        startButton.onClick.AddListener(Fire);
        
        startCameraButton.onClick.AddListener(SetStartCamera);
        targetCameraButton.onClick.AddListener(SetTargetCamera);
    }

    private void SetStartCamera()
    {
        _rocketLauncherMap.TryGetValue(startBase.options[startBase.value].text, out var launcherGameObject);

        if (launcherGameObject == null)
        {
            return;
        }

        var startCamera = launcherGameObject.GetComponentInChildren<Camera>();
        
        CameraManager.SetCamera(startCamera);
    }

    private void SetTargetCamera()
    {
        _targetMap.TryGetValue(targetBase.options[targetBase.value].text, out var targetGameObject);
        
        if (targetGameObject == null)
        {
            return;
        }

        var targetCamera = targetGameObject.GetComponentInChildren<Camera>();
        
        CameraManager.SetCamera(targetCamera);
    }

    private void Fire()
    {
        _rocketLauncherMap.TryGetValue(startBase.options[startBase.value].text, out var launcherGameObject);

        if (launcherGameObject == null)
        {
            return;
        }

        var launcher = launcherGameObject.GetComponent<RocketLauncher>();

        _targetMap.TryGetValue(targetBase.options[targetBase.value].text, out var target);
        launcher.target = target;

        StartCoroutine(launcher.Fire(Mathf.FloorToInt(rocketCount.value)));
    }
}
