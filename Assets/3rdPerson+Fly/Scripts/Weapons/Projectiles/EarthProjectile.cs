using UnityEngine;
using System;

public class EarthProjectile : ProjectileBase
{
    private Transform m_lastTransform;

    public override float GetMassFromSize()
    {        
        // this is real, however it means that mass increases incredibly fast with size
        var radius = GetComponent<Renderer>().bounds.extents.magnitude;
        var volume = (4/3) * Math.PI * Math.Pow(radius,2); // r^3 is real world, r^2 is much less punishing for giant rocks
        return (float)volume * density;
    }

    public override void UpdateMass()
    {
        if (rigidBody.transform != m_lastTransform)
        {
            var mass = GetMassFromSize();
            rigidBody.mass = mass;
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