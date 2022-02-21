using System;
using UnityEngine;

public class UnguidedMissilePredictor
{
    private float _baseDrag;
    private float _mass;
    private float _thrust;

    private float _fireTime;
    private float _firingAngle;

    private const float Gravity = 9.81f;
    
    private readonly int _maxSteps;
    private readonly float _stepSize;

    private int _step;
    private Vector2 _position;
    private Vector2 _velocity;

    public UnguidedMissilePredictor(int maxSteps, float stepSize)
    {
        _maxSteps = maxSteps;
        _stepSize = stepSize;
    }

    public void SetMissile(float mass, float thrust, float baseDrag, float fireTime, float firingAngle)
    {
        _baseDrag = baseDrag;
        _mass = mass;
        _thrust = thrust;
        _fireTime = fireTime;
        _firingAngle = firingAngle;
    }

    public float SimulateImpactPoint(float launchHeight, float targetHeight)
    {
        _position = new Vector2(0, launchHeight);
        _velocity = Vector2.zero;
        _step = 0;

        for (var i = 0; i < _maxSteps; i++)
        {
            Step();

            if (_position.y < targetHeight && _velocity.y < 0f)
            {
                // Debug.Log($"t: ${elapsedTime()}; pos: ${_position}; fire: ${_fireTime}");
                // var bias = Mathf.Pow(_firingAngle / 45f, 2f) * 300 + (_position.x / 300);
                var bias = Mathf.Pow(_firingAngle / 45f, 2f) * 300;
                // var bias = 0;
                return _position.x + bias;
            }
        }
        
        throw new Exception($"Could not find an impact within maximum steps. Current Position: {_position}.");
    }

    private void Step()
    {
        var elapsedTime = this.elapsedTime();
        
        //Debug.Log($"Time: {elapsedTime}; Step: {_step}, Position: {_position}; Velocity: {_velocity}; IsFiring: {elapsedTime < _fireTime}");

        // Engine
        if (elapsedTime <= _fireTime)
        {
            _velocity += (Vector2.right * _thrust * _stepSize).Rotate(_firingAngle);
        }
        
        // Drag
        var currentDrag = _baseDrag / Mathf.Pow(2, _position.y / 5000);
        _velocity -= (_velocity.normalized * Mathf.Pow(_velocity.magnitude, 2) * currentDrag *_stepSize) / _mass;

        // Gravity
        _velocity += Vector2.down * _stepSize * Gravity; 
        
        _position += _velocity * _stepSize; 
        _step++;
    }

    private float elapsedTime()
    {
        return _step * _stepSize;
    }
}