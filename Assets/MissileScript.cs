using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.VFX;


public abstract class MissileScript : MonoBehaviour {
    
    public float maxSpeed = 1100;
    public float steeringForce = 0.5f;
    public float maxSteeringAngle = 15;
    public float baseDrag = 0.1f;
    public VisualEffect engine;
    public GameObject explosion;
    [FormerlySerializedAs("light")] public GameObject engineLight;
    
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

    private TrailManagerScript _trailManager;
    
    protected void Start() {
        rb = GetComponent<Rigidbody>();
        initializationTime = Time.timeSinceLevelLoad;

        _trailManager = FindObjectOfType<TrailManagerScript>();
    }

    private void FixedUpdate()
    {
        var tf = transform;
        var position = tf.position;
        var forward = tf.rotation * Vector3.up;
        var velocity = rb.velocity;

        timeSinceInitialization = Time.timeSinceLevelLoad - initializationTime;

        if (ShouldExplode())
        {

            var colliders = Physics.OverlapSphere(position, 50);

            foreach (var collider in colliders)
            {
                if (collider.gameObject.CompareTag("Rocket"))
                {
                    Instantiate(explosion, collider.gameObject.transform.position,
                        collider.gameObject.transform.rotation);
                    Destroy(collider.gameObject);
                }
            }

            Instantiate(explosion, position, transform.rotation);
            Destroy(this);
        }
        

        currentDrag = baseDrag / Mathf.Pow(2, position.y / 5000);
        rb.AddForce(velocity.normalized * (Mathf.Pow(velocity.magnitude, 2) * -currentDrag));
        
        if (ShouldFire())
        {
            rb.AddForce(forward * (maxSpeed), ForceMode.Acceleration);
        }
        engine.SetBool("EngineFiring", ShouldFire());
        engineLight.SetActive(ShouldFire());
        
        GetSteeringAngle().ToAngleAxis(out var steeringAngle, out var steeringAxis);

        rb.AddTorque(steeringAxis * (Mathf.Clamp(steeringAngle, -maxSteeringAngle, maxSteeringAngle) * steeringForce), ForceMode.Acceleration);

        if (_trailManager != null)
        {
            _trailManager.ReportPosition(gameObject, position);
        }
    }

    protected abstract bool ShouldExplode();
    protected abstract bool ShouldFire();
    protected abstract Quaternion GetSteeringAngle();

}