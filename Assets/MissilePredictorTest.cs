using System;
using UnityEngine;

public class MissilePredictorTest : MonoBehaviour
{
    private void Start()
    {
        var predictor = new UnguidedMissilePredictor(1000000, 0.02f);
        
        predictor.SetMissile(915, 220, 0.03f, 5.3f, 45);

        try
        {
            Debug.Log(predictor.SimulateImpactPoint());
        }
        catch (Exception e)
        {
            Debug.Log(e);
            throw;
        }

    }
}