using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

static public class SaveLoad
{
    static public SpawnInfo[] EnemiesDataList;
    static public SpawnInfo[] ItemDataList;
    static public bool newLVL = false;
    static string key = "4163731634";
    public class SpawnInfo
    {
        public GameObject Object;
        public Transform SpawnTransf;
        public bool Disappeared;
    }
    class DisappearedObjectsInfo
    {
        public bool[] DisappearedObjects;
    }
    static public void LoadObjects(string SpawnTagName, ref SpawnInfo[] List, string FileName)
    {
        FindObjectsToSpawn(SpawnTagName, ref List);
        if (!newLVL)
            LoadObjectData(ref List, FileName);
        SpawnObjects(ref List);
    }
    static public void FindObjectsToSpawn(string TagName, ref SpawnInfo[] List )
    {
        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag(TagName);
        List = new SpawnInfo[spawnPoints.Length];
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            SpawnInfo ObjInfo = new();
            ObjInfo.SpawnTransf = spawnPoints[i].GetComponent<Transform>();
            ObjInfo.Object = spawnPoints[i].GetComponent<ObjectSpawnPoint>().ObjectToSpawn;
            ObjInfo.Disappeared = false;
            List[i] = ObjInfo;
        }
    }
    static public void SpawnObjects(ref SpawnInfo[] List)
    {
        int i = 0;
        foreach (SpawnInfo obj in List)
        {
            if (!obj.Disappeared)
            {
                var ob = Object.Instantiate(obj.Object, obj.SpawnTransf.position, Quaternion.Euler(0, 0, 0));
                ob.GetComponent<IDsc>().ID = i;
            }
            ++i;
        }
    }
    static public void ObjectDisappear(int ID, ref SpawnInfo[] List)
    {
        List[ID].Disappeared = true;
    }
    static public void SaveObjectData(ref SpawnInfo[] List, string FileName)
    {
        DisappearedObjectsInfo objData = new();
        objData.DisappearedObjects = new bool[List.Length];
        int i = 0;
        foreach (var objInfo in List)
        {
            objData.DisappearedObjects[i] = objInfo.Disappeared;
            i++;
        }
        string objDataS = JsonUtility.ToJson(objData);
        objDataS = EncryptFile(objDataS);
        File.WriteAllText(Application.dataPath + PlayerPrefs.GetString("CurrentSavePath") + FileName, objDataS);
        PlayerPrefs.SetString(PlayerPrefs.GetString("CurrentSave"), System.DateTime.Now.ToString());
    }
    static public void LoadObjectData(ref SpawnInfo[] List, string FileName)
    {
        if (File.Exists(Application.dataPath + PlayerPrefs.GetString("CurrentSavePath") + FileName))
        {
            DisappearedObjectsInfo objData;
            string objDataS = File.ReadAllText(Application.dataPath + PlayerPrefs.GetString("CurrentSavePath") + FileName);
            objDataS = EncryptFile(objDataS);
            objData = JsonUtility.FromJson<DisappearedObjectsInfo>(objDataS);
            int i = 0;
            foreach (var objInfo in List)
            {
                objInfo.Disappeared = objData.DisappearedObjects[i];
                i++;
            }
        }
    }
    static public void SaveControls(PlayerInput input)
    {
        string controls = input.actions.SaveBindingOverridesAsJson();
        Debug.Log(controls);
        PlayerPrefs.SetString("Controls", controls );
    }
    static public void LoadControls()
    {
        string controls = PlayerPrefs.GetString("Controls", string.Empty);
        if(string.IsNullOrEmpty(controls)) { return; }
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PlayerInput>().actions.LoadBindingOverridesFromJson(controls);
    }
    static public string EncryptFile(string file)
    {
        string result = "";
        for(int i = 0; i < file.Length; i++) 
        {
            result += (char)(file[i] ^ key[i % key.Length]);
        }
        return result;
    }
}
