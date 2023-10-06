using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField] GameObject healthBar;
    [SerializeField] Image dashBar;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI syringeText;
    public TextMeshProUGUI ammoText;
    public GameObject Menu;
    [SerializeField] GameObject Controls;
    bool MenuActivity = false;
    GameObject player;
    PlayerController playerController;
    float dashCooldawn = 0;
    bool dashCD = false;
    void Start()
    {
        player = GameManager.FindPlayer();
        if(player != null)
            healthBar.GetComponent<HealthBar>().Target = player;
        Menu.SetActive(false);
    }

   
    void Update()
    {
        if (player != null)
        {
            playerController = player.GetComponent<PlayerController>();
            moneyText.text = $"{playerController.itemEQ[(int)GameManager.ItemID.Money]}";
            syringeText.text = $"{playerController.itemEQ[(int)GameManager.ItemID.Syringe]}";
            ammoText.text = $"{playerController.itemEQ[(int)GameManager.ItemID.Ammo9mm]}";
            dashBar.fillAmount = (playerController.dashCooldown - dashCooldawn) / playerController.dashCooldown;
            if (!playerController.canDash && !dashCD)
            {
                dashCD = true;
                dashCooldawn = playerController.dashCooldown;
            }
            else if(dashCD && dashCooldawn > 0)
            {
                dashCooldawn -= Time.deltaTime;
            }
            else if(dashCD)
            {
                dashCD = false;
                dashCooldawn = 0;
            }
        }
        else
        {
            player = GameManager.FindPlayer();
            if (player != null)
                healthBar.GetComponent<HealthBar>().Target = player;
        }
    }

    public void Resume()
    {
        TurnMenu();
    }
    public void Exit()
    {
        SceneManager.LoadScene(0);
    }
    public void TurnMenu()
    {
        if (Controls.activeSelf)
        {
            Controls.SetActive(false);
        }
        else
        {
            MenuActivity = !MenuActivity;
            Menu.SetActive(MenuActivity);
        }
    }
    public void Options()
    {
        Controls.SetActive(true);
    }
    public void BackToMenu()
    {
        Controls.SetActive(false);
    }
}
