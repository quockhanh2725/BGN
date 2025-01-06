using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator anim;

    [Header("Particles")]
    [SerializeField] private ParticleSystem dustFX;
    private float dustFXTimer;

    [Header("Moving info")]
    public float moveSpeed;
    public float jumpForce;
    public Vector2 wallJumpDirection;
    public float doubleJumpForce;
    private bool canDoubleJump = true;
    private bool canMove;
    private float movingInput;

    private bool canBeControlled;

    private bool readyToLand;

    private float defaultJumpForce;

    [SerializeField] private float bufferJumpTime;
                     private float bufferJumpCounter;

    [SerializeField] private float cayoteJumpTime;
                     private float cayoteJumpCounter;
                     private bool canHaveCayoteJump;

    [Header("Knockback info")]
    [SerializeField] private Vector2 knockbackDirection;
    [SerializeField] private float knockbackTime;
    [SerializeField] private float knockbackProtectionTime;

                     private bool isKnocked;
                     private bool canbeKnocked = true;

    [Header("Collision info")]
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask whatIsWall;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private Transform enemyCheck;
    [SerializeField] private float enemyCheckRadius;
    private bool isGrounded;
    private bool isWallDeteced;
    private bool canWallSlide;
    private bool isWallSliding;


    private bool facingRight = true;
    private int facingDirection = 1;

    private float defaultGravityScale;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        SetAnimationLayer();
        defaultJumpForce = jumpForce;

        defaultGravityScale = rb.gravityScale;
        rb.gravityScale = 0;
    }

    void Update()
    {
        AnimationControler();

        if (isKnocked)
            return;

        FlipControler();
        CollisionChecks();
        InputChecks();
        CheckForEnemy();

        bufferJumpCounter -= Time.deltaTime;
        cayoteJumpCounter -= Time.deltaTime;

        if (isGrounded)
        {
            canDoubleJump = true;
            canMove = true;

            if (bufferJumpCounter > 0)
            {
                bufferJumpCounter = -1;
                Jump();
            }

            canHaveCayoteJump = true;

            if (readyToLand)
            {
                dustFX.Play();
                readyToLand = false;
            }

        }
        else
        {
            if(!readyToLand)
                readyToLand = true;

            if (canHaveCayoteJump)
            {
                canHaveCayoteJump = false;
                cayoteJumpCounter = cayoteJumpTime;
            }

        }


        if (canWallSlide)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.1f);
        }
        Move();

    }

    private void CheckForEnemy()
    {
        Collider2D[] hitedCollider = Physics2D.OverlapCircleAll(enemyCheck.position, enemyCheckRadius);

        foreach (var enemy in hitedCollider)
        {
            if (enemy.GetComponent<Enemy>() != null)
            {
                Enemy newEnemy = enemy.GetComponent<Enemy>();

                if (newEnemy.invincible)
                    return;

                if (rb.velocity.y < 0)
                {
                    AudioManager.instance.PlaySFX(1);
                    newEnemy.Damge();
                    anim.SetBool("flipping" , true);
                    Jump();
                }
            }
        }
    }
    private void stopFlippingAnimation()
    {
        anim.SetBool("flipping", false);
    }

    private void WallJump()
    {
        canMove = false;
        rb.velocity = new Vector2(wallJumpDirection.x * -facingDirection , wallJumpDirection.y);

        dustFX.Play();
    }
    private void AnimationControler()
    {
        bool isMoving = rb.velocity.x != 0;
        anim.SetBool("isKnocked", isKnocked);
        anim.SetBool("isMoving", isMoving);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isWallSlidin", isWallSliding);
        anim.SetBool("isWallDetected", isWallDeteced);
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("canBeControlled", canBeControlled);
    }

    private void InputChecks()
    {
        if (!canBeControlled)
            return;

        movingInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetAxis("Vertical") < 0)
            canWallSlide = false; 


        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W))
            JumpButtom();
        
    }

    private void ReturnControll()
    {
        rb.gravityScale = defaultGravityScale;
        canBeControlled = true;
    }

    private void Flip()
    {
        if (dustFXTimer < 0)
        {
            dustFX.Play();
            dustFXTimer = .7f;
        }
        facingDirection = facingDirection * -1;
        facingRight = !facingRight;
        transform.Rotate(0,180,0);
    }

    private void FlipControler()
    {
        dustFXTimer -= Time.deltaTime;

        if(facingRight && rb.velocity.x < 0)
        {
            Flip();
        }
        else if(!facingRight && rb.velocity.x > 0)
        {
            Flip(); 
        }
    }

    private void SetAnimationLayer()
    {
        int skinIndex = PlayerManager.instance.chooseSkinId;

        for (int i = 0; i < anim.layerCount; i++)
        {
            anim.SetLayerWeight(i, 0);
        }

        anim.SetLayerWeight(skinIndex, 1);
    }

    private void JumpButtom()
    {
        if (!isGrounded)
            bufferJumpCounter = bufferJumpTime;

        if (isWallSliding)
        {
            WallJump();
            canDoubleJump = true;
        }
        else if (isGrounded || canHaveCayoteJump)
        {
            Jump();
        }
        else if (canDoubleJump)
        {
            canMove = true;
            canDoubleJump = false;
            jumpForce = doubleJumpForce;
            Jump();
            jumpForce = defaultJumpForce;
        }

        canWallSlide = false;
    }


    public void Knockback(Transform damgeTransform)
    {
        AudioManager.instance.PlaySFX(10);
        if (!canbeKnocked)
            return;

        if (GameManager.instance.difficulty > 1)
        {
            PlayerManager.instance.OnTakingDamage();
        }

        PlayerManager.instance.ScreenShake(-facingDirection);
        isKnocked = true;
        canbeKnocked = false;


        int hDirection = 0;
        if (transform.position.x > damgeTransform.transform.position.x)
        {
            hDirection = 1;
        }
        else if (transform.position.x < damgeTransform.transform.position.x)
        {
            hDirection = -1;
        }

        rb.velocity = new Vector2(knockbackDirection.x * hDirection, knockbackDirection.y);

        Invoke("CancelKnockback", knockbackTime);
        Invoke("AllowKnockback", knockbackProtectionTime);
    }

    private void CancelKnockback()
    {
        isKnocked = false;
    }

    private void AllowKnockback()
    {
        canbeKnocked = true;
    }

    private void CollisionChecks()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
        // Raycast (tọa độ nhân vật , hướng raycase , khoảng cách , xem đối tượng kích hoạt raycase)
        isWallDeteced = Physics2D.Raycast(transform.position , Vector2.right * facingDirection , wallCheckDistance , whatIsWall);


        if (isWallDeteced && rb.velocity.y < 0)
            canWallSlide = true;


        if (!isWallDeteced)
        {
            isWallSliding = false;
            canWallSlide = false;
        }
    }

    private void Move()
    {
        if (canMove)
            rb.velocity = new Vector2(moveSpeed * movingInput, rb.velocity.y); // getAxits : trả về số gần đúng  getAxitsRaw : trả về số đúng (-1 , 0 , 1 )
    }

    private void Jump()
    {
        AudioManager.instance.PlaySFX(4);
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);

        if(isGrounded)
            dustFX.Play();
    }

    public void Push(float pushForce)
    {
        rb.velocity = new Vector2(rb.velocity.x, pushForce);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - groundCheckDistance));
        //hiển thị thực quan các trục trong chỉnh sửa
        Gizmos.DrawLine(transform.position , new Vector3(transform.position.x + wallCheckDistance * facingDirection , transform.position.y));
        Gizmos.DrawWireSphere(enemyCheck.position, enemyCheckRadius);
 
    }
}