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

    public abstract void ApplyGravity();
    public abstract void ApplyDrag();
    public abstract void UpdateMass();
    public abstract void UpdateThermal();

    public abstract float GetMassFromSize();

    protected void Awake()
    {
        rigidBody = this.GetComponent<Rigidbody>();
        rend = this.GetComponent<Renderer>();        
    }

    protected void updateShaderVar(float newVal)
    {
        rend.material.SetFloat(shaderVar, newVal);
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
