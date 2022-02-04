using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Flicker : MonoBehaviour
{

    public AnimationCurve intensityValues;
    public float baseIntensity;

    private Light _light;
    
    void Start()
    {
        _light = GetComponent<Light>();
    }

    void Update()
    {
        _light.intensity = baseIntensity * intensityValues.Evaluate(Random.value);
    }
}
