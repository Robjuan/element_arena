using UnityEngine;

// this projectile class is the base from which all projectiles are inherited
// projectiles are not designed to use regular gravity, instead implement their own drag/gravity/mass etc
// any actual projectile should inherit from this class

public abstract class ProjectileBase : MonoBehaviour
{   
    [Header("Graphics")]
    [Tooltip("Shadervar")]
    public string shaderVar = "_Power";

    [Header("Physics")]
    [Tooltip("Quadratic Drag Coefficient")]
    public float quadDragCoeff = 0.1f;
    [Tooltip("Gravity (Set Mass on Rigidbody)")]
    public float gravity = 0.1f;
    [Tooltip("Density * Size = mass")]
    public float density;

    [Tooltip("Temperature at which projectile catches fire")]
    public float ignitionTemperature;

    public float shootForce;
    public float temperature;
  
    public Vector3 initialPosition { get; protected set; }
    public Vector3 initialDirection { get; protected set; }
    protected Rigidbody rigidBody;
    protected Renderer rend;

    protected Vector3 lastPosition;
    protected float radius;

    public abstract void ApplyGravity();
    public abstract void ApplyDrag();
    public abstract void UpdateMass();
    public abstract void UpdateThermal();

    public abstract float GetMassFromSize();
    public abstract float GetDamage();

    protected void Awake()
    {
        rigidBody = this.GetComponent<Rigidbody>();
        rend = this.GetComponent<Renderer>();        
        radius = GetComponent<Renderer>().bounds.extents.magnitude;
    }

    protected void FixedUpdate()
    {
        // appearance first to prevent flashing?
        UpdateThermal();

        // physics updates must come before applys
        UpdateMass();      

        ApplyGravity();
        ApplyDrag();
    }

    void Update()
    {
        // Hit detection
        RaycastHit closestHit = new RaycastHit();
        closestHit.distance = Mathf.Infinity;
        bool foundHit = false;

        // Sphere cast
        Vector3 displacementSinceLastFrame = transform.position - lastPosition;
        RaycastHit[] hits = Physics.SphereCastAll(lastPosition, radius, displacementSinceLastFrame.normalized, displacementSinceLastFrame.magnitude);
        foreach (var hit in hits)
        {
            if (IsHitValid(hit) && hit.distance < closestHit.distance)
            {
                foundHit = true;
                closestHit = hit;
            }
        }

        if (foundHit)
        {
            // Handle case of casting while already inside a collider
            if(closestHit.distance <= 0f)
            {
                closestHit.point = transform.position;
                closestHit.normal = -transform.forward;
            }

            OnHit(closestHit.point, closestHit.collider);
        }

        lastPosition = transform.position;
    }

    protected void OnHit(Vector3 point, Collider collider)
    {
        Damageable damageable = collider.GetComponent<Damageable>();
        if (damageable)
        {
            damageable.InflictDamage(GetDamage(), this.gameObject);
        }
    }

    protected bool IsHitValid(RaycastHit hit)
    {
        if(hit.collider.GetComponent<Damageable>() == null)
        {
            return false;
        }
        return true;
    }

    public void Shoot(Vector3 shotDirection)
    {
        initialPosition = transform.position;
        // forcemode allows control of continuous vs instant , and mass-affected vs not
        rigidBody.AddForce(shotDirection * shootForce, ForceMode.Impulse);
    }

    public Rigidbody GetRigidBody()
    {
        return rigidBody;
    }

}
