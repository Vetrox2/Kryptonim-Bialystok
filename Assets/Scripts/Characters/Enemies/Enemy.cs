using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Character
{
    [SerializeField] protected float movementBackSpeed = 1f;
    [SerializeField] protected float obstacleCheckDistance = 0;
    [SerializeField] protected bool movingDistanceLimited = false;
    [SerializeField] protected float movingDistance = 1f;
    float distanceTravelled = 0f;
    protected Vector2 movingStartPoint;
    protected Vector2 movingEndPoint;
    bool hasStartingPoint = false;

    [Header("FOV")]
    [SerializeField] protected float visionRange = 1;
    [SerializeField] float visionAngle;
    [SerializeField] protected Transform EyesPos;
    [SerializeField] protected LayerMask obstacleMask;
    protected bool canSeeEnemy = false;
    protected GameObject Target = null;
    bool enemyInAngle = false;
    
    virtual protected void Start()
    {
        StartCoroutine(FOVCheck());
        if (movingDistanceLimited)
        {
            movingStartPoint = transform.position;
            movingEndPoint = isFacingRight ? new Vector2(transform.position.x + movingDistance, transform.position.y) : new Vector2(transform.position.x - movingDistance, transform.position.y);
        }
    }
    virtual protected void EnemyMove(float movementBoost = 1, float jump = 0)
    {
        ObstacleCheck();
        rb.velocity = new Vector2(movementSpeed * GetDirection() * movementBoost, jump);
        if (movingDistanceLimited) CheckTravellLimit();
    }
    virtual protected void CheckTravellLimit()
    {
        if (Math.Round(transform.position.x, 1) == Math.Round(movingEndPoint.x, 1))
        {
            Flip();
            Vector2 p = movingStartPoint;
            movingStartPoint = movingEndPoint;
            movingEndPoint = p;
        }
    }
    virtual protected void MoveBack()
    {
        rb.velocity = new Vector2(movementBackSpeed * -GetDirection(), 0);
    }

    virtual protected void Shoot()
    {
        if (Target != null)
        {
            rangeWeapon.GetComponent<RangeWeapon>().shoot(Target, true);
        }
    }
    virtual protected void MeleeAttack()
    {
        meleeWeapon.GetComponent<MeleeWeapon>().StartAttack();
    }

    protected IEnumerator FOVCheck()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);
            FieldOfViewCheck();
        }
    }
    virtual protected void FieldOfViewCheck()
    {
        Collider2D[] TargetT = Physics2D.OverlapCircleAll(EyesPos.position, visionRange, 1 << LayerMask.NameToLayer("Player"));
        if (TargetT.Length != 0)
        {
            Transform Target = TargetT[0].transform;
            Vector2 directionToTarget = (Target.position - EyesPos.position);
            enemyInAngle = false;
            if (isFacingRight && directionToTarget.x > 0)
            {
                if (directionToTarget.y < 0)
                {
                    enemyInAngle = MathF.Atan(directionToTarget.y / directionToTarget.x) * 180 / MathF.PI > -visionAngle / 2;
                }
                else
                {
                    enemyInAngle = MathF.Atan(directionToTarget.y / directionToTarget.x) * 180 / MathF.PI < visionAngle / 2;
                }
            }
            else if(!isFacingRight && directionToTarget.x < 0)
            {
                if (directionToTarget.y > 0)
                {
                    enemyInAngle = MathF.Atan(directionToTarget.y / directionToTarget.x) * 180 / MathF.PI > -visionAngle / 2;
                }
                else
                {
                    enemyInAngle = MathF.Atan(directionToTarget.y / directionToTarget.x) * 180 / MathF.PI < visionAngle / 2;
                }
            }
            
            if (enemyInAngle && !Physics2D.Raycast(EyesPos.position, directionToTarget, directionToTarget.magnitude, obstacleMask))
            {
                canSeeEnemy = true;
                this.Target = Target.gameObject;
            }
            else
            {
                canSeeEnemy = false;
            }
        }
        else
        {
            canSeeEnemy = false;
            this.Target = null;
        }
    }
    virtual protected bool ObstacleCheck()
    {
        if (Physics2D.Raycast(transform.position, Vector2.right * GetDirection(), obstacleCheckDistance, obstacleMask))
        {
            Flip();
            return true;
        }
        return false;
    }
    protected float GetDirection()
    {
        return isFacingRight ? 1f : -1f;
    }
}
