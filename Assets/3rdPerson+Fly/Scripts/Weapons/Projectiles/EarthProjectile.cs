using UnityEngine;

public class EarthProjectile : ProjectileBase
{
    private Transform m_lastTransform;

    public override void UpdateMass()
    {
        if (rigidBody.transform != m_lastTransform)
        {
            rigidBody.SetDensity(density);
        }

        m_lastTransform = rigidBody.transform;
    }

    public override void ApplyGravity()
    {
        // apply gravity
        var globalDown = new Vector3(0,-1,0);
        Vector3 grav_force = gravity * globalDown * rigidBody.mass; 
        rigidBody.AddForce(grav_force);
    }

    public override void ApplyDrag()
    {
        // apply drag
        Vector3 drag_force = quadDragCoeff * -1 * rigidBody.velocity.normalized * rigidBody.velocity.magnitude; 
        rigidBody.AddForce(drag_force);
    }
                       
}