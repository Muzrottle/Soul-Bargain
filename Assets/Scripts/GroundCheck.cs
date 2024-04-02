using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] float raycastDistance = 0.1f;  // Length of the ray

    float fallTimer = 0f;

    PlayerAnimationHandler playerAnimationHandler;
    PlayerMovement playerMovement;
    CharacterController characterController;

    private void Start()
    {
        playerAnimationHandler = GetComponent<PlayerAnimationHandler>();
        playerMovement = GetComponent<PlayerMovement>();
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (characterController.isGrounded)
        {
            fallTimer = 0f;
        }

        if (playerAnimationHandler.IsJumping || !characterController.isGrounded || !playerAnimationHandler.IsGrounded)
        {
            CheckGrounded();
        }
    }

    void CheckGrounded()
    {
        Vector3 raycastOrigin = transform.position + Vector3.up * 0.1f; // Start slightly above the character
        RaycastHit hit;

        if (playerMovement.Velocity <= 0f && characterController.isGrounded)
        {
            playerAnimationHandler.TouchedGround();
        }
        else if (!playerAnimationHandler.IsJumping && !characterController.isGrounded)
        {
            if (fallTimer <= 0.1f)
            {
                fallTimer += Time.deltaTime;
            }
        }

        if (fallTimer >= 0.1f && playerAnimationHandler.IsGrounded && !Physics.Raycast(raycastOrigin, Vector3.down, out hit, raycastDistance))
        {
            playerAnimationHandler.Falling();
        }
    }
}