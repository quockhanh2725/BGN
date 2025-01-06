using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_BlueBird : Enemy
{
    private RaycastHit2D ceillingDetected;


    [Header("blue bird specific")]

    [SerializeField] private float ceillingDistance;
    [SerializeField] private float groundDistance;


    [SerializeField] private float flyUpForce;
    [SerializeField] private float flyDownForce;
                     private float flyForce;

    private bool canFly = true;
    public override void Damge()
    {
        canFly = false;
        rb.velocity = new Vector2(0, 0);
        rb.gravityScale = 0;
        base.Damge();
    }

    protected override void Start()
    {
        base.Start();
        flyForce = flyUpForce;
    }
    private void Update()
    {
        CollisionChecks();

        if (ceillingDetected)
            flyForce = flyDownForce;
        else if (groundDetected)
            flyForce = flyUpForce;


        if (wallDetected)
            Flip();
    }

    [SerializeField] private Transform movePoint;
    [SerializeField] private float xMultiplier;
    [SerializeField] private float yMultiplier;
    public void FlyUpEvent() {
            if (canFly)
                rb.velocity = new Vector2(speed * facingDirection, flyForce);
            //if (canFly)
            //{
            //    Vector2 direction = transform.position - movePoint.position;
            //    rb.velocity = new Vector2(-direction.x * xMultiplier, -direction.y * yMultiplier);
            //}
        }

    protected override void CollisionChecks()
    {
        base.CollisionChecks();
        ceillingDetected = Physics2D.Raycast(transform.position, Vector2.up, ceillingDistance, whatIsGround);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y + ceillingDistance));
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - groundDistance));
    }
}
