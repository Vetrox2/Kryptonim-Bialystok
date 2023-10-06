using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;

public class Crow : Enemy
{
    [Header("Graphics")]
    [SerializeField] GameObject GFX;
    [SerializeField] Animator Animator;
    [SerializeField] float SayCD = 2f;
    [Header("Attacking")]
    [SerializeField] float TargetingDistance = 10f;
    [SerializeField] float CrowDamage = 1f;
    [SerializeField] float attackCD = 1f;
    [SerializeField] float waitAfterAttackTime = 5f;
    [SerializeField] bool OneHitAttacker = false;
    [SerializeField] AudioClip flyClip;
    [SerializeField] AudioClip attackClip;
    [SerializeField] AudioClip sayClip;
    [SerializeField] AudioSource ads; 
    [SerializeField] AudioSource ads2;

    bool OneHitAttacked = false;
    bool headingBack = false;
    Transform treePoint;

    AIDestinationSetter AIDestinationSetter;
    bool sawEnemy = false;
    bool attacked = false;
    AIPath aipath;
    protected override void Awake()
    {
        base.Awake();
        AIDestinationSetter = GetComponent< AIDestinationSetter> ();
        aipath = GetComponent<AIPath>();
        aipath.maxSpeed = movementSpeed;
        StartCoroutine(SayAnim());
    }
    private void Update()
    {
        if (!sawEnemy && canSeeEnemy)
        {
            Animator.SetBool("isFlying", true);
            aipath.isStopped = false;
            sawEnemy = true;
            AIDestinationSetter.target = Target.transform;
            
        }
        else if(sawEnemy && AIDestinationSetter.target.CompareTag("Player") && TargetingDistance < Vector2.Distance(AIDestinationSetter.target.position, rb.position))
        {
            AIDestinationSetter.target = null;
            sawEnemy = false;
            aipath.isStopped = true;
            Invoke("AfterAttack", waitAfterAttackTime);
        }
        if (OneHitAttacker && OneHitAttacked)
        {
            AfterOneAttack();
        }
        if(aipath.desiredVelocity.x > 0 && !isFacingRight || aipath.desiredVelocity.x < 0 && isFacingRight)
        {
            Flip(GFX.transform);
        }
        if (Animator.GetBool("isFlying") && ads2.clip == null)
        {
            ads2.clip = flyClip;
            ads2.Play();
        }
        else if(!Animator.GetBool("isFlying"))
        {
            ads2.Stop();
            ads2.clip = null;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !attacked)
        {
            collision.gameObject.GetComponent<Health>().TakenDamage(CrowDamage);
            OneHitAttacked = true;
            ads.clip = attackClip;
            ads.Play();
            StartCoroutine(AttackCD());
        }
    }
    IEnumerator AttackCD()
    {
        attacked = true;
        yield return new WaitForSeconds(attackCD);
        attacked = false;
    }
    IEnumerator SayAnim()
    {
        while (true)
        {
            if (!Animator.GetBool("isFlying"))
            {
                Animator.SetBool("Say", true);
                ads.clip = sayClip;
                ads.Play();
                yield return new WaitForSeconds(1f);
                Animator.SetBool("Say", false);
            }
            yield return new WaitForSeconds(SayCD);
        }
    }
    void AfterAttack()
    {
        if (!sawEnemy && !headingBack)
        {
            headingBack = true;
            aipath.isStopped = false;
            SearchClosestTree();
            AIDestinationSetter.target = treePoint;
            Animator.SetBool("isFlying", true);
            StartCoroutine(BackAtTree());
        }
    }
    void AfterOneAttack()
    {
        if (!headingBack)
        {
            headingBack = true;
            aipath.isStopped = false;
            SearchClosestTree();
            AIDestinationSetter.target = treePoint;
            Animator.SetBool("isFlying", true);
            StartCoroutine(BackAtTree());
        }
    }
    void SearchClosestTree()
    {
        GameObject[] trees = GameObject.FindGameObjectsWithTag("Tree");
        float distance = 99999999;
        foreach (GameObject tree in trees) 
        {
            if(distance >= Vector2.Distance(tree.transform.position, transform.position)){
                distance = Vector2.Distance(tree.transform.position, transform.position);
                treePoint = tree.transform;
            }
        }
        
    }
    IEnumerator BackAtTree()
    {
        while (true)
        { 
            if(sawEnemy && (!OneHitAttacker || (headingBack && !OneHitAttacked))) { headingBack = false; break; }
            if (Math.Round(transform.position.x, 1) == Math.Round(treePoint.position.x, 1))
            {
                Animator.SetBool("isFlying", false);
                AIDestinationSetter.target = null;
                aipath.isStopped = true;
                headingBack = false;
                sawEnemy = false; 
                OneHitAttacked = false;
                break;
            }
            yield return new WaitForSeconds(0.2f);
        }
    }
}
