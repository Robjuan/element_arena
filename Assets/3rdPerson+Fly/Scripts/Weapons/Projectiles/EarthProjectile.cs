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

    public override void UpdateThermal()
    {
        // transform object . temp into a good shader variable
        // grit buildup shader values:
        // 0 - fully lava'd
        // 10 - almost fully rock
        // 100 - totally rock -- only small change from 10. 
        // good enough to scale to 0-10 for now.

        // temps above 40 will be all lava'd  // TODO: ignition??
        // temp 40 = shader 0
        // temp 0 = shader 10

        // convert to 0-10 range
        var scaled = (this.temperature / 40) * 10;

        // invert, shader gets "cooler" with higher vals // (max + min) - num
        var shaderVal = 10f - scaled;

        updateShaderVar(shaderVal);
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