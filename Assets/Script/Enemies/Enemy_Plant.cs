using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Plant : Enemy
{
    [Header("Plant spesifics")]

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletOrigin;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private bool facingRight;
    protected override void Start()
    {
        base.Start();
        if (facingRight)
            Flip();
    }


    // Update is called once per frame
    void Update()
    {
         
        idleTimeCounter -= Time.deltaTime;
        CollisionChecks();

        if (!playerDetection)
            return;
        bool playerDetected = playerDetection.collider.GetComponent<Player>() != null;

        if (idleTimeCounter < 0 && playerDetected)
        {
            
            anim.SetTrigger("attack");
            idleTimeCounter = idleTime;
        }
    }

    private void AttackEvent()
    {
        GameObject newBullet = Instantiate(bulletPrefab, bulletOrigin.transform.position, bulletOrigin.transform.rotation);
        newBullet.GetComponent<Bullet>().SetupSpeed(bulletSpeed * facingDirection, 0);
        Destroy(newBullet , 5f);
    }
}
