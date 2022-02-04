using UnityEngine;

public class UnguidedMissile : MissileScript
{

    public float fireTime;

    [HideInInspector]
    public float launchAngle;
    [HideInInspector]
    public float eta;

    protected override bool ShouldFire()
    {
        return timeSinceInitialization < fireTime;
    }

    protected override Quaternion GetSteeringAngle()
    {
        return ShouldFire() ? Quaternion.identity : Quaternion.FromToRotation(transform.forward, rb.velocity);
    }
}