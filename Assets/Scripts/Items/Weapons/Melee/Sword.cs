using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MeleeWeapon
{
    public override void StartAttack()
    {
        if (ReadyToAttack)
        {
            ReadyToAttack = false;
            IgnoreCollisionWithOwner(this.gameObject);
            gameObject.SetActive(true);
            Rotator.GetComponent<Animator>().Play("SwordAttack_Anim");
            PlaySoundEffect();
            Invoke("EndAttack", animCooldown); 
            Invoke("AttackCD", attackCooldown);
        }
    }

    

    
}