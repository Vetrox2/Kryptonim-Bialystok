using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnPoint : MonoBehaviour
{
    public int ID = 0;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().Interact += ZakopanieWKopiec;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().Interact -= ZakopanieWKopiec;
        }
    }
    private void ZakopanieWKopiec()
    {
        GameManager.FindPlayer().GetComponent<Health>().HealToFull();
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().SpawnID = ID;
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().Save();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
}
