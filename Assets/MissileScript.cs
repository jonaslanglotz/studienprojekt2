using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.VFX;


public abstract class MissileScript : MonoBehaviour {
    
    public float maxSpeed = 1100;
    public float steeringForce = 0.5f;
    public float maxSteeringAngle = 15;
    public float baseDrag = 0.1f;
    public VisualEffect engine;
    public GameObject light;
    
    public float currentDrag;

    [HideInInspector]
    public float initializationTime;
    
    [HideInInspector]
    public float timeSinceInitialization;
    
    [HideInInspector]
    public Rigidbody rb;

    [HideInInspector]
    public GameObject launcher;
    
    public GameObject target;
    
    protected void Start() {
        rb = GetComponent<Rigidbody>();
        initializationTime = Time.timeSinceLevelLoad;
    }

    private void FixedUpdate()
    {
        var tf = transform;
        var position = tf.position;
        var forward = tf.rotation * Vector3.up;
        var velocity = rb.velocity;

        timeSinceInitialization = Time.timeSinceLevelLoad - initializationTime;

        currentDrag = baseDrag / Mathf.Pow(2, position.y / 5000);
        rb.AddForce(velocity.normalized * Mathf.Pow(velocity.magnitude, 2) * -currentDrag);
        
        if (ShouldFire())
        {
            rb.AddForce(forward * (maxSpeed), ForceMode.Acceleration);
        }
        engine.SetBool("EngineFiring", ShouldFire());
        light.SetActive(ShouldFire());
        
        GetSteeringAngle().ToAngleAxis(out var steeringAngle, out var steeringAxis);

        rb.AddTorque(steeringAxis * (Mathf.Clamp(steeringAngle, -maxSteeringAngle, maxSteeringAngle) * steeringForce), ForceMode.Acceleration);
    }

    protected abstract bool ShouldFire();
    protected abstract Quaternion GetSteeringAngle();
}