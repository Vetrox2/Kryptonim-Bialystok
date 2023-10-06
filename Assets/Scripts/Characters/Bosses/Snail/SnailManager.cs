using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnailManager : MonoBehaviour
{
    [SerializeField] GameObject InvisibleWall;
    [SerializeField] GameObject InvisibleFloor;
    [SerializeField] GameObject Teleport;
    [SerializeField] GameObject HealthBar;
    GameObject Snail = null;

    private void Start()
    {
        StartCoroutine(BossAliveCheck());
    }
    IEnumerator BossAliveCheck()
    {
        yield return new WaitForSeconds(1f);
        Snail = GameObject.Find("SnailBoss(Clone)");
        if (Snail == null)
        {
            SnailDeath();
        }
        else
        {
            Snail.GetComponent<Health>().OnDeath += SnailDeath;
        }
    }
    void SnailDeath()
    {
        InvisibleFloor.SetActive(true);
        InvisibleWall.SetActive(false);
        Teleport.SetActive(true);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && Snail != null)
        {
            Snail.GetComponent<Snail>().Activated = true;
            StartCoroutine(ActivateHealthBar());
        }
    }
    IEnumerator ActivateHealthBar()
    {
        yield return new WaitForSeconds(1f);
        HealthBar.SetActive(true);
        HealthBar.GetComponent<BossHealthBar>().Target = Snail;
    }
}
