using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap_Fire_Swicher : MonoBehaviour
{
    public Trap_Fire myTrap;
    private Animator anim;
    [SerializeField] private float timeNotActive = 2 ;
    public void Start()
    {
        anim = GetComponent<Animator>(); 
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        
        if(collision.GetComponent<Player>() != null)
        {
            anim.SetTrigger("pressed");
            myTrap.FireSwichAfter(timeNotActive);
        }
    }
}
