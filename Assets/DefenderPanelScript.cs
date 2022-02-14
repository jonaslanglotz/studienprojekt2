using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DefenderPanelScript : MonoBehaviour
{

    public TMP_Dropdown baseDropdown;
    public Slider rocketCountSlider;
    public TMP_Text rocketCountLabel;
    public Button startButton;
    
    private Dictionary<string, GameObject> _baseMap;
    
    // Start is called before the first frame update
    private void Start()
    {
        _baseMap = new Dictionary<string, GameObject>();
        
        var bases = GameObject.FindGameObjectsWithTag("Base");

        for (var i = 0; i < bases.Length; i++)
        {
            _baseMap.Add($"Startbasis {i+1}", bases[i]);
        } 
        
        baseDropdown.options.Clear();
        foreach (var startBase in _baseMap.Keys)
        {
            baseDropdown.options.Add(new TMP_Dropdown.OptionData(startBase));
        }
        
        rocketCountSlider.onValueChanged.AddListener(value => rocketCountLabel.text = Mathf.FloorToInt(value).ToString());
        
        startButton.onClick.AddListener(Fire);
        
    }

    private void Fire()
    {
        _baseMap.TryGetValue(baseDropdown.options[baseDropdown.value].text, out var baseGameObject);

        if (baseGameObject == null)
        {
            return;
        }

        var launcher = baseGameObject.transform.GetComponentInChildren<IronDomeBattery>();

        StartCoroutine(launcher.Fire(Mathf.FloorToInt(rocketCountSlider.value)));
    }
}
