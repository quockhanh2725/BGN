using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Trunk : Enemy
{
    [Header("truck specifics")]
    [SerializeField] private float moveBackTime;
    private float retreatTimeCounter;

    [Header("collision specifics")]
    [SerializeField] private float checkRadius;
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private Transform groundBehindCheck;
    private bool wallBehind;
    private bool groundBehind;

    private bool playerDetected;

    [SerializeField] private float attackCooldown;
                     private float attackCooldownCounter;


    [Header("bullet specifics")]
    [SerializeField] private float bulletSpeed;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletOrigin;
    protected override void Start()
    {
        base.Start();
    }


    void Update()
    {
        CollisionChecks();

        attackCooldownCounter -= Time.deltaTime;
        retreatTimeCounter -= Time.deltaTime;

        if (!canMove)
            rb.velocity = new Vector2(0, 0);

        if (playerDetected && retreatTimeCounter < 0)
            retreatTimeCounter = moveBackTime;

        if (playerDetection.collider.GetComponent<Player>() != null)
        {
            if (attackCooldownCounter < 0)
            {
                attackCooldownCounter= attackCooldown;
                anim.SetTrigger("attack");
                canMove = false;
            }
            else if(playerDetection.distance < 3 )
            {
                
                MoveBackwards(1.5f);
            }
        }
        else
        {
            if (retreatTimeCounter > 0)
                MoveBackwards(0.5f);
            else
                WalkAround();
        }

        anim.SetFloat("xVelocity", rb.velocity.x);
    }

    private void MoveBackwards(float multiplier)
    {
        if (wallBehind)
            return;

        if(!groundBehind)
            return ;

        rb.velocity = new Vector2 (rb.velocity.x + multiplier * - facingDirection, rb.velocity.y ); // -facingDirection

    }

    private void AttackEvent()
    {
        GameObject newBullet = Instantiate(bulletPrefab, bulletOrigin.transform.position, bulletOrigin.transform.rotation);
        newBullet.GetComponent<Bullet>().SetupSpeed(bulletSpeed * facingDirection, 0);
        Destroy(newBullet, 3f);
    }

    private void ReturnMovement()
    {
        canMove = true;
    }

    protected override void CollisionChecks()
    {
        base.CollisionChecks();
        playerDetected = Physics2D.OverlapCircle(transform.position, checkRadius, whatIsPlayer);
        groundBehind  = Physics2D.Raycast(groundBehindCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
        wallBehind = Physics2D.Raycast(wallCheck.position, Vector2.right * (- facingDirection + 1), wallCheckDistance, whatIsGround);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawWireSphere(transform.position, checkRadius);
        Gizmos.DrawLine(groundBehindCheck.position, new Vector2(groundBehindCheck.position.x, groundBehindCheck.position.y - groundCheckDistance));
    }
}
