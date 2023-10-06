using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bebech : Enemy
{
    [Header("Bebzol Settings")]
    [SerializeField] float knockbackDamage = 1;
    [SerializeField] float knockbackHight = 10;
    [SerializeField] float movementBoost = 5;
    [SerializeField] float healingSlow = 0.5f;
    [SerializeField] float healingValue = 3;
    [SerializeField] float startHealingPercent = 10;

    [SerializeField] float shootCD = 0.5f; 
    [SerializeField] float chargeCD = 0.5f;
    [SerializeField] float reloadTime = 1f;
    [SerializeField] float meleeRange = 1f;
    [SerializeField] float fastCokeSpeed = 2f;
    [SerializeField] List<float> Shoot2CokeRotation;
    [SerializeField] GameObject cokeDummyBIG;
    [SerializeField] GameObject cokeDummyBullet;
    [SerializeField] List<GameObject> cokeDummyAmmo;
    int AmmoCount = 0;
    bool ready = true;
    bool chargeReady = true;
    bool reloading = false;
    bool charging = false;
    bool reloadCancelled = false;
    bool bossActivated = false;
    bool meleeAttackReady = false;
    float reloadingTimer = 0f;
    int currentCancelID = 0;


    protected override void Start()
    {
        base.Start();
        cokeDummyBIG.SetActive(false);
        AmmoCount = cokeDummyAmmo.Count;
    }

    private void Update()
    {
        if(!bossActivated && canSeeEnemy)
            bossActivated = true;
        if (reloading)
        {
            reloadingTimer += Time.deltaTime;
            MeleeAttackCheck(); 
        }
        if(charging)
        {
            Charging();
        }
        if (bossActivated && !reloading && !charging)
        {
            if (canSeeEnemy && ready)
            {
                int attackID = UnityEngine.Random.Range(1, 6);
                switch (attackID)
                {
                    case 1: case 2:
                        if (AmmoCount >= Shoot2CokeRotation.Count)
                        {
                            Shoot2();
                        }
                        else if (AmmoCount >= 1)
                            Shoot();
                        else if(!reloadCancelled)
                            Reload();
                        else 
                            Charge();
                        break;
                    case 3:
                        if (Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 0.5f), Vector2.right * GetDirection(), visionRange, 1 << LayerMask.NameToLayer("Player")))
                            Charge(); 
                        break;
                    default:
                        if (AmmoCount > 0)
                        {
                            Shoot();
                        }
                        else if (!reloadCancelled)
                            Reload();
                        else
                            Charge();
                        break;
                }
            }
            if (!canSeeEnemy && ready)
            {
                if (AmmoCount < cokeDummyAmmo.Count)
                {
                    Reload();
                }
                else if (chargeReady)
                {
                    Charge();
                }
            }
        }
    }
    //ChargeDMG
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player") && charging)
        {
            foreach (var c1 in collision.gameObject.GetComponent<PlayerController>().Colliders)
                foreach (var c2 in Colliders)
                {
                    Physics2D.IgnoreCollision(c1, c2);
                    StartCoroutine(IngoreCollisionDelay(c1, c2));
                }
            collision.collider.GetComponent<PlayerController>().stunned = true;
            collision.collider.GetComponent<Health>().TakenDamage(knockbackDamage);
            collision.collider.GetComponent<Rigidbody2D>().velocity = new Vector2(0, knockbackHight);
        }
    }
    IEnumerator IngoreCollisionDelay(Collider2D c1, Collider2D c2)
    {
        yield return new WaitForSeconds(0.1f);
        Physics2D.IgnoreCollision(c1, c2, false);
    }
    protected override void Shoot()
    {
        ready = false;
        base.Shoot();
        AmmoCount--;
        cokeDummyAmmo[AmmoCount].SetActive(false);
        StartCoroutine(AttackCD(shootCD));
    }

    //shot with 3 fast cokes
    protected void Shoot2()
    {
        ready = false;
        for(int i = 0 ; i < Shoot2CokeRotation.Count; i++)
        {
            cokeDummyAmmo[AmmoCount - 1 - i].SetActive(false);
        }
        AmmoCount -= Shoot2CokeRotation.Count;
        for (int i = 0; i < Shoot2CokeRotation.Count; i++)
        {
            var coke = Instantiate(cokeDummyBullet, rangeWeapon.transform.position, Quaternion.Euler(0, 0, Shoot2CokeRotation[i]));
            foreach(var col in Colliders)
                Physics2D.IgnoreCollision(coke.GetComponent<Collider2D>(), col);
            coke.GetComponent<Rigidbody2D>().velocity = new Vector2(GetDirection() * fastCokeSpeed * Mathf.Sin(Shoot2CokeRotation[i] / 180 * Mathf.PI),
                fastCokeSpeed * Mathf.Cos(Shoot2CokeRotation[i] / 180 * Mathf.PI));
        }
        StartCoroutine(AttackCD(shootCD));
    }
    private void MeleeAttackCheck()
    {
        if (reloadingTimer < 0.5 * reloadTime && meleeAttackReady && Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 0.5f), Vector2.right * GetDirection(), meleeRange, 1 << LayerMask.NameToLayer("Player")))
        {
            currentCancelID++;
            meleeAttackReady = false;
            reloading = false;
            ready = false;
            StartCoroutine(AttackCD((float)(0.4 * reloadTime)));
            reloadCancelled = true;
            MeleeAttack();
        }
    }
    private void Reload()
    {
        reloading = true;
        cokeDummyBIG.SetActive(true);
        meleeAttackReady = true;
        reloadingTimer = 0;
        StartCoroutine(AfterReload(reloadTime));
    }
    IEnumerator AfterReload(float cd)
    {
        int cancelID = currentCancelID + 1;
        yield return new WaitForSeconds(cd);
        if (!reloadCancelled)
        {
            cokeDummyBIG.SetActive(false);
            foreach (var item in cokeDummyAmmo)
                item.SetActive(true);
            AmmoCount = cokeDummyAmmo.Count;
            reloading = false;
        }
        else if(cancelID == currentCancelID)
        {
            reloadCancelled = false;
        }
    }

    void Charge()
    {
        chargeReady = false;
        charging = true;
    }
    void Charging()
    {
        if (!ObstacleCheck())
            EnemyMove();
        else
        {
            charging = false;
            StartCoroutine(ChargeCD(chargeCD));
        }
    }
    IEnumerator ChargeCD(float cd)
    {
        yield return new WaitForSeconds(cd);
        chargeReady = true;
    }

    IEnumerator AttackCD(float cd)
    {
        yield return new WaitForSeconds(cd);
        ready = true;
    }
}
