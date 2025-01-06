using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Danger
{
    protected Animator anim;
    protected Rigidbody2D rb;

    protected int facingDirection = -1;

    [SerializeField] protected LayerMask whatIsGround;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected float wallCheckDistance;

    protected bool wallDetected;
    protected bool groundDetected;

    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected Transform wallCheck;

    protected Transform player;
    [HideInInspector]public bool invincible;

    [SerializeField] protected LayerMask whatToIgnore;
    protected bool aggresive;
    protected RaycastHit2D playerDetection;


    [Header("Move info")]
    [SerializeField] protected float speed;
    [SerializeField] protected float idleTime;
                     protected float idleTimeCounter;


    protected bool canMove = true;

    protected virtual void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("FindPlayer", 0, .5f);
        FindPlayer();

        if (groundCheck == null)
            groundCheck = transform;

        if (wallCheck == null)
            wallCheck = transform;
    }

    private void FindPlayer()
    {
        if(player != null)
            return;


        if (PlayerManager.instance.currentPlayer != null)
            player = PlayerManager.instance.currentPlayer.transform;
    }

    public virtual void Damge()
    {
        if (!invincible)
        {
            canMove = false;
            anim.SetTrigger("gotHit");
        }
    }


    protected virtual void WalkAround()
    {
        if (idleTimeCounter <= 0 && canMove)
        {
            rb.velocity = new Vector2(speed * facingDirection, rb.velocity.y);
        }
        else
            rb.velocity = Vector2.zero;

        idleTimeCounter -= Time.deltaTime;

        if (wallDetected || !groundDetected)
        {
            idleTimeCounter = idleTime;
            Flip();
        }
    }


    public void DestroyMe()
    {
        Destroy(gameObject);
    }


    protected virtual void Flip()
    {
        facingDirection = facingDirection * -1;
        transform.Rotate(0, 180, 0);
    }


    protected virtual void CollisionChecks()
    {
        groundDetected = Physics2D.Raycast(groundCheck.position , Vector2.down , groundCheckDistance , whatIsGround);
        wallDetected = Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, wallCheckDistance, whatIsGround);
        playerDetection = Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, 50, ~whatToIgnore);
    }


    protected virtual void OnDrawGizmos()
    {
        if(groundCheck != null)
            Gizmos.DrawLine(groundCheck.position , new Vector2(groundCheck.position.x , groundCheck.position.y - groundCheckDistance));
        if(wallCheck != null)
        {
            Gizmos.DrawLine(wallCheck.position, new Vector2(wallCheck.position.x + wallCheckDistance * facingDirection , wallCheck.position.y));
            Gizmos.DrawLine(wallCheck.position, new Vector2(wallCheck.position.x + playerDetection.distance * facingDirection, wallCheck.position.y));

        }
    }
}
