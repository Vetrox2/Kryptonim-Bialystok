using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class MinionWarrior : Enemy
{
    [SerializeField] float attackDistance = 1f;
    protected override void Awake()
    {
        base.Awake();
        GetComponent<Health>().GetDamage += GetDamageInTheBack;
    }
    private void Update()
    {
        EnemyMove();
        if(canSeeEnemy && Vector2.Distance(Target.transform.position, transform.position) <= attackDistance)
        {
            MeleeAttack();
        }
    }
    private void GetDamageInTheBack()
    {
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;
        if ((isFacingRight && player.position.x < transform.position.x) || (!isFacingRight && player.position.x > transform.position.x))
        {
            Flip();
            Vector2 p = movingStartPoint;
            movingStartPoint = movingEndPoint;
            movingEndPoint = p;
        }
    }

}
