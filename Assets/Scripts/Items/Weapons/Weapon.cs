using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public GameObject Owner;
    public GameObject PickUpVersion;
    public int Id = 0;
    public float attackCooldown = 1f;
    public float animCooldown = 0f;
    public bool ReadyToAttack = true;
    public void IgnoreCollisionWithOwner(GameObject weapon)
    {
        foreach (var collider in Owner.GetComponent<Character>().Colliders)
        {
            Physics2D.IgnoreCollision(collider, weapon.GetComponent<Collider2D>(), true);
        }
    }
    protected void PlaySoundEffect()
    {
        AudioSource audio;
        if (TryGetComponent<AudioSource>(out audio) && audio.clip != null)
        {
            audio.Play();
        }
        
    }

}
