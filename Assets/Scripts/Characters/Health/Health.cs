using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class Health : MonoBehaviour
{
    [SerializeField] bool unkillable = false;
    [SerializeField]float health;
    public float maxHealth = 1;
    [SerializeField] List<GameObject> drop;
    [SerializeField] List<int> dropAmount;
    public event Action GetDamage;
    public event Action OnDeath;
    void Awake()
    {
        health = maxHealth;
    }
    void Update()
    {
        if(!unkillable && health <= 0)
        {
            if (CompareTag("Enemy"))
            {
                Drop();
                if(GetComponent<Character>().DisappearAfterDeath)
                    SaveLoad.ObjectDisappear(GetComponent<IDsc>().ID, ref SaveLoad.EnemiesDataList );
            }
            if (CompareTag("Player"))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            OnDeath?.Invoke();
            Destroy(this.gameObject);
        }
    }
    public void TakenDamage(float damage)
    {
        health -= damage;
        GetDamage?.Invoke();
    }
    public bool ifMaxHealth()
    {
        return maxHealth > health ;
    }
    public float GetHealth()
    {
        return health;
    }
    public void HealToFull()
    {
        health = maxHealth;
    }
    void Drop()
    {
        int i = 0;
        foreach (GameObject d in drop)
        {
            for (int j = 0; j < dropAmount[i]; j++)
            {
                System.Random r = new System.Random();
                Vector3 randomX = new Vector3(r.Next(-100,100)/100, 0,0);
                var drop = Instantiate(d, this.gameObject.transform.position + randomX, new Quaternion(0, 0, 0, 0));
                drop.GetComponent<PickUpItem>().enemyDrop = true;
            }
            i++;
        }
    }
}
