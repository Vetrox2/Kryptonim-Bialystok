using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLVLtp : MonoBehaviour
{
    [SerializeField] bool resetEQ = false;
    bool haveKey = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().Interact += NextLVL;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().Interact -= NextLVL;
        }
    }
    void NextLVL()
    {
        if (haveKey)
        {
            if (resetEQ)
            {
                for (int i = 0; i < GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().itemEQ.Length; i++)
                    GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().itemEQ[i] = 0;
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().rangeWeapon = null;
            }
            SaveLoad.newLVL = true;
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().Save();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
    public void GotKey()
    {
        haveKey = true;
        GetComponent<Animator>().SetBool("TP_Activated", true);
    }
}
