using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    public float BulletSpeed = 5;
    public float BulletDamage = 1;
    public float rotation = 0;
    public float WeaponDamageMultiplier = 1;
    [HideInInspector]public Rigidbody2D rb;

    virtual protected void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
    }
    virtual protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Bullet>() || collision.gameObject.layer.Equals(LayerMask.NameToLayer("IgnoreBullet")))
        {
            Physics2D.IgnoreCollision(collision.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
        else
        {
            if (collision.GetComponent<Health>() && !collision.isTrigger)
            {
                collision.GetComponent<Health>().TakenDamage(BulletDamage * WeaponDamageMultiplier);
            }
            Destroy(this.gameObject);
        }
    }
    public void IgnoreCollisionWithEnemies()
    {
        foreach (var Enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            foreach (var enemyCollider in Enemy.GetComponent<Character>().Colliders)
                Physics2D.IgnoreCollision(GetComponent<Collider2D>(), enemyCollider, true);
        }
    }



}
