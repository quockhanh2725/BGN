using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D rg;
    public Animator anim;

    [Header("Moving info")]
    public float moveSpeed;
    public float jumpForce;
    public Vector2 wallJumpDirection;
    float movingInput;
    private bool canDoubleJump = true;
    private bool canMove;

    [Header("Collision info")]
    public LayerMask whatIsGround;
    public float groundCheckDistance;
    public float wallCheckDistance;
    private bool isGrounded;
    private bool isWallDeteced;
    private bool canWallSlide;
    private bool isWallSliding;


    private bool facingRight = true;
    private int facingDerection = 1;

    void Start()
    {
        rg = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        AnimationControler();
        FlipControler();
        CollisionChecks();
        InputChecks();

        if (isGrounded)
        {
            canDoubleJump = true;
            canMove = true;

        }


        if (canWallSlide)
        {
            isWallSliding = true;
            rg.velocity = new Vector2(rg.velocity.x, rg.velocity.y * 0.1f);
        }    
            Move();
        
    }

    private void wallJump()
    {
        canMove = false;
        rg.velocity = new Vector2(wallJumpDirection.x * -facingDerection , wallJumpDirection.y);
    }
    private void AnimationControler()
    {
        bool isMoving = rg.velocity.x != 0;
        anim.SetBool("isMoving", isMoving);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isWallSlidin", isWallSliding);
        anim.SetBool("isWallDetected", isWallDeteced);
        anim.SetFloat("yVelocity", rg.velocity.y);
    }

    private void InputChecks()
    {
        movingInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetAxis("Vertical") < 0)
            canWallSlide = false; 


        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W))
            JumpButtom();
        
    }

    private void Flip()
    {
        facingDerection = facingDerection * -1;
        facingRight = !facingRight;
        transform.Rotate(0,180,0);
    }

    private void FlipControler()
    {
        if(facingRight && rg.velocity.x < 0)
        {
            Flip();
        }
        else if(!facingRight && rg.velocity.x > 0)
        {
            Flip(); 
        }
    }

    private void JumpButtom()
    {
        if (isWallSliding)
            wallJump();
        else if (isGrounded)
        {
            Jump();
        }
        else if (canDoubleJump)
        {
            canDoubleJump = false;
            Jump();
        }

        canWallSlide = false;
    }

    private void CollisionChecks()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
        // Raycast (tọa độ nhân vật , hướng raycase , khoảng cách , xem đối tượng kích hoạt raycase)
        isWallDeteced = Physics2D.Raycast(transform.position , Vector2.right * facingDerection , wallCheckDistance , whatIsGround);


        if (isWallDeteced && rg.velocity.y < 0)
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
            rg.velocity = new Vector2(moveSpeed * movingInput, rg.velocity.y); // getAxits : trả về số gần đúng  getAxitsRaw : trả về số đúng (-1 , 0 , 1 )
    }

    private void Jump()
    {
        rg.velocity = new Vector2(rg.velocity.x, jumpForce);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - groundCheckDistance));
        //hiển thị thực quan các trục trong chỉnh sửa
        Gizmos.DrawLine(transform.position , new Vector3(transform.position.x + wallCheckDistance * facingDerection , transform.position.y));
 
    }
}