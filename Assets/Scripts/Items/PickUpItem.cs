using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : IDsc
{
    [SerializeField]GameManager.ItemID ItemID;
    [SerializeField] Collider2D BoxCollider;
    public int Amount = 1;
    bool ready = true;
    public bool enemyDrop = false;

    private void Start()
    {
        StartCoroutine(IgnoreCol());
    }
    IEnumerator IgnoreCol()
    {
        yield return new WaitForSeconds(0.1f);
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] items = GameObject.FindGameObjectsWithTag("PickUp");
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        foreach (var enemy in enemies) 
            foreach(var col in enemy.GetComponent<Enemy>().Colliders)
                Physics2D.IgnoreCollision(BoxCollider, col);
        foreach (var col in player.GetComponent<PlayerController>().Colliders)
            Physics2D.IgnoreCollision(BoxCollider, col);
        foreach (var item in items)
            Physics2D.IgnoreCollision(BoxCollider, item.GetComponent<BoxCollider2D>());
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collidor = collision.gameObject;
        if (collidor.CompareTag("Player") && ready)
        {
            ready = false;
            if(ItemID == GameManager.ItemID.Key)
            {
                GameObject.FindGameObjectWithTag("NextLVL").GetComponent<NextLVLtp>().GotKey();
                Destroy(this.gameObject);
                return;
            }
            collision.GetComponent<PlayerController>().itemEQ[(int)ItemID] += Amount;
            if(!enemyDrop)
                SaveLoad.ObjectDisappear(ID, ref SaveLoad.ItemDataList);
            Destroy(this.gameObject);
        }
    }
}

