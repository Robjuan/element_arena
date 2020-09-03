using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

public class EarthProjectile : ProjectileBase
{
    [Header("Earth Projectile Properties")]
    // palette: https://www.color-hex.com/color-palette/3392
    public Color ignitionColor = new Color32(191, 18, 0, 255);

    public float damageScale;

    [Header("Remnants")]
    public GameObject shatterEffect;
    public GameObject explosionEffect;
    [Range(0.05f, 1f)] public float effectScale = 0.3f; 
    [Tooltip("not a percentage, relative model scale determines. ~35 is good default")]
    [Range(20, 100)] public float largeRemnantScale = 38f;
    [Range(5, 50)] public float smallRemnantScale = 10f;
    [Tooltip("Random number of small rems in this range")]
    public int smallRemnantCountFloor = 0;
    public int smallRemnantCountCeiling = 10;
    [Range(1, 10)] public float smallRemScatterRadius = 2f;
    public GameObject[] small_possibleRemnantObjects;
    
    public GameObject[] large_possibleRemnantObjects;

    private GameObject largeRemnant;
    private List<GameObject> smallRemnants = new List<GameObject>();

    private Vector3 lastLocalScale;
    private float lastTemp;


    public override float GetDamage(Collision coll)
    {
        //Debug.Log("mass = " + rigidBody.mass + ", temp = " + temperature + ", v = " + rigidBody.velocity.magnitude);
        //  dot product of collision normal and collision velocity,  times the mass of the other collider 

        var cp = coll.GetContact(0);
        var collEnergy = Vector3.Dot(cp.normal,coll.relativeVelocity);
        Debug.Log("colliding for damage: " + (collEnergy * rigidBody.mass * thermals.Temperature * damageScale));
        return collEnergy * rigidBody.mass * thermals.Temperature * damageScale;
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
                //inspector values are off
                float emission_min = 5f;
                float emission_max = 10f;

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
        isDestroyed = true;
        Vector3 spawnposition = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        // hide / remove the existing projectile
        // we will only setinactive when the coroutine is finished
        // object pooling may warrant something different here, like the coroutine to be managed by one of the remnants themselves
        outerSphereRend.enabled = false;
        innerSphereRend.enabled = false;
        thisColl.enabled = false;


        // fire the effect
        GameObject effect;
        if (thermals.isIgnited())
        {
            effect = Instantiate(explosionEffect, spawnposition, transform.rotation);
        } else
        {
            effect = Instantiate(shatterEffect, spawnposition, transform.rotation);
        }
        effect.transform.localScale = transform.localScale * effectScale;


        // instantiate the leftover rocks
        largeRemnant = Instantiate(large_possibleRemnantObjects[UnityEngine.Random.Range(0, large_possibleRemnantObjects.Length)], spawnposition, transform.rotation);
            // todo: add force?

        // at projectile localscale = 1, rock filling most of projectile is localscale = 31
        // this scaling number is set for aesthetics
        largeRemnant.transform.localScale = transform.localScale * largeRemnantScale;
        // choose how many small rems to spawn
        // todo: more / less rems based on how big we are
        // todo: the more rems there are the smaller each should be (within a range)
        var smallRemCount = UnityEngine.Random.Range(smallRemnantCountFloor, smallRemnantCountCeiling);

        for (int i = 0; i < smallRemCount; i++)
        {
            // randomly select one from the list of possibles
            var remselected = UnityEngine.Random.Range(0, small_possibleRemnantObjects.Length);
            // spawn it where we exploded
            var newsmallrem = Instantiate(small_possibleRemnantObjects[remselected], spawnposition, transform.rotation);

            // randomly place it on a sphere of radius rad
            var rad = smallRemScatterRadius;// + largeRemnant.GetComponent<Collider>().bounds.extents.magnitude;
            var centre = spawnposition;
            Vector3 spherePos = (UnityEngine.Random.onUnitSphere * rad) + centre;
            newsmallrem.transform.position = spherePos;
            // scale the small rem
            newsmallrem.transform.localScale = transform.localScale * smallRemnantScale;
            
                // todo: add force?

            // keep track of all smallrems for settling
            smallRemnants.Add(newsmallrem);
        }
        
        // start the checking of the rock settlements
        StartCoroutine("SettleRemnants");
    }

    public IEnumerator SettleRemnants()
    {
        for(; ; ) // loop forever because coroutine
        {
            var largeSettled = false;

            var lrb = largeRemnant.GetComponent<Rigidbody>();
            if (!lrb.isKinematic)
            {
                if (lrb.IsSleeping())
                {
                    lrb.isKinematic = true;
                    largeSettled = true;
                }
            }

            var allSmallSettled = true;
            foreach (GameObject go in smallRemnants)
            {
                var rb = go.GetComponent<Rigidbody>();
                if(!rb.isKinematic)
                {
                    if (rb.IsSleeping())
                    {
                        rb.isKinematic = true;
                        // this will not end the coroutine here, it will come back and check one more time, probably nbd.
                    }
                    else
                    {
                        allSmallSettled = false;
                    }
                }
            }
            
            if (largeSettled && allSmallSettled)
            {
                this.gameObject.SetActive(false);
                yield break; // end the coroutine if everything is settled

            }
            yield return null; //new WaitForSeconds(0.1f);
        }

    }

    public void OnDrawGizmos()
    {
        try
        {
            Gizmos.DrawWireSphere(transform.position, GetRadius());
        }
        catch 
        {
            Debug.Log("can't draw gizmo?");
        }   
        
    }
}