using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncher : MonoBehaviour
{

    public GameObject target;
    public GameObject missilePrefab;

    public float maximumFireTime;

    private UnguidedMissilePredictor _predictor;
    
    void Start()
    {
        _predictor = new UnguidedMissilePredictor(10000, 0.02f);
    }

    void Update()
    {
        if (!Input.GetKeyDown(KeyCode.L)) return;

        var missile = GameObject.Instantiate(missilePrefab);
        
        var distanceToTarget = (target.transform.position - this.transform.position).magnitude;

        var rb = missile.GetComponent<Rigidbody>();
        var script = missile.GetComponent<UnguidedMissile>();

        script.launcher = gameObject;
        script.target = target;

        var optimalFireTime = 0f;
        var optimalAngle = 0f;
        var optimalOffset = float.MaxValue;

        for (var i = 1; i < 4; i++)
        {
            var fireTime = maximumFireTime * (i / 3f);

            var min = 0f;
            var max = 57f;

            var currentAngle = (max + min) / 2;
            var currentDistance = 0f;

            for (var iterations = 0;
                 Mathf.Abs(currentDistance - distanceToTarget) > 10 && iterations < 20;
                 iterations++)
            {
                
                
                _predictor.SetMissile(rb.mass, script.maxSpeed, script.baseDrag, fireTime, currentAngle);
                try
                {
                    currentDistance = _predictor.SimulateImpactPoint();
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                    throw;
                }

                var offset = distanceToTarget - currentDistance;
                
                Debug.Log($"currentAngle: {currentAngle}, min: {min}, max: {max}, offset: {offset}, fireTime: {fireTime}, distance: {distanceToTarget}, calcDistance: {currentDistance}");

                if (Mathf.Abs(offset) < optimalOffset)
                {
                    optimalOffset = Mathf.Abs(offset);
                    optimalAngle = currentAngle;
                    optimalFireTime = fireTime;
                }

                if (offset >= 0)
                {
                    min = currentAngle;
                }
                else
                {
                    max = currentAngle;
                }
                currentAngle = (min + max) / 2;
            }

            if (optimalOffset < 10)
            {
                break;
            }
        }

        script.fireTime = optimalFireTime;
        
        Debug.Log($"FireTime: {optimalFireTime}, Angle: {optimalAngle}, CalculatedOffset: {optimalOffset}");

        missile.transform.position = transform.position;
        missile.transform.rotation = Quaternion.AngleAxis(-90 + optimalAngle, Vector3.Cross(target.transform.position - transform.position, Vector3.up));
    }
}
