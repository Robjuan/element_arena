using UnityEngine;
using System;

public class EarthProjectile : ProjectileBase
{
    private Transform m_lastTransform;

    public override float GetDamage()
    {
        return rigidBody.mass * temperature * rigidBody.velocity.magnitude;
    }

    public override float GetMassFromSize()
    {        
        // this is real, however it means that mass increases incredibly fast with size
        // we're going to want some sort of curve - 
        // mass should not decrease too fast at low numbers (miniscule lazer projectiles are bad (no visual feedback, hard physics))
        // mass should not get out of hand at large sizes (giant immovable boulders are bad (niche use case - doable at max size))
        // mass should otherwise smoothly increase 

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
        // TODO : an effect i'm actually excited about
        
        // the actual range is set by the weapon, but setting the range here means that the colour won't change outside these values.
        float temp_max = 40f;
        float temp_min = 5f;
        
        float scaled = ((this.temperature - temp_min) / (temp_max - temp_min));

        var rend = this.GetComponent<Renderer>();

        // this brown is very light - how to do dark rocks
        Color brown = new Color(0.1f,0.02f,0f,1f);
        Color orange = new Color(1.1f,0.60f,0.06f,1f);

        rend.material.color = Color.Lerp(brown, orange,scaled);
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