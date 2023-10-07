using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pedestal : MonoBehaviour
{
    PlayerController player;
    [SerializeField] List<GameObject> cokePedestals = new List<GameObject>();
    [SerializeField] GameObject Gate;
    [SerializeField] float openingGateTime= 2f;
    int setCokeNumber = 0;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = collision.GetComponent<PlayerController>();
            player.Interact += SetItemOnPedestal;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player.Interact -= SetItemOnPedestal;
        }
    }
    void SetItemOnPedestal()
    {
        if (player.itemEQ[(int)GameManager.ItemID.CokeKey] > setCokeNumber)
        {
            cokePedestals[setCokeNumber].SetActive(true);
            setCokeNumber++;
        }
        if(setCokeNumber == cokePedestals.Count)
        {
            StartCoroutine(OpenGate());
        }
    }
    IEnumerator OpenGate()
    {
        float height = Gate.GetComponent<SpriteRenderer>().bounds.size.y * Gate.transform.localScale.y;
        float ticTime = 0.01f;
        float Time = 0f;
        while (Time <= openingGateTime)
        {
            Gate.transform.position = new Vector2(Gate.transform.position.x, Gate.transform.position.y + height * ticTime / openingGateTime);
            yield return new WaitForSeconds(ticTime);
            Time += ticTime;
        }
    }
}
