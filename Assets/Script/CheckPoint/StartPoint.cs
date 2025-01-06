using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPoint : MonoBehaviour
{

    private void Awake()
    {
        PlayerManager.instance.respawnPoint = transform;
        PlayerManager.instance.RespawnPlayer();
    }

    private void Start()
    {
        AudioManager.instance.PlayBGMRandom();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
        if (collision.GetComponent<Player>() != null)
        {
            if (!GameManager.instance.startTime)
                GameManager.instance.startTime = true;

            if(collision.transform.position.x > transform.position.x)
                GetComponent<Animator>().SetTrigger("touch");
        }
    }
}
