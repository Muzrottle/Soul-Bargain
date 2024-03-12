using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerAnimationHandler : MonoBehaviour
{
    [SerializeField] float timeToResetAttackPattern = 1f;

    PlayerMovement playerMovement;
    LookAtMouse lookAtMouse;
    Animator playerAnim;

    bool isAttacking = false;
    bool isDodging = false;
    bool isBlocking = false;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerAnim = GetComponent<Animator>();
        lookAtMouse = GetComponent<LookAtMouse>();
    }

    // Update is called once per frame
    void Update()
    {
        MovementAnim();
        DodgeAnim();
        AttackAnim();
        BlockAnim();
    }

    private void AttackAnim()
    {
        
        if (Input.GetKeyDown(KeyCode.Mouse0) && CanAttack())
        {
            playerAnim.SetTrigger("isAttacking");
        }

        if (isAttacking)
        {
            playerMovement.StopLookingAtMouse();
        }
    }

    private void DodgeAnim()
    {
        if (Input.GetKeyDown(KeyCode.Space) && CanDodge())
        {
            playerAnim.Play("Dodge");
            isDodging = true;
        }
    }

    private void BlockAnim()
    {
        if (Input.GetKey(KeyCode.Mouse1) && CanBlock())
        {
            Debug.Log("Icerdema");
            isBlocking = true;
            playerAnim.SetBool("isBlocking", isBlocking);
        }
        else if (Input.GetKeyUp(KeyCode.Mouse1) && isBlocking || StopBlock() )
        {
            isBlocking = false;
            playerAnim.SetBool("isBlocking", isBlocking);
        }

        if (isBlocking && Input.GetKeyDown(KeyCode.F))
        {
            playerAnim.SetTrigger("hasBlocked");
        }
    }

    private void MovementAnim()
    {
        if (playerMovement.Move != Vector3.zero && CanMove())
        {
            Move(true);
        }
        else
        {
            Move(false);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Sprint(true);
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            Sprint(false);
        }
    }

    private void Sprint(bool isSprinting)
    {
        playerAnim.SetBool("isSprinting", isSprinting);
    }

    private void Move(bool isMoving)
    {
        playerAnim.SetBool("isMoving", isMoving);
    }

    private bool CanDodge() => !isAttacking;
    private bool CanAttack() => !isDodging;
    private bool CanBlock() => !isAttacking && !isDodging && !isBlocking;
    private bool StopBlock() => isAttacking || isDodging && isBlocking;
    public bool CanMove() => !isAttacking;

    private void Attacked()
    {
        playerMovement.StopLookingAtMouse();
        isAttacking = true;
        lookAtMouse.GetMousePos();
    }

    private void AttackEnded()
    {
        isAttacking = false;

        playerMovement.StartLookingAtMouse();
    }

    private void SetDodgeStateFalse()
    {
        isDodging = false;
    }
}
