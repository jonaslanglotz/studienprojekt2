using System;
using UnityEngine;

public class UnguidedMissile : MissileScript
{

    public float fireTime;

    [HideInInspector]
    public float launchAngle;
    [HideInInspector]
    public float eta;

    private bool shouldExplode = false;

    protected override bool ShouldExplode()
    {
        return shouldExplode;
    }

    private void OnCollisionEnter(Collision collision)
    {
        shouldExplode = true;
    }

    protected override bool ShouldFire()
    {
        if (timeSinceInitialization < 0.2)
        {
            Debug.Log($"Rocket: ${transform.name}, ${transform.position}, ${transform.rotation}");
        }
        return timeSinceInitialization < fireTime;
    }

    protected override Quaternion GetSteeringAngle()
    {
        return ShouldFire() ? Quaternion.identity : Quaternion.FromToRotation(transform.up, rb.velocity);
    }
}