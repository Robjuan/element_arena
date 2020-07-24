using UnityEngine;
using System;

public class EarthProjectile : ProjectileBase
{
    private Vector3 lastLocalScale;

    public override float GetDamage(Collision coll)
    {
        //Debug.Log("mass = " + rigidBody.mass + ", temp = " + temperature + ", v = " + rigidBody.velocity.magnitude);
        //  dot product of collision normal and collision velocity (ie the velocity of the two bodies relative to each other), times the mass of the other collider 
        //return Vector3.Dot(coll.relativeVelocity, )
        var cp = coll.GetContact(0);
        //cp.normal * 
        return rigidBody.mass * thermals.Temperature * rigidBody.velocity.magnitude;
    }

    public override float GetMassFromSize()
    {      
        // real = (4/3) * Math.PI * Math.Pow(radius,2)
        // this is real, however it means that mass increases incredibly fast with size
        // we're going to want some sort of curve - 
        // mass should not decrease too fast at low numbers (miniscule lazer projectiles are bad (no visual feedback, hard physics))
        // mass should not get out of hand at large sizes (giant immovable boulders are bad (niche use case - doable at max size))
        // mass should otherwise smoothly increase 
        var radius = rend.bounds.extents.magnitude;
        var volume = Math.Pow(radius,2); // r^3 is real world, r^2 is much less punishing for giant rocks
        var retval = (float)volume * density;
        return retval;
    }

    public override void UpdateMass()
    {
        if (rigidBody.transform.localScale != lastLocalScale)
        {
            var mass = GetMassFromSize();
            rigidBody.mass = mass;
        }

        lastLocalScale = rigidBody.transform.localScale;
        
    }

    public override void UpdateThermalApperance()
    {
        // TODO : an effect i'm actually excited about
        
        // the actual range is set by the weapon, but setting the range here means that the colour won't change outside these values.
        float temp_max = 40f;
        float temp_min = 5f;

        float scaled = ((thermals.Temperature - temp_min) / (temp_max - temp_min));

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