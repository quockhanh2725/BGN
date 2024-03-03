using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bgn : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator anim;


    [Header("Moving info")]
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpForce;
    float movingInput;
    private bool canDoubleJump;


    [Header("Collision info")]
    [SerializeField]private LayerMask whatIsGround;
    [SerializeField] float checkGroundedDistance;
    private bool isGrounded;

    private bool isMoving;
    private float yVelocity;
    private bool facingFlip;
    private float facingDestance;
    void Start()
    {
        
    }

    void Update()
    {
        
        Animation();
        CheckCollision();
        CheckMoving();
        ResetDoubleJump();
        if (movingInput > 0 && facingFlip)
            Flip();
        else if (movingInput < 0 && !facingFlip)
            Flip();
    }

    private void Flip()
    {
        facingFlip = !facingFlip;
        transform.Rotate(0, 180, 0);
    }

    private void Animation()
    {
        isMoving = rb.velocity.x != 0;
        anim.SetBool("isMoving", isMoving);
        anim.SetFloat("yVelocity",yVelocity);
        anim.SetBool("isGrounded",isGrounded);
    }

    private void CheckCollision()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, checkGroundedDistance, whatIsGround);
    }

    private void CheckMoving()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space))
        {
            JumpDouble();
        }
        Moving();
    }

    private void JumpDouble()
    {
        if (isGrounded)
            Jump();
        else if (canDoubleJump)
        {
            canDoubleJump = false;
            Jump();
        }
    }

    private void Moving()
    {
        movingInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(movingInput * moveSpeed, rb.velocity.y);
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x , jumpForce);
    }

    private void ResetDoubleJump()
    {
        if (isGrounded)
            canDoubleJump = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x , transform.position.y - checkGroundedDistance));
    }
}
