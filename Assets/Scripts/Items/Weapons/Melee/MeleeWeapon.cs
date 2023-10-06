using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MeleeWeapon : Weapon
{
    public float swordDamage = 1f;
    public GameObject Rotator;
    abstract public void StartAttack();
    public void EndAttack()
    {
        gameObject.SetActive(false);
    }
    public void AttackCD()
    {
        ReadyToAttack = true;
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.GetComponent<Health>() && !collision.isTrigger)
        {
            collision.GetComponent<Health>().TakenDamage(swordDamage);
        }
    }
}
