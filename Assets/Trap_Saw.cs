using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap_Saw : Trap
{
    private Animator anim;
    private bool isWoking;


    [SerializeField] private Transform[] movePoint;
    [SerializeField] private float speed;
    [SerializeField] private float countdown;


    private int movePointIndex;
    private float countdownTime;

    void Start()
    {
        anim = GetComponent<Animator>();
        transform.position = movePoint[0].position;
    }

 
    void Update()
    {
        countdownTime -= Time.deltaTime;
        bool isWoking = countdownTime < 0;

        anim.SetBool("isWoking", isWoking);

        if(isWoking)
        transform.position = Vector3.MoveTowards(transform.position, movePoint[movePointIndex].position, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position , movePoint[movePointIndex].position) < 0.15f)
        {
            Flip();
            countdownTime = countdown;
            movePointIndex++;

            if (movePointIndex >= movePoint.Length)
                movePointIndex = 0;
        }
    }

    private void Flip()
    {
        transform.localScale = new Vector3(1, transform.localScale.y * -1);
    }
}
