using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject PlayerPrefab;
    GameObject Spawn;
    public int SpawnID = 0;
    public List <GameObject> WeaponList;

    private void Start()
    {
        LoadGame();
    }
    
    public void LoadGame()
    {
        PlayerData.LoadPlayerData(ref SpawnID, ref Spawn, ref PlayerPrefab,ref WeaponList);
        SaveLoad.LoadObjects("EnemySpawn", ref SaveLoad.EnemiesDataList, "/enemiesData.sav");
        SaveLoad.LoadObjects("ItemSpawn", ref SaveLoad.ItemDataList, "/itemsData.sav");
        SaveLoad.newLVL = false;
    }
    
    public void Save()
    {
        GameObject Player = FindPlayer();
        if (SaveLoad.newLVL)
            SpawnID = 0;
        PlayerData.SavePlayerData(Player, ref SpawnID);
        PlayerPrefs.SetInt(PlayerPrefs.GetString("CurrentSave")+"LVL", SceneManager.GetActiveScene().buildIndex);
        SaveLoad.SaveObjectData(ref SaveLoad.EnemiesDataList, "/enemiesData.sav");
        SaveLoad.SaveObjectData(ref SaveLoad.ItemDataList, "/itemsData.sav");
    }
    static public GameObject FindPlayer()
    {
        GameObject player;
        if (player = GameObject.FindGameObjectWithTag("Player"))
            return player;
        return null;
    }
    public enum ItemID
    {
        Money,
        Syringe,
        Ammo9mm,
        Key,
        CokeKey
    }
    public enum WeaponID
    {

    }
    
}
