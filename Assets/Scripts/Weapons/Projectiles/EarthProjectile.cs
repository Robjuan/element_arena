using UnityEngine;
using System;
using System.Collections;

public class EarthProjectile : ProjectileBase
{
    [Header("Earth Projectile Properties")]
    // palette: https://www.color-hex.com/color-palette/3392
    public Color ignitionColor = new Color32(191, 18, 0, 255);


    [Header("Remnants")]
    public GameObject[] small_possibleRemnantObjects;
    public GameObject[] large_possibleRemnantObjects;

    private GameObject largeRemnant;
    private GameObject[] smallRemnants;

    private Vector3 lastLocalScale;
    private float lastTemp;


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

        if (lastTemp!= thermals.Temperature)
        {

            // the actual range is set by the weapon, but setting the range here means that the colour won't change outside these values.
            float temp_max = 35f;
            float temp_min = 8f;

            // innersphere radius numbers 
            // these are set based on the prefab and what localscale vars look good
            float inner_max = 1.1f;
            float inner_min = 1.0f;


            float scaled =  HelperFunctions.ScaleToRange(thermals.Temperature, temp_max, temp_min, inner_max, inner_min);
            innerSphere.transform.localScale = new Vector3(scaled, scaled, scaled);

            // only start glowing when we get hot
            if (thermals.isIgnited())
            {
                // these are inspector values but don't match the actual color values
                //float emission_min = 7.3f;
                //float emission_max = 10f;

                float emission_min = 1f;
                float emission_max = 500f;

                float intensity = HelperFunctions.ScaleToRange(thermals.Temperature, temp_max, temp_min, emission_max, emission_min);
                               
                innerSphereRend.material.EnableKeyword("_EMISSION");
                innerSphereRend.material.SetColor("_EmissionColor", ignitionColor * intensity);

            }
            else // turn off glowing if temp drops below ignition
            {
                innerSphereRend.material.DisableKeyword("_EMISSION");
            }

            lastTemp = thermals.Temperature;
        }
    }


    public override void ApplyGravity()
    {
        var globalDown = new Vector3(0,-1,0);
        Vector3 grav_force = gravity * globalDown * rigidBody.mass; 
        rigidBody.AddForce(grav_force);
    }

    public override void ApplyDrag()
    {
        Vector3 drag_force = quadDragCoeff * -1 * rigidBody.velocity.normalized * rigidBody.velocity.magnitude; 
        rigidBody.AddForce(drag_force);
    }

    public override void DestroyProjectile()
    {
        thisColl.enabled = false; // need to destroy or hide&disable coll first to stop interacting with rems
        isDestroyed = true;
        Debug.Log("firing destroy projectile");
        // instantiate the leftover rocks
        
        largeRemnant = Instantiate(large_possibleRemnantObjects[UnityEngine.Random.Range(0, large_possibleRemnantObjects.Length)],
                                    this.transform.position, this.transform.rotation);

        // it seems to be spawing these rems from where the player is?? or something??


        // start the checking of the rock settlements
        StartCoroutine("SettleRemnants");

        // play a particle effect

        // actually destroying will break the coroutines
        // potentially just make invis etc and then destroy at end of coroutine?
        // object pooling may want otherwise
        //Destroy(this.gameObject);
    }

    public IEnumerator SettleRemnants()
    {
        var allsettled = false;

        var lrb = largeRemnant.GetComponent<Rigidbody>();
        if (!lrb.isKinematic)
        {
            if(lrb.IsSleeping())
            {
                Debug.Log("setting kinem");
                lrb.isKinematic = true;
                allsettled = true;
                
            }
        }

        /*
        foreach(GameObject go in smallRemnants)
        {
            Debug.Log("rem go = " + go);
            var rb = go.GetComponent<Rigidbody>();
            if(!rb.isKinematic)
            {
                if (rb.IsSleeping())
                {
                    Debug.Log("rb sleeping");
                    rb.isKinematic = true;
                    // this will not end the coroutine here, it will come back and check one more time, probably nbd.
                }
                else
                {
                    allsettled = false;
                }
                // if it's on the ground -> set kinematic
                // else -> allsettled = false;
            }
        }
        */
        if (allsettled)
        {
            Debug.Log("ending coroutine");
            yield break; // end the coroutine if everything is settled

        }
        yield return null; //new WaitForSeconds(0.1f);
    }
}