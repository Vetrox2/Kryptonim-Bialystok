using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class PlayerData
{
    static public void SavePlayerData(GameObject Player, ref int SpawnID)
    {
        PlayerDataInfo playerData = new();
        playerData.itemEQ = Player.GetComponent<PlayerController>().itemEQ;
        if (Player.GetComponent<PlayerController>().rangeWeapon != null)
            playerData.rangeWeaponID = Player.GetComponent<PlayerController>().rangeWeapon.GetComponent<RangeWeapon>().Id;
        else
            playerData.rangeWeaponID = -1;
        playerData.health = Player.GetComponent<Health>().GetHealth();
        playerData.spawnID = SpawnID;
        string playerDataString = JsonUtility.ToJson(playerData);
        playerDataString = SaveLoad.EncryptFile(playerDataString);
        File.WriteAllText(Application.dataPath + PlayerPrefs.GetString("CurrentSavePath") + "/playerData.sav", playerDataString);
        PlayerPrefs.SetString(PlayerPrefs.GetString("CurrentSave"), System.DateTime.Now.ToString());
    }

    static public void LoadPlayerData(ref int SpawnID, ref GameObject Spawn, ref GameObject PlayerPrefab, ref List<GameObject> WeaponList)
    {
        if (File.Exists(Application.dataPath + PlayerPrefs.GetString("CurrentSavePath") + "/playerData.sav"))
        {
            string playerDataString = File.ReadAllText(Application.dataPath + PlayerPrefs.GetString("CurrentSavePath") + "/playerData.sav");
            playerDataString = SaveLoad.EncryptFile(playerDataString);
            PlayerDataInfo playerData = JsonUtility.FromJson<PlayerDataInfo>(playerDataString);
            SpawnID = playerData.spawnID;
            foreach (var spawn in GameObject.FindGameObjectsWithTag("SpawnPoint"))
            {
                if (spawn.GetComponent<SpawnPoint>().ID == SpawnID)
                    Spawn = spawn;
            }
            GameObject Player = UnityEngine.Object.Instantiate(PlayerPrefab, Spawn.transform.position, new Quaternion());

            Player.GetComponent<PlayerController>().itemEQ = playerData.itemEQ;
            Player.GetComponent<Health>().TakenDamage(Player.GetComponent<Health>().maxHealth - playerData.health);
            if (playerData.rangeWeaponID != -1)
                PickUpWeapon.SetNewRangeWeapon(Player.GetComponent<Collider2D>(), WeaponList[playerData.rangeWeaponID]);
        }
        else
        {
            SpawnID = 0;
            foreach (var spawn in GameObject.FindGameObjectsWithTag("SpawnPoint"))
            {
                if (spawn.GetComponent<SpawnPoint>().ID == SpawnID)
                    Spawn = spawn;
            }
            GameObject.Instantiate(PlayerPrefab, Spawn.transform.position, new Quaternion());
        }
    }
    class PlayerDataInfo
    {
        public int[] itemEQ;
        public int rangeWeaponID;
        public float health;
        public int spawnID = 0;
    }
}
