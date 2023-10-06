using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Wolf : Enemy
{
    [Header("Wolf Settings")]
    [SerializeField] float targetingDistance = 5f;
    [SerializeField] float movementBoost = 1.5f;
    [SerializeField] float biteDamage = 1f;
    [SerializeField] float walkBreakTime = 1f;
    [SerializeField] Collider2D biteCollider;
    [SerializeField] AudioClip biteClip;
    [SerializeField] AudioSource ads;

    ContactFilter2D biteFilter = new ContactFilter2D();
    Animator Animator;
    bool limitedDistanceAchieved = false;
    bool sawEnemy = false;
    bool ready = true;
    bool lostEnemy = false;
    Transform targetTransform = null;

    override protected void Start()
    {
        base.Start();
        StartCoroutine(RunningAnimCheck());
        Animator = GetComponent<Animator>();
        biteFilter.layerMask = LayerMask.NameToLayer("Player");
    }
    private void Update()
    {
        if(lostEnemy)
        {
            sawEnemy = false;
            lostEnemy = false;
            movingDistanceLimited = true;
            movingStartPoint = transform.position;
            movingEndPoint = isFacingRight ? new Vector2(transform.position.x + movingDistance, transform.position.y) : new Vector2(transform.position.x - movingDistance, transform.position.y);
        }
        if (!limitedDistanceAchieved && !canSeeEnemy && !sawEnemy)
        {
            EnemyMove();
        }
        if (canSeeEnemy && !sawEnemy)
        {
            sawEnemy = true;
            targetTransform = Target.transform;
        }
        if (sawEnemy && canSeeEnemy)
        {
            movingDistanceLimited = false;
            if (ready && Math.Abs(EyesPos.position.x - Target.transform.position.x) < 1)
            {
                StartCoroutine(Attack());
            }
            else if (ready)
            {
                EnemyMove(movementBoost);
            }
        }
        if (sawEnemy && !canSeeEnemy)
        {
            if (targetingDistance < Math.Abs(EyesPos.position.x - targetTransform.position.x))
                lostEnemy = true;
            else
            {
                if (targetTransform.position.x > EyesPos.position.x && GetDirection() < 0)
                    Flip();
                if (targetTransform.position.x < EyesPos.position.x && GetDirection() > 0)
                    Flip();
            }
        }
    }
    IEnumerator Attack()
    {
        ready = false;
        StartCoroutine(AttackAnim());
        ads.clip = biteClip;
        ads.Play();
        List <Collider2D> results = new();
        yield return new WaitForSeconds(0.55f);
        biteCollider.enabled = true;
        biteCollider.OverlapCollider(biteFilter, results);
        foreach (Collider2D c in results)
        {
            if (c.CompareTag("Player"))
            {
                c.gameObject.GetComponent<Health>().TakenDamage(biteDamage);
            }
        }
        biteCollider.enabled = false;
        yield return new WaitForSeconds(0.2f);
        ready = true;
    }
    IEnumerator AttackAnim()
    {
        Animator.SetBool("Attack", true);
        yield return new WaitForSeconds(0.55f);
        Animator.SetBool("Attack", false);
    }
    IEnumerator RunningAnimCheck()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);
            if (Math.Abs(rb.velocity.x) > 1)
                Animator.SetBool("Moving", true);
            else
                Animator.SetBool("Moving", false);
            Animator.SetFloat("RunM", rb.velocity.x / movementSpeed);
        }
    }
    override protected void CheckTravellLimit()
    {
        if (Math.Round(transform.position.x, 1) == Math.Round(movingEndPoint.x, 1))
        {
            limitedDistanceAchieved = true;
            Vector2 p = movingStartPoint;
            movingStartPoint = movingEndPoint;
            movingEndPoint = p;
            Invoke("ContinueWalking", walkBreakTime);
        }
    }
    private void ContinueWalking()
    {
        Flip();
        limitedDistanceAchieved = false;
    }
}
