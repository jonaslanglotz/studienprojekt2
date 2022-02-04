using UnityEngine;
using Quaternion = UnityEngine.Quaternion;

public class GuidedMissile : MissileScript
{
    private ProportionalNavigationHelper _proportionalNavigation;
    
    protected new void Start()
    {
        base.Start();
        _proportionalNavigation = new ProportionalNavigationHelper(1);
    }
    protected override bool ShouldFire()
    {
        return true;
    }

    protected override Quaternion GetSteeringAngle()
    {
        UpdateProportionalNavigationData();
        
        var rotation = CalculateSteeringAngle();

        return NormalizeAngle(rotation);
    }

    private void UpdateProportionalNavigationData()
    {
        _proportionalNavigation.EnterMissilePosition(transform.position);
        _proportionalNavigation.EnterTargetPosition(target.transform.position);
    }

    private Quaternion CalculateSteeringAngle()
    {
        return Quaternion.FromToRotation(transform.rotation * Vector3.up, _proportionalNavigation.CommandAcceleration() );
    }

    private static Quaternion NormalizeAngle(Quaternion rotation)
    {
        rotation.ToAngleAxis(out var angle, out var axis);
        
        if (Mathf.Abs(angle) > 180)
        {
            angle = angle > 0 ? -360 + angle : 360 + angle;
        }

        return Quaternion.AngleAxis(angle, axis);
    }
}