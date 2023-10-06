using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceGolem : Enemy
{
    [Header("Golem Settings")]
    [SerializeField] float targetingDistance = 5f;
    [SerializeField] float movementBoost = 1.5f;
    [SerializeField] float jump = 5f;
    [SerializeField] float explosionDamage = 1f;
    [SerializeField] float walkBreakTime = 1f;
    [SerializeField] AudioSource ads;
    [SerializeField] Collider2D explosionCollider;

    ContactFilter2D explosionFilter = new ContactFilter2D();
    Animator Animator;
    bool limitedDistanceAchieved = false;
    bool sawEnemy = false;
    bool ready = true;
    bool lostEnemy = false;
    bool jumped = false;
    Transform targetTransform = null;

    override protected void Start()
    {
        base.Start();
        StartCoroutine(RunningAnimCheck());
        explosionFilter.layerMask = LayerMask.NameToLayer("Player");
        Animator = GetComponent<Animator>();
        explosionCollider.enabled = false;
    }
    private void Update()
    {
        if (canSeeEnemy)
        {
            sawEnemy = true;
            targetTransform = Target.transform;
        }
        if (ready && targetTransform != null && Vector2.Distance(EyesPos.position, targetTransform.position) < 1.5)
        {
            StartCoroutine(Attack());
        }
        else if (sawEnemy && ready && !jumped)
        {
            EnemyMove(movementBoost);
        }
        if (sawEnemy && !canSeeEnemy)
        {
            if (targetingDistance < Math.Abs(EyesPos.position.x - targetTransform.position.x))
                sawEnemy = false;
            else
            {
                if (targetTransform.position.x > EyesPos.position.x && GetDirection() < 0)
                    Flip();
                if (targetTransform.position.x < EyesPos.position.x && GetDirection() > 0)
                    Flip();
                if (!jumped && ObstacleCheck())
                {
                    EnemyMove(movementBoost/1.8f, jump);
                    if(!jumped)
                        Invoke("CheckAfterJump", 2f);
                    jumped = true;
                }  
            }
        }
    }
    void CheckAfterJump()
    {
        jumped = false;
    }
    IEnumerator Attack()
    {
        ready = false;
        rb.velocity = new Vector2(rb.velocity.x*1.5f, 3.5f);
        ads.Play();
        yield return new WaitForSeconds(0.2f);
        explosionCollider.enabled = true;
        List<Collider2D> results = new();
        explosionCollider.OverlapCollider(explosionFilter, results);
        foreach (Collider2D c in results)
        {
            if (c.CompareTag("Player"))
            {
                c.gameObject.GetComponent<Health>().TakenDamage(explosionDamage);
            }
        }
        GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(1.2f);
        Destroy(this.gameObject);
    }
    IEnumerator RunningAnimCheck()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);
            if (Math.Abs(rb.velocity.x) > 0.5)
                Animator.SetBool("Run", true);
            else
                Animator.SetBool("Run", false);
            Animator.SetFloat("RunM", rb.velocity.x / movementSpeed);
        }
    }
    override protected bool ObstacleCheck()
    {
        if (Physics2D.Raycast(transform.position, Vector2.right * GetDirection(), obstacleCheckDistance, obstacleMask))
        {
            return true;
        }
        return false;
    }

}
