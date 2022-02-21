using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class ProportionalNavigationHelper
{
    private readonly float _gain;
    
    private Vector3 _position = Vector3.zero;
    private Vector3 _lastPosition = Vector3.zero;
    private Vector3 _velocity = Vector3.zero;

    private Vector3 _targetPosition = Vector3.zero;
    private Vector3 _targetLastPosition = Vector3.zero;

    public ProportionalNavigationHelper(float gain)
    {
        _gain = gain;
    }

    public void EnterMissilePosition(Vector3 position)
    {
        _velocity = (_position - _lastPosition) / Time.fixedDeltaTime;
        
        _lastPosition = _position;
        _position = position;
    }
    
    public void EnterTargetPosition(Vector3 position)
    {
        _targetLastPosition = _targetPosition;
        _targetPosition = position;
    }
    
    public Vector3 CommandAcceleration()
    {
        var losNormal = Vector3.Cross(LineOfSight(), Vector3.Cross(LineOfSight(), LineOfSight() - (_targetLastPosition - _lastPosition))).normalized;
        var accelerationMagnitude = _gain * RelativeVelocity().magnitude * AngularRate().magnitude + (_gain / 2) * ComponentOfVectorInDirection(Vector3.up * -9.81f, losNormal);
        return LineOfSight().normalized * 10 - losNormal * accelerationMagnitude;
    }

    private static float ComponentOfVectorInDirection(Vector3 vector, Vector3 direction)
    {
        return Vector3.Cross(direction.normalized, Vector3.Cross(vector, direction.normalized)).magnitude;
    }

    private Vector3 LineOfSight()
    {
        return _targetPosition - _position;
    }

    private Vector3 RelativeVelocity()
    {
        return TargetVelocity() - _velocity;
    }

    private Vector3 TargetVelocity()
    {
        return (_targetPosition - _targetLastPosition) / Time.fixedDeltaTime;
    }

    private Vector3 AngularRate()
    {
        return Vector3.Cross(LineOfSight(), RelativeVelocity()) / LineOfSight().sqrMagnitude;
    }
}