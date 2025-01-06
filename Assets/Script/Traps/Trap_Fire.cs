using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap_Fire : Danger
{
    public bool isWorking;
    private Animator anim;
    public float repeatRate;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        if(transform.parent == null)
            InvokeRepeating("FireSwich", 0, repeatRate);
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("isWorking", isWorking);
    }

    public void FireSwich()
    {
        isWorking = !isWorking;
    }
    public void FireSwichAfter(float seconds)
    {
        CancelInvoke();
        isWorking = false;
        Invoke("FireSwich", seconds);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (isWorking)
            base.OnTriggerEnter2D(collision);
    }
}
