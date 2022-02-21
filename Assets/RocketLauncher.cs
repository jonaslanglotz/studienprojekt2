using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncher : MonoBehaviour
{

    public GameObject target;
    public GameObject missilePrefab;

    public GameObject launcherAssembly;
        
    public float maximumFireTime;

    private UnguidedMissilePredictor _predictor;
    
    
    void Start()
    {
        _predictor = new UnguidedMissilePredictor(10000, 0.02f);
    }

    void Update()
    {
        if (!Input.GetKeyDown(KeyCode.L)) return;

        StartCoroutine(Fire(1));
    }
    
    GameObject CreateMissile()
    {
        var missile = Instantiate(missilePrefab);
        
        var script = missile.GetComponent<UnguidedMissile>();
        script.launcher = gameObject;
        script.target = target;

        return missile;
    }

    void SetFiringParameters(GameObject missile, float fireTime, float angle)
    {
        var script = missile.GetComponent<UnguidedMissile>();
        script.fireTime = fireTime;
        script.launchAngle = angle;

        missile.transform.position = transform.position;
        missile.transform.rotation = Quaternion.AngleAxis(-90 + angle, Vector3.Cross(target.transform.position - transform.position, Vector3.up));
    }

    void CalculateLaunchParameters(GameObject missile, out float angle, out float fireTime)
    {
        var distanceToTarget = (target.transform.position - transform.position).magnitude;
        
        var rb = missile.GetComponent<Rigidbody>();
        var script = missile.GetComponent<UnguidedMissile>();
        
        var optimalFireTime = 0f;
        var optimalAngle = 0f;
        var optimalOffset = float.MaxValue;

        for (var i = 1; i < 4; i++)
        {
            var currentFireTime = maximumFireTime * (i / 3f);

            var min = 0f;
            var max = 57f;

            var currentAngle = (max + min) / 2;
            var currentDistance = 0f;

            for (var iterations = 0;
                 Mathf.Abs(currentDistance - distanceToTarget) > 10 && iterations < 20;
                 iterations++)
            {
                
                
                _predictor.SetMissile(rb.mass, script.maxSpeed, script.baseDrag, currentFireTime, currentAngle);
                try
                {
                    currentDistance = _predictor.SimulateImpactPoint(transform.position.y, target.transform.position.y);
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                    throw;
                }

                var offset = distanceToTarget - currentDistance;
                
                if (Mathf.Abs(offset) < optimalOffset)
                {
                    optimalOffset = Mathf.Abs(offset);
                    optimalAngle = currentAngle;
                    optimalFireTime = currentFireTime;
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

        angle = optimalAngle;
        fireTime = optimalFireTime;
    }
    

    public IEnumerator Fire(int rocketCount)
    {
        const float angleStep = 0.1f;
        
        CalculateLaunchParameters(missilePrefab, out var firingAngle, out var fireTime );
        Debug.Log($"angle: ${firingAngle}, time: ${fireTime}");

        var initialVerticalRotation = launcherAssembly.transform.localRotation.eulerAngles.x;
        // var endVerticalRotation = Quaternion.Euler(firingAngle, 0f, 0f);

        var targetVector = target.transform.position - transform.position;
        var initialHorizontalRotation = transform.rotation.eulerAngles.y;
        var endHorizontalRotation = Vector3.Angle(targetVector, Vector3.forward) - 180;
        endHorizontalRotation = targetVector.x < 0 ? -endHorizontalRotation : endHorizontalRotation;

        var maxAngleDifference = Mathf.Max(Mathf.Abs(Mathf.DeltaAngle(firingAngle, initialVerticalRotation)),
            Mathf.Abs(Mathf.DeltaAngle(endHorizontalRotation, initialHorizontalRotation)));
        
        for (int i = 0; i < maxAngleDifference; i++)
        {
            launcherAssembly.transform.localRotation = Quaternion.AngleAxis(
                Mathf.LerpAngle(initialVerticalRotation, firingAngle, i / maxAngleDifference), Vector3.right);
            transform.rotation = Quaternion.AngleAxis(Mathf.LerpAngle(initialHorizontalRotation, endHorizontalRotation, i/maxAngleDifference), Vector3.up);
            yield return new WaitForEndOfFrame();
        }
        launcherAssembly.transform.localRotation = Quaternion.AngleAxis(firingAngle, Vector3.right);
        transform.rotation = Quaternion.AngleAxis(endHorizontalRotation, Vector3.up);

        for (int i = 0; i < rocketCount; i++)
        {
            var missile = CreateMissile();
            SetFiringParameters(missile, fireTime, firingAngle);
            yield return new WaitForSeconds(0.5f);
        }

        yield return null;
    }
}
