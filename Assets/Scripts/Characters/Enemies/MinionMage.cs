using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class MinionMage : Enemy
{
   
    private void Update()
    {
        if(canSeeEnemy)
        {
            if (rangeWeapon.GetComponent<RangeWeapon>().ReadyToAttack)
                Shoot();
            else
                MoveBack();
        }
        else
        {
            EnemyMove();
        }
    }
}
