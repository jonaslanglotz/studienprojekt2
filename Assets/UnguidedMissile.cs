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
        return timeSinceInitialization < fireTime;
    }

    protected override Quaternion GetSteeringAngle()
    {
        return ShouldFire() ? Quaternion.identity : Quaternion.FromToRotation(transform.forward, rb.velocity);
    }
}