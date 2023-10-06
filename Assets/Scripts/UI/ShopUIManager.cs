using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopUIManager : MonoBehaviour
{
    public TextMeshProUGUI[] PriceText = new TextMeshProUGUI[3];
    public int[] PriceList = new int[3] {0,1,1};
    GameObject player;
    void Start()
    {
        StartCoroutine(FindPlayer());
    }
    public void Buy()
    {
        GameObject ButtonRef = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject;
        int id = (int)ButtonRef.GetComponent<ShopButtonInfo>().ItemID;
        if (player != null && player.GetComponent<PlayerController>().itemEQ[(int)GameManager.ItemID.Money] >= PriceList[id])
        {
            player.GetComponent<PlayerController>().itemEQ[(int)GameManager.ItemID.Money] -= PriceList[id];
            player.GetComponent<PlayerController>().itemEQ[id] += 1;

        }
    }
    public void UpdatePriceText()
    {
        for(int i = 1; i < PriceText.Length ; i++)
        {
            PriceText[i].text = $"{PriceList[i]}";
        }
    }
    IEnumerator FindPlayer()
    {
        while(true)
        {
            yield return new WaitForSeconds(0.1f);
            player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                break;
        }
    }



}
