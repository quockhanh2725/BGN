using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
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

        if (isWoking)
            transform.position = Vector3.MoveTowards(transform.position, movePoint[movePointIndex].position, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, movePoint[movePointIndex].position) < 0.15f)
        {
            countdownTime = countdown;
            movePointIndex++;

            if (movePointIndex >= movePoint.Length)
                movePointIndex = 0;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            collision.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit2D (Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            collision.transform.SetParent(null);
        }
    }
}
