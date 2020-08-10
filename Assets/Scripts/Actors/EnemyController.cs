using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Animator anim;

    public float touchDamage;
    public Transform walkTarget;
    public float walkSpeed;
    public bool IsAlive { get; set; }

    private Renderer[] rends;

    private void Awake() 
    {
        IsAlive = true;
        // npc has two rends, one each for head / body
        rends = GetComponentsInChildren<Renderer>();
    }

    public void SetWalkTarget(Transform wt)
    {
        this.walkTarget = wt;
    }

    public void Die()
    {
        if (IsAlive && this.gameObject)
        {
            
            IsAlive = false;
            foreach(Renderer rend in rends)
            {
                rend.material.color = Color.red;
            }

            if (anim)
            {
                anim.SetTrigger("Die");
            } else
            {
                Destroy(this.gameObject, 2f);
            }
        }

    }

    void FixedUpdate()
    {
        if (IsAlive)
        {

            // todo: make this better, use more robust movement than MoveTowards
            // slope will be float (-1,1) based on (uphill,downhill)
            var slope = 0f;
            RaycastHit hit;
            if (Physics.SphereCast(transform.position, .25f, Vector3.down, out hit, 3f))
            {
                slope = Vector3.Dot(transform.forward, (Vector3.Cross(Vector3.up, hit.normal)));
            }
            var adjustedSpeed = walkSpeed;
            if (slope != 0)
            {
                adjustedSpeed = walkSpeed / slope;
            }

            transform.LookAt(walkTarget.transform);
            transform.position = Vector3.MoveTowards(transform.position, walkTarget.position, adjustedSpeed * Time.deltaTime);
        }
    }

    protected void OnCollisionEnter(Collision coll)
    {
        var other = coll.gameObject;
        // todo: let them damage each other?
        // maybe with better nav and more varied attacks
        if (other.tag == "Player")
        {
            if (anim)
            {
                anim.SetBool("Attack", true);
            }

            Damageable damageable = other.GetComponent<Damageable>();
            if (damageable)
            {
                damageable.InflictDamage(touchDamage, this.gameObject);
            }
        }
    }


    void Update()
    {

    }
}
