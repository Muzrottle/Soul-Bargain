using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using UnityEditor;
using UnityEngine;

public class PlayerAnimationHandler : MonoBehaviour
{
    [SerializeField] AvatarMask blockMoveMask;

    PlayerMovement playerMovement;
    PlayerAttributes playerAttributes;
    LookAtMouse lookAtMouse;
    Animator playerAnim;
    Camera mainCamera;

    bool isAttacking = false;
    public bool IsAttacking { get { return isAttacking; } }
    bool isDodging = false;
    bool isBlocking = false;
    bool isMoving = false;
    public bool IsMoving { get { return isMoving; } }
    bool isSprinting = false;
    public bool IsSprinting { get { return isSprinting; } }
    bool isJumping = false;
    public bool IsJumping { get { return isJumping; } }
    bool sprintJumped = false;
    public bool SprintJumped { get { return sprintJumped; } }
    bool isGrounded = true;
    public bool IsGrounded { get { return isGrounded; } }
    bool isLanded = true;
    bool isFalling = false;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerAttributes = GetComponent<PlayerAttributes>();
        playerAnim = GetComponent<Animator>();
        lookAtMouse = GetComponent<LookAtMouse>();
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        MovementAnim();
        DodgeAnim();
        AttackAnim();
        BlockAnim();
        JumpAnim();
    }

    private void OnAnimatorIK(int layerIndex)
    {
        playerAnim.SetLookAtWeight(1f, 0f, 0.6f, 0.8f, 0.5f);
        Ray lookAtRay = new Ray(transform.position, mainCamera.transform.forward);

        playerAnim.SetLookAtPosition(lookAtRay.GetPoint(25));
    }

    private void MovementAnim()
    {
        if (playerMovement.Move != Vector3.zero && CanMove())
        {
            isMoving = true;
            playerAnim.SetBool("isMoving", isMoving);

        }
        else
        {
            isMoving = false;
            playerAnim.SetBool("isMoving", isMoving);
        }

        if (Input.GetKey(KeyCode.LeftShift) && CanSprint())
        {
            isSprinting = true;
            playerAnim.SetBool("isSprinting", isSprinting);
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift) && isSprinting || StopSprint())
        {
            isSprinting = false;
            playerAnim.SetBool("isSprinting", isSprinting);
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

    private void AttackAnim()
    {

        if (Input.GetKeyDown(KeyCode.Mouse0) && CanAttack())
        {
            playerAnim.SetTrigger("isAttacking");
            playerAttributes.DrainInstantStamina(5f);
        }

        if (isAttacking)
        {
            playerMovement.StopLookingAtMouse();
        }
    }

    private void BlockAnim()
    {
        if (Input.GetKey(KeyCode.Mouse1) && CanBlock())
        {
            isBlocking = true;
            playerAnim.SetBool("isBlocking", isBlocking);
        }
        else if (Input.GetKeyUp(KeyCode.Mouse1) && isBlocking || StopBlock())
        {
            isBlocking = false;
            playerAnim.SetBool("isBlocking", isBlocking);
        }

        if (isBlocking && Input.GetKeyDown(KeyCode.R))
        {
            playerAnim.SetTrigger("hasBlocked");
        }
    }

    public void JumpAnim()
    {
        if (Input.GetKeyDown(KeyCode.F) && CanJump())
        {
            if (isSprinting)
            {
                sprintJumped = true;
            }
            playerMovement.PlayerJump();
            isJumping = true;
            isGrounded = false;
            isLanded = false;
            playerAnim.applyRootMotion = false;
            playerAnim.SetBool("isJumping", isJumping);
            playerAttributes.DrainInstantStamina(10f);
        }
    }

    public void TouchedGround()
    {
        Debug.Log("isLanded");

        if (sprintJumped)
        {
            playerAnim.SetTrigger("jumpRoll");
        }
        else
        {
            playerAnim.SetTrigger("isLanded");
        }

        isJumping = false;
        playerAnim.SetBool("isJumping", isJumping);
        sprintJumped = false;
        isGrounded = true;
        playerAnim.applyRootMotion = true;
        isFalling = false;
        playerAnim.SetBool("isFalling", isFalling);
    }

    public void Falling()
    {
        isLanded = false;
        isGrounded = false;
        isFalling = true;
        playerAnim.SetBool("isFalling", isFalling);
    }

    private bool CanDodge() => !isAttacking && isLanded;
    private bool CanAttack() => !isDodging && isLanded;
    public bool CanJump() => (isMoving || isSprinting) && isGrounded && !isDodging && isLanded;
    private bool CanBlock() => !isAttacking && !isDodging && !isBlocking && isLanded;
    private bool StopBlock() => (isAttacking || isDodging) && isBlocking && isLanded;
    public bool CanMove() => !isAttacking && isLanded;
    public bool CanSprint() => isMoving && !isSprinting && !isBlocking && isLanded;
    public bool StopSprint() => isBlocking || !isMoving || !isLanded;


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

    private void JumpEnded()
    {
        isLanded = true;
    }
}
