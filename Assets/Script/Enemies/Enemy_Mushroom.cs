using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Mushroom : Enemy
{


    protected override void Start()
    {
        base.Start();
    }

    private void Update()
    {
        anim.SetFloat("xVelocity" , rb.velocity.x);
        WalkAround();
        CollisionChecks();
    }

}
