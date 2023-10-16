using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.IO;

public class Menu : MonoBehaviour
{
    [Header("UI Pages")]
    [SerializeField] GameObject menu;
    [SerializeField] GameObject options;
    [SerializeField] GameObject controls;
    [SerializeField] GameObject savesPanel;
    [SerializeField] GameObject newGameText;
    [Header("Options")]
    [SerializeField] GameObject kretImage;
    [SerializeField] Sprite kretYellow;
    [SerializeField] Sprite kretGrey;
    [SerializeField] Sprite kretBlack;
    [SerializeField] GameObject kret;
    [Header("SavesPanel")]
    [SerializeField] TextMeshProUGUI saveText1;
    [SerializeField] TextMeshProUGUI saveText2;
    [SerializeField] TextMeshProUGUI saveText3;
    bool newGame = false;

    GameObject currentMenu;
    private void Start()
    {
        newGame = false;
        SaveLoad.LoadControls();
        //VolumeSettings.SetVolume();
    }
    public void NewGame()
    {
        newGame = true;
        Continue();
    }
    public void Continue()
    {
        if (newGame)
        {
            newGameText.SetActive(true);
        }
        else
        {
            newGameText.SetActive(false);
        }
        kret.GetComponent<SpriteRenderer>().sprite = kretImage.GetComponent<Image>().sprite;
        savesPanel.SetActive(true);
        currentMenu = savesPanel;
        CheckSaves();
        if(PlayerPrefs.GetString("Save1") != "")
            saveText1.text = PlayerPrefs.GetString("Save1");
        if (PlayerPrefs.GetString("Save2") != "")
            saveText2.text = PlayerPrefs.GetString("Save2");
        if (PlayerPrefs.GetString("Save3") != "")
            saveText3.text = PlayerPrefs.GetString("Save3");
    }

    public void Exit()
    {
        Application.Quit();
    }
    public void Options()
    {
        currentMenu = options;
        menu.SetActive(false);
        options.SetActive(true);
    }
    public void Controls()
    {
        currentMenu = controls;
       menu.SetActive(false);
        controls.SetActive(true);
    }
    public void Escape(InputAction.CallbackContext contex)
    {
        if(contex.started)
            BackToMenu();
    }
    public void BackToMenu()
    {
        menu.SetActive(true);
        currentMenu.SetActive(false);
        if (newGame)
        {
            newGame = false;
        }
    }
    public void Easy()
    {
        kretImage.GetComponent<Image>().sprite = kretYellow;
    }
    public void Medium()
    {
        kretImage.GetComponent<Image>().sprite = kretGrey;
    }
    public void Hard()
    {
        kretImage.GetComponent<Image>().sprite = kretBlack;
    }
    public void Save1() 
    {
        PlayerPrefs.SetString("CurrentSavePath", "/Saves/Save1");
        DeleteSave();
        PlayerPrefs.SetString("CurrentSave", "Save1");
        CreateDir();
        if(PlayerPrefs.GetInt("Save1LVL")!=0)
            SceneManager.LoadScene(PlayerPrefs.GetInt("Save1LVL"));
        else
            SceneManager.LoadScene(1);
    }
    public void Save2()
    {
        PlayerPrefs.SetString("CurrentSavePath", "/Saves/Save2");
        DeleteSave();
        PlayerPrefs.SetString("CurrentSave", "Save2");
        CreateDir();
        if (PlayerPrefs.GetInt("Save1LVL") != 0)
            SceneManager.LoadScene(PlayerPrefs.GetInt("Save2LVL"));
        else
            SceneManager.LoadScene(1);
    }
    public void Save3() 
    {
        PlayerPrefs.SetString("CurrentSavePath", "/Saves/Save3");
        DeleteSave();
        PlayerPrefs.SetString("CurrentSave", "Save3");
        CreateDir();
        if (PlayerPrefs.GetInt("Save1LVL") != 0)
            SceneManager.LoadScene(PlayerPrefs.GetInt("Save3LVL"));
        else
            SceneManager.LoadScene(1);
    }
    void DeleteSave()
    {
        if (newGame)
        {
            PlayerPrefs.SetInt(PlayerPrefs.GetString("CurrentSave") + "LVL", 1);
            if(Directory.Exists(Application.dataPath + PlayerPrefs.GetString("CurrentSavePath")))
                DeleteDirectory(Application.dataPath + PlayerPrefs.GetString("CurrentSavePath"));
        }
    }
    public static void DeleteDirectory(string target_dir)
    {
        string[] files = Directory.GetFiles(target_dir);
        string[] dirs = Directory.GetDirectories(target_dir);

        foreach (string file in files)
        {
            File.SetAttributes(file, FileAttributes.Normal);
            File.Delete(file);
        }

        foreach (string dir in dirs)
        {
            DeleteDirectory(dir);
        }

        Directory.Delete(target_dir, false);
    }
    void CreateDir()
    {
        if (!Directory.Exists(Application.dataPath + PlayerPrefs.GetString("CurrentSavePath"))) {
            if (!Directory.Exists(Application.dataPath + "/Saves")){
                Directory.CreateDirectory(Application.dataPath + "/Saves"); 
            }
            Directory.CreateDirectory(Application.dataPath + PlayerPrefs.GetString("CurrentSavePath"));
        }
    }
    void CheckSaves()
    {
        if (!Directory.Exists(Application.dataPath + "/Saves/Save1"))
        {
            PlayerPrefs.SetString("Save1", "");
        }
        if (!Directory.Exists(Application.dataPath + "/Saves/Save2"))
        {
            PlayerPrefs.SetString("Save2", "");
        }
        if (!Directory.Exists(Application.dataPath + "/Saves/Save3"))
        {
            PlayerPrefs.SetString("Save3", "");
        }
    }
}
