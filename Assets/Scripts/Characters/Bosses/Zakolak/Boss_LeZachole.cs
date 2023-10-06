using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using Unity.Mathematics;
using UnityEngine;

public class Boss_LeZachole : MonoBehaviour
{
    
    [SerializeField] List<Collider2D> colliders;
    bool ReadyToAttack = true;
    float attackCD = 0;

    [SerializeField] GameObject laser;
    [SerializeField] GameObject laserRotator;
    [SerializeField] float laserAttackCD = 2f;

    [SerializeField] GameObject Rocket;
    [SerializeField] Transform RocketSpawn;
    [SerializeField] int rocketsCount = 10;
    [SerializeField] float rocketAttackCD = 3f;
    [SerializeField] float firstRocketAngle = 90f;
    [SerializeField] float lastRocketAngle = 130f;
    [SerializeField] float timeBetweenRockets = 0.07f;

    [SerializeField] GameObject JavelinRocket;
    [SerializeField] int javelinsCount = 3;
    [SerializeField] float javelinCDBeetwenRockets = 0.5f;
    [SerializeField] float javelinAttackCD = 2f;
    private void Update()
    {
        if (ReadyToAttack)
        {
            ReadyToAttack = false;
            int AttackNumber = UnityEngine.Random.Range(1, 4);
            switch (AttackNumber)
            {
                case 1:
                    {
                        laser.SetActive(true);
                        laserRotator.GetComponent<Animator>().Play("LaserAnim");
                        Invoke("LaserCD", 1f);
                        attackCD = laserAttackCD;
                        break;
                    }
                case 2:
                    {
                        for(int i = 0; i<javelinsCount;i++)
                            Invoke("JavelinAttack", javelinCDBeetwenRockets*i);
                        attackCD = javelinAttackCD + javelinCDBeetwenRockets * javelinsCount;
                        break;
                    }
                case 3:
                    {
                        StartCoroutine(RocketAttack());
                        attackCD = rocketAttackCD;
                        break;
                    }
            }
            
            Invoke("AttackCD", attackCD);
        }

    }
    void LaserCD()
    {
        laser.SetActive(false);
    }
    void AttackCD()
    {
        ReadyToAttack = true;
    }
    void IgnoreCollision(GameObject missile)
    {
        foreach(var collider in colliders)
        {
            Physics2D.IgnoreCollision(collider, missile.GetComponent<Collider2D>(), true);
        }
    }
    void JavelinAttack() 
    {
        var rocket = Instantiate(JavelinRocket, RocketSpawn.position, RocketSpawn.rotation);
        IgnoreCollision(rocket);
    }
    IEnumerator RocketAttack()
    {
        float rotation = (lastRocketAngle - firstRocketAngle) / rocketsCount;
        for (int i = 0; i < rocketsCount; i++)
        {
            var rocket = Instantiate(Rocket, RocketSpawn.position, Quaternion.Euler(0, 0, firstRocketAngle + rotation * i));
            IgnoreCollision(rocket);
            yield return new WaitForSeconds(timeBetweenRockets);
        }
    }
}
