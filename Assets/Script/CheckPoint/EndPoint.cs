using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPoint : MonoBehaviour
{
    private InGame_UI inGame_UI;
    private void Start()
    {
        inGame_UI = GameObject.Find("Canvas").GetComponent<InGame_UI>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            GetComponent<Animator>().SetTrigger("activate");

            PlayerManager.instance.KillPlayer();
            AudioManager.instance.PlaySFX(3);
            inGame_UI.OnLevelFinished();
            GameManager.instance.SaveBestTime();
            GameManager.instance.SaveCollectedFruits();
            GameManager.instance.SaveLevelInfo();
        }
    }
}
