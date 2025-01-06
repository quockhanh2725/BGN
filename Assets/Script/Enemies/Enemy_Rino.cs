using UnityEngine;

public class Enemy_Rino : Enemy
{

    [Header("rino specific")]
    [SerializeField] private float agroSpeed;
    [SerializeField] private float shockTime;
                     private float shockTimeCouter;

    

    protected override void Start()
    {
        base.Start();
        invincible = true;
    }


    // Update is called once per frame
    void Update()
    {

        AnimatorControllers();
        CollisionChecks();

        if (!playerDetection)
        {
            WalkAround();
            return; 
        }

        //playerDetection = Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, 50, ~whatToIgnore);
        if (playerDetection.collider.GetComponent<Player>() != null)
        {
            aggresive = true;
        }

        if (!aggresive)
        {
            WalkAround();
        }
        else
        {
            if (!groundDetected)
            {
                Flip();
                aggresive = false;
            }
            rb.velocity = new Vector2(agroSpeed * facingDirection, rb.velocity.y);

            if (wallDetected && invincible) // tong tuong
            {
                invincible = false;
                shockTimeCouter = shockTime;
            }


            if (shockTimeCouter <= 0 && !invincible) // het tong tuong
            {
                invincible = true;
                Flip();
                aggresive = false;
            }

            shockTimeCouter -= Time.deltaTime;

        }
        

    }

    private void AnimatorControllers()
    {
        anim.SetFloat("xVelocity", rb.velocity.x);
        anim.SetBool("invincible", invincible);
    }

    //protected override void OnDrawGizmos()
    //{
    //    base.OnDrawGizmos();
    //    Gizmos.DrawLine(wallCheck.position, new Vector2(wallCheck.position.x + playerDetection.distance * facingDirection, wallCheck.position.y));
    //}
}
