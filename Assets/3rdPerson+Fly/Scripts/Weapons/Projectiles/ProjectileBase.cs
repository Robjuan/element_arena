using UnityEngine;

// this projectile class is the base from which all projectiles are inherited
// projectiles are not designed to use regular gravity, instead implement their own drag/gravity/mass etc
// any actual projectile should inherit from this class

public abstract class ProjectileBase : MonoBehaviour
{   
    [Header("Physics")]
    [Tooltip("Quadratic Drag Coefficient")]
    public float quadDragCoeff = 0.1f;
    [Tooltip("Gravity (Set Mass on Rigidbody)")]
    public float gravity = 0.1f;
    [Tooltip("Density * Size = mass")]
    public float density;

    [Tooltip("Temperature at which projectile catches fire")]
    public float ignitionTemperature;

    [Tooltip("Inactive projectiles can't collide or be rendered")]
    public bool isActive = false;

    public float shootForce;
    public float temperature;

    //public LayerMask layerMask = -1;

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
        rigidBody = GetComponent<Rigidbody>();
        rend = this.GetComponentInChildren<Renderer>();        
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

    protected void OnCollisionEnter(Collision coll)
    {
        var other = coll.gameObject;
        Damageable damageable = other.GetComponent<Damageable>();
        if (damageable)
        {
            damageable.InflictDamage(GetDamage(), this.gameObject);
        }
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
