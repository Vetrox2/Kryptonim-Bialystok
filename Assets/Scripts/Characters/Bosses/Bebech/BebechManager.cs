using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BebechManager : MonoBehaviour
{
    [SerializeField] AudioSource ads;
    [SerializeField] GameObject teleport;
    [SerializeField] GameObject boss;
    [SerializeField] GameObject healthBar;
    [SerializeField] AudioClip nieClip;
    private void Start()
    {
        boss.GetComponent<Health>().OnDeath += ActivateTP;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ads.Play();
            healthBar.SetActive(true);
        }
    }
    void ActivateTP()
    {
        teleport.SetActive(true);
        ads.Stop();
        ads.clip = nieClip;
        ads.loop = false;
        ads.volume = 0.25f;
        ads.Play();
    }
}
