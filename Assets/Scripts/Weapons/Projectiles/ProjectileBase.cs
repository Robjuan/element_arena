using UnityEngine;

// this projectile class is the base from which all projectiles are inherited
// projectiles are not designed to use regular gravity, instead implement their own drag/gravity/mass etc
// any actual projectile should inherit from this class

public abstract class ProjectileBase : MonoBehaviour, IPooledObject
{
    [Header("Physics")]
    [Tooltip("Quadratic Drag Coefficient")]
    public float quadDragCoeff = 0.1f;
    [Tooltip("Gravity (Set Mass on Rigidbody)")]
    public float gravity = 0.1f;
    [Tooltip("Density * Size = mass")]
    public float density;

    public float shatterForce;
    public float maxLifetime;
    private float spawnTime;

    
    [HideInInspector] public float shootForce; // set by modifier values

    [HideInInspector] public bool isPlaceholder = false; // for calcs & showing before instantiate

    //public LayerMask layerMask = -1;

    public Vector3 initialPosition { get; protected set; }
    public Vector3 initialDirection { get; protected set; }
    protected Rigidbody rigidBody;
    protected Renderer innerSphereRend;
    protected Renderer outerSphereRend;
    [Header("Thermals & Appearance")]
    public ThermalBody thermals;
    protected SphereCollider thisColl;

    protected Vector3 lastPosition;
    private float radius;

    protected GameObject innerSphere;

    public float maxTransparencyValue;
    public Material inner_TransparentMaterial;
    public Material outer_TransparentMaterial;
    protected Material inner_StartingMaterial;
    protected Material outer_StartingMaterial;

    protected bool isDestroyed = false;
    [HideInInspector] public bool isObstructed = false;
    [HideInInspector] public Color outerSphereLastColor; // for going back after showing red on obstruction

    public abstract void ApplyGravity();
    public abstract void ApplyDrag();
    public abstract void UpdateMass();

    public abstract void UpdateThermalApperance();

    public abstract float GetMassFromSize();
    public abstract float GetDamage(Collision coll);

    public abstract void DestroyProjectile();

    protected void Awake()
    {
        rigidBody = GetComponentInChildren<Rigidbody>();
        thermals = GetComponentInChildren<ThermalBody>();
        thisColl = GetComponentInChildren<SphereCollider>();

        foreach (Transform child in gameObject.transform)
        {
            if (child.tag == "Projectile_InnerSphere")
            {
                innerSphere = child.gameObject;
                innerSphereRend = child.GetComponent<Renderer>();
                inner_StartingMaterial = innerSphereRend.material;

            }
            else if (child.tag == "Projectile_OuterSphere")
            {
                outerSphereRend = child.GetComponent<Renderer>();
                outer_StartingMaterial = outerSphereRend.material;
            }
        }
    }

    public void ResetMaterials()
    {
        innerSphereRend.material = inner_StartingMaterial;
        outerSphereRend.material = outer_StartingMaterial;
    }

    public void OnObjectSpawn() 
    {
        ResetMaterials();
        rigidBody.velocity = Vector3.zero;
        rigidBody.angularVelocity = Vector3.zero;
        isDestroyed = false;
        outerSphereRend.enabled = true;
        innerSphereRend.enabled = true;
        thisColl.enabled = true;
    }

    public void Start()
    {

        if (isPlaceholder)
        {
            innerSphereRend.material = inner_TransparentMaterial;
            outerSphereRend.material = outer_TransparentMaterial;

            UpdateTransparency(1);

            // no collisions
            thisColl.isTrigger = true;
        }
    }

    public void UpdateTransparency(float transparencyVal)
    {
        Color inner_oldcolor = innerSphereRend.material.color;
        innerSphereRend.material.SetColor("_Color", new Color(inner_oldcolor.r, inner_oldcolor.g, inner_oldcolor.b, transparencyVal * maxTransparencyValue));

        Color outer_oldcolor = outerSphereRend.material.color;
        outerSphereRend.material.SetColor("_Color", new Color(outer_oldcolor.r, outer_oldcolor.g, outer_oldcolor.b, transparencyVal * maxTransparencyValue));
    }

    protected void FixedUpdate()
    {
        if (!isPlaceholder)
        {
            // physics updates must come before applys
            UpdateMass();

            ApplyGravity();
            ApplyDrag();
        }

    }

    protected void Update()
    {
        UpdateThermalApperance();
        if ((spawnTime + maxLifetime <= Time.time) && !isDestroyed && !isPlaceholder)
        {
            DestroyProjectile();
        }
    }

    public float GetRadius()
    {
        var scale = gameObject.transform.localScale.x; // sphere should be the same in all direction
        return thisColl.radius * scale; // multiply collider radius by localscale
    }

    // on trigger x should only be called if isTrigger is enabled, ie is placeholder
    // may also be called if an ext object is a trigger and rego projectile goes in
    // two triggers do not collide with each other
    private void OnTriggerEnter(Collider other)
    {
        if (isPlaceholder)
        {
            isObstructed = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isPlaceholder)
        {
            isObstructed = false;
        }
    }

    public void HandleObstruction()
    {
        outerSphereLastColor = outerSphereRend.material.color;
        outerSphereRend.material.SetColor("_Color", new Color(Color.red.r, Color.red.g, Color.red.b, maxTransparencyValue));
    }

    public void ResetOuterSphereColor()
    {
        outerSphereRend.material.SetColor("_Color", outerSphereLastColor);
    }

    void OnCollisionEnter(Collision coll)
    {
        var other = coll.gameObject;
        Damageable damageable = other.GetComponent<Damageable>();
        if (damageable)
        {
            damageable.InflictDamage(GetDamage(coll), this.gameObject);
        }

        var cp = coll.GetContact(0);
        var collEnergy = Vector3.Dot(cp.normal, coll.relativeVelocity);
        if (collEnergy >= shatterForce && !isDestroyed && !isPlaceholder)
        {
            Debug.Log("shattering with collenergy: " + collEnergy);
            DestroyProjectile();
        }
    }



    public void Shoot(Vector3 shotDirection)
    {
        spawnTime = Time.time;
        initialPosition = transform.position;
        // forcemode allows control of continuous vs instant , and mass-affected vs not
        rigidBody.AddForce(shotDirection * shootForce, ForceMode.Impulse);
    }

    public Rigidbody GetRigidBody()
    {
        return rigidBody;
    }

}
