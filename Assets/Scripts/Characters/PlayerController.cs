using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerController : Character
{
    
    public float jumpSpeed = 1;
    public bool stunned = false;
    public bool pickUp = false;
    float move;
    InputAction.CallbackContext shooting;
    [SerializeField] Collider2D GroundCheck;

    //EQ
    public int[] itemEQ = new int[5] { 0, 0, 0, 0, 0 };

    //shitty variable needed for dash speed and duration
    public float dashSpeed = 2f;
    public float dashDuration = 0.1f;
    public float dashCooldown = 1.0f;
    float dashTime = 0f;
    [HideInInspector]public bool canDash = true;
    public event Action Interact;

    int i = 0;

    private void Start()
    {
        foreach (var Enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            foreach (var Collider in Colliders)
                foreach(var enemyCollider in Enemy.GetComponent<Character>().Colliders)
                    Physics2D.IgnoreCollision(Collider, enemyCollider, true);
        }
    }
    void Update()
    {
        //moving and facing
        move = Input.GetAxis("Horizontal");
        if (isFacingRight && move < 0f || !isFacingRight && move > 0f)
        {
            Flip();
        }
        // Dash implementation
        if (Time.time < dashTime && !stunned)
        {
            rb.velocity = new Vector2(move * dashSpeed, rb.velocity.y);
        }
        // Movement implementation
        else if (!stunned)
        {
            if (velocityToAdd == Vector2.zero && Mathf.Abs(rb.velocity.x) > movementSpeed && !ToFastStarted)
            {
                ToFastStarted = true;
                StartCoroutine(ToFast());
            }
            else if (!ToFastStarted)
            {
                rb.velocity = new Vector2(move * movementSpeed, rb.velocity.y);
            }
        }
        if (stunned)
        {
            Invoke("StunnedGroundCheck", 1f);
        }
        //rangeAttack
        if (rangeWeapon != null && shooting.performed)
        {
            rangeWeapon.GetComponent<RangeWeapon>().shoot();
        }
    }
    void StunnedGroundCheck()
    {
        if (stunned && GroundCheck.IsTouchingLayers(1 << LayerMask.NameToLayer("Ground")))
        {
            stunned = false;
            rb.gravityScale = 2f;
        }
    }
        IEnumerator ToFast()
        {
        float a = 0.1f;
        while (ToFastStarted)
        {
            if (Mathf.Abs(rb.velocity.x) > movementSpeed)
                rb.velocity = Vector2.Lerp(new Vector2(rb.velocity.x, rb.velocity.y), new Vector2(move * movementSpeed, rb.velocity.y), a);
            if (Mathf.Abs(rb.velocity.x) <= movementSpeed || GroundCheck.IsTouchingLayers(1 << LayerMask.NameToLayer("Ground")))
            {
                ToFastStarted = false;
                break;
            }
            a += a < 1 ? 0.1f : 0;
            yield return new WaitForSeconds(0.2f);
        }
    }
    public void Escape(InputAction.CallbackContext contex)
    {
        if(contex.started)
            GameObject.FindGameObjectWithTag("UI").GetComponent<UI>().TurnMenu();
    }
    public void Jump(InputAction.CallbackContext contex)
    {
        if (contex.started && GroundCheck.IsTouchingLayers(1 << LayerMask.NameToLayer("Ground")))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
        }
    }
    public void Dash()
    {
        if (canDash)
        {
            dashTime = Time.time + dashDuration;
            canDash = false;
            Invoke("ResetDash", dashCooldown);
        }
    }
    public void MeleeAttack(InputAction.CallbackContext contex)
    {
        if (contex.started)
        {
            meleeWeapon.GetComponent<Sword>().StartAttack();
        }
    }
    public void RangeAttack(InputAction.CallbackContext contex)
    {
        shooting = contex;
    }
    public void Heal(InputAction.CallbackContext contex)
    {
        if (contex.started)
        {
            if (itemEQ[(int)GameManager.ItemID.Syringe] > 0 && GetComponent<Health>().ifMaxHealth())
            {
                itemEQ[(int)GameManager.ItemID.Syringe]--;
                GetComponent<Health>().TakenDamage(-1);
            }
        }
    }
    public void PickUp(InputAction.CallbackContext contex)
    {
        if (contex.started)
        {
            pickUp = true;
            Invoke("PickUpCD", 0.2f);
            Interact?.Invoke();
        }
    }
    
    void ResetDash()
    {
        canDash = true;
    }
    
    void PickUpCD()
    {
        pickUp = false;
    }
    
    
}
