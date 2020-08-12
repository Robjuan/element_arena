using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;

public class EnemyController : ActorController
{
    [Header("Subclass members")]
    public float touchDamage;
    public Transform walkTarget;
    public float walkSpeed;
    public override bool IsAlive { get; set; }

    private Renderer[] rends;

    private void Awake() 
    {
        IsAlive = true;
        // npc has two rends, one each for head / body
        rends = GetComponentsInChildren<Renderer>();
    }

    void Start()
    {
        if(aiSateController)
        {
            aiSateController.SetupAI(true);
        }
    }

    public void SetWalkTarget(Transform wt)
    {
        this.walkTarget = wt;
    }

    public override void Die()
    {
        if (IsAlive && this.gameObject)
        {
            
            IsAlive = false;
            foreach(Renderer rend in rends)
            {
                rend.material.color = Color.red;
            }

            if (aiSateController)
            {
                aiSateController.Deactivate();
            }

            if (anim)
            {
                anim.SetTrigger("Die");
                this.GetComponent<BoxCollider>().enabled = false;
            } else
            {
                Destroy(this.gameObject, 2f);
            }

        }

    }

    void RawMoveToLocation()
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

    void FixedUpdate()
    {
        if (IsAlive)
        {
            //RawMoveToLocation();
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

    protected void OnCollisionExit(Collision coll)
    {
        if (anim)
        {
            anim.SetBool("Attack", false);
        }
    }

    public override void ReceiveDamage()
    {
        if(anim)
        {
            anim.SetTrigger("ReceiveDamage");
        }
    }

    void Update()
    {
    }
}
