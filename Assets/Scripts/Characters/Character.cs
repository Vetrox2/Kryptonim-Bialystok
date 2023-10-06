using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
[RequireComponent(typeof(Health))]
public abstract class Character : IDsc
{
    public bool DisappearAfterDeath = false;
    [Header("EQ")]
    public GameObject rangeWeapon;
    public GameObject meleeWeapon;
    public List <Collider2D> Colliders;
    [Header("Movement")]
    public float movementSpeed = 1;
    public bool isFacingRight = true;
    protected Vector2 velocityToAdd = Vector2.zero;
    protected bool ToFastStarted = false;

    [HideInInspector]
    protected Rigidbody2D rb;
    

    virtual protected void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void LateUpdate()
    {
        if (velocityToAdd.x != 0)
            rb.velocity += velocityToAdd;
        if (velocityToAdd.x != 0 && Mathf.Abs(rb.velocity.x) > Mathf.Abs(velocityToAdd.x) + movementSpeed)
        {
            if (rb.velocity.x > 0)
                rb.velocity = new Vector2(velocityToAdd.x + movementSpeed, rb.velocity.y);
            else
                rb.velocity = new Vector2(velocityToAdd.x - movementSpeed, rb.velocity.y);
        }
    }
    protected void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
    protected void Flip(Transform transf)
    {
        isFacingRight = !isFacingRight;
        Vector3 theScale = transf.localScale;
        theScale.x *= -1;
        transf.localScale = theScale;
    }
    public void AddVelocity(Vector2 velocity)
    {
        velocityToAdd = velocity;
    }
    public void ChangeToInterpolate()
    {
        if(rb.interpolation == RigidbodyInterpolation2D.Extrapolate)
            rb.interpolation = RigidbodyInterpolation2D.Interpolate;
    }
    public void ChangeToExtrapolate()
    {
        if (rb.interpolation == RigidbodyInterpolation2D.Interpolate)
            rb.interpolation = RigidbodyInterpolation2D.Extrapolate;
    }

}
