using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Trader : MonoBehaviour
{
    [SerializeField] GameObject shopUI;
    public GameObject InteractionText;

    //PriceList
    [SerializeField] GameManager.ItemID ItemsIDList;
    [SerializeField] int[] TraderPriceList = new int[10] { 0, 2, 3, 0, 0, 0, 0, 0, 0, 0 };

    private void Start()
    {
        InteractionText.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            InteractionText.SetActive(true);
            collision.GetComponent<PlayerController>().Interact += ActivateShop;
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            InteractionText.SetActive(false);
            collision.GetComponent<PlayerController>().Interact -= ActivateShop;
            shopUI.SetActive(false);
        }
    }
    private void ActivateShop()
    {
        shopUI.GetComponent<ShopUIManager>().PriceList = TraderPriceList;
        shopUI.GetComponent<ShopUIManager>().UpdatePriceText();
        shopUI.SetActive(true);
        InteractionText.SetActive(false);
    }
}
