using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpWeapon : IDsc
{
    public GameObject Weapon;
    bool ReadyToInteract = true;
    [SerializeField] Collider2D BoxCollider;
    private void Start()
    {
        StartCoroutine(IgnoreCol());
    }
    IEnumerator IgnoreCol()
    {
        yield return new WaitForSeconds(0.01f);
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] items = GameObject.FindGameObjectsWithTag("PickUp");
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        foreach (var enemy in enemies)
            foreach (var col in enemy.GetComponent<Enemy>().Colliders)
                Physics2D.IgnoreCollision(BoxCollider, col);
        foreach (var col in player.GetComponent<PlayerController>().Colliders)
            Physics2D.IgnoreCollision(BoxCollider, col);
        foreach (var item in items)
            Physics2D.IgnoreCollision(BoxCollider, item.GetComponent<BoxCollider2D>());
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && collision.GetComponent<PlayerController>().pickUp && ReadyToInteract)
        {
            collision.GetComponent<PlayerController>().pickUp = false;
            ReadyToInteract = false;
            if (collision.GetComponent<PlayerController>().rangeWeapon != null)
                    Instantiate(collision.GetComponent<PlayerController>().rangeWeapon.GetComponent<RangeWeapon>().PickUpVersion, collision.transform.position, new Quaternion(0, 0, 0, 0));
            SetNewRangeWeapon(collision, Weapon);
            SaveLoad.ObjectDisappear(ID, ref SaveLoad.ItemDataList);
            Destroy(this.gameObject);
        }
    }
    static public void SetNewRangeWeapon(Collider2D collision, GameObject Weapon)
    {
        GameObject newWeapon = Instantiate(Weapon, collision.transform);
        newWeapon.transform.localScale = Weapon.transform.localScale;
        newWeapon.GetComponent<RangeWeapon>().Owner = collision.gameObject;
        Destroy(collision.GetComponent<PlayerController>().rangeWeapon);
        collision.GetComponent<PlayerController>().rangeWeapon = newWeapon;
    }
}
