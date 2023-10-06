using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BebechManager : MonoBehaviour
{
    [SerializeField] AudioSource ads;
    [SerializeField] GameObject teleport;
    [SerializeField] GameObject boss;
    private void Start()
    {
        boss.GetComponent<Health>().OnDeath += ActivateTP;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ads.Play();
        }
    }
    void ActivateTP()
    {
        teleport.SetActive(true);
    }
}
