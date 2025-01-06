using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    
    [HideInInspector]public Transform respawnPoint;
    [HideInInspector] public GameObject currentPlayer;
    [HideInInspector] public int chooseSkinId;
    [HideInInspector] public int fruits;

    public InGame_UI inGameUI;
    [Header("Player info")]
    [SerializeField] private GameObject fruitPrefab;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject deathFX;

    [Header("Camera Shake FX")]
    [SerializeField] private CinemachineImpulseSource impulse;
    [SerializeField] private Vector3 shakeDirection;
    [SerializeField] private float forceMultiplier;

    public void ScreenShake(int facingDir)
    {
        impulse.m_DefaultVelocity = new Vector3(shakeDirection.x * facingDir, shakeDirection.y) * forceMultiplier;
        impulse.GenerateImpulse();
    }

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RespawnPlayer();
        }
    }

    private bool HaveEnoughFruits()
    {
        if (fruits > 0)
        {
            fruits--;

            if (fruits <= 0)
                fruits = 0;

            DropFruit();
            return true;

        }
        return false;

    }

    private void DropFruit()
    {
        int fruitIndex = UnityEngine.Random.Range(0, Enum.GetNames(typeof(FruitType)).Length);
        GameObject newFruit = Instantiate(fruitPrefab , currentPlayer.transform.position , transform.rotation);
        newFruit.GetComponent<Fruit_DropedByPlayer>().FruitSetup(fruitIndex);
        Destroy(newFruit,20);
    }

    public void OnTakingDamage()
    {
        if (!HaveEnoughFruits())
        {
            KillPlayer();
            

            if (GameManager.instance.difficulty < 3)
                Invoke("RespawnPlayer", 1);
            else
                inGameUI.OnDeath();

        }
        
    }

    public void OnFalling()
    {
        KillPlayer();
        int difficulty = GameManager.instance.difficulty;

        if (difficulty < 3)
        {
            Invoke("RespawnPlayer", 1);

            if (difficulty > 1)
                HaveEnoughFruits();
        }
        else
            inGameUI.OnDeath();
    }

    public void RespawnPlayer()
    {
        if (currentPlayer == null)
        {
            AudioManager.instance.PlaySFX(11);
            currentPlayer = Instantiate(playerPrefab, respawnPoint.position, transform.rotation);
        }
    }

    public void KillPlayer()
    {
        AudioManager.instance.PlaySFX(0);
        GameObject newDeathFX = Instantiate(deathFX, currentPlayer.transform.position, currentPlayer.transform.rotation);
        Destroy(newDeathFX , .4f);
        Destroy(currentPlayer);
    }

}
