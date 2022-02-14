using UnityEngine;

[ExecuteAlways]
public class Flicker : MonoBehaviour
{

    public AnimationCurve intensityValues;
    public float baseIntensity;

    private Light _light;

    private void Start()
    {
        _light = GetComponent<Light>();
    }

    private void Update()
    {
        _light.intensity = baseIntensity * intensityValues.Evaluate(Random.value);
    }
}
