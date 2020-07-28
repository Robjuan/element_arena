using UnityEngine;
using System;
using UnityEditor.Animations;
using UnityEngine.UIElements;

public class EarthProjectile : ProjectileBase
{
    private Vector3 lastLocalScale;
    private float lastTemp;

    // palette: https://www.color-hex.com/color-palette/3392
    public Color underlay_MaxTemp = new Color32(255, 102, 0, 255);
    public Color underlay_MinTemp = new Color32(85, 51, 51, 255);
          
    public override float GetDamage(Collision coll)
    {
        //Debug.Log("mass = " + rigidBody.mass + ", temp = " + temperature + ", v = " + rigidBody.velocity.magnitude);
        //  dot product of collision normal and collision velocity,  times the mass of the other collider 

        var cp = coll.GetContact(0);
        var collEnergy = Vector3.Dot(cp.normal,coll.relativeVelocity);
        return collEnergy * rigidBody.mass * thermals.Temperature;
    }

    public override float GetMassFromSize()
    {      
        // real = (4/3) * Math.PI * Math.Pow(radius,2)
        // this is real, however it means that mass increases incredibly fast with size
        // we're going to want some sort of curve - 
        // mass should not decrease too fast at low numbers (miniscule lazer projectiles are bad (no visual feedback, hard physics))
        // mass should not get out of hand at large sizes (giant immovable boulders are bad (niche use case - doable at max size))
        // mass should otherwise smoothly increase 

        // with new, accurate radius, mass is increasing not that fast tbh

        var volume = Math.Pow(this.GetRadius(),3); // r^3 is real world, r^2 is much less punishing for giant rocks
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
      
        // todo: store these vars in a better place
        // the actual range is set by the weapon, but setting the range here means that the colour won't change outside these values.

        if (lastTemp!= thermals.Temperature)
        {
            
            float temp_max = 40f;
            float temp_min = 5f;

            // innersphere radius numbers // revisit these numbers with the new projectile
            float inner_max = 1.683f;
            float inner_min = 0.795f;

            
            float scaled = (inner_max - inner_min) * ((thermals.Temperature - temp_min) / (temp_max - temp_min)) + inner_min;
            //Debug.Log(innerSphere.transform.localScale);
            //Debug.Log(scaled);

            innerSphere.transform.localScale = new Vector3(scaled, scaled, scaled);

            // todo: shift the colour of both lava & rock
            //rend.material.color = Color.Lerp(underlay_MaxTemp, underlay_MinTemp, scaled);

            lastTemp = thermals.Temperature;
        }
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