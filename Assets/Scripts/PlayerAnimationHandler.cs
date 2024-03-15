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
    LookAtMouse lookAtMouse;
    Animator playerAnim;

    bool isAttacking = false;
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
        JumpAnim();
    }

    private void MovementAnim()
    {
        Debug.Log("IsSprinting: " +IsSprinting);
        Debug.Log("IsMoving: " + IsMoving);
        Debug.Log("IsLanded: " + IsMoving);
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
            //Vector3 jump = transform.position + transform.forward + Vector3.up * 5f;
            //transform.DOJump(jump, 2f, 1, 1f)
            //.SetEase(Ease.Linear)
            //.OnComplete(() =>
            //{
            //    ApplyGravity();
            //});
        }

        //if (Input.GetKeyDown(KeyCode.V))
        //{
        //    Debug.Log("Zýpla");
        //    Vector3 jump = transform.position + transform.forward + Vector3.up * 5f;
        //    playerAnim.applyRootMotion = false;

        //    transform.DOJump(jump, 2f, 1, 1f)
        //    .SetEase(Ease.Linear)
        //    .OnComplete(()=>{ playerAnim.applyRootMotion = true; });
        //}
        //if (CanJump())
        //{
        //    playerAnim.applyRootMotion = false;
        //    isGrounded = false;
        //    playerAnim.SetBool("isJumping", isJumping);
        //}
    }

    //private void StopJumping()
    //{
    //    if (isJumping && characterController.isGrounded && playerMovement.Velocity < 0f)
    //    {
    //        playerAnim.applyRootMotion = true;
    //        isGrounded = true;
    //        isJumping = false;
    //        playerAnim.SetBool("isJumping", isJumping);
    //    }
    //}

    //void ApplyGravity()
    //{
    //    // Calculate the time it takes to fall back to the ground
    //    float timeToGround = 1f * 0.5f;

    //    // Decrease the jump height for the downward motion
    //    float jumpHeight = 0;

    //    // Use DOTween to animate the downward motion (falling back to the ground)
    //    transform.DOMoveY(0, timeToGround)
    //        .SetEase(Ease.InQuad)
    //        .OnStart(() =>
    //        {
    //            Debug.Log("Gravity applied!");
    //        })
    //        .OnComplete(() =>
    //        {
    //            Debug.Log("Landed on the ground!");

    //            // Reset the jump height for the next jump
    //            jumpHeight = 2f;
    //        });
    //}

    //public void HasJumped()
    //{
    //    isJumping = true;
    //}

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
    private bool StopBlock() => isAttacking || isDodging && isBlocking && !isLanded;
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
