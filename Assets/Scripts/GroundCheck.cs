using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] float raycastDistance = 0.1f;  // Length of the ray

    PlayerAnimationHandler playerAnimationHandler;
    PlayerMovement playerMovement;
    
    private void Start()
    {
        playerAnimationHandler = GetComponent<PlayerAnimationHandler>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (!playerAnimationHandler.IsGrounded)
        {
            CheckGrounded();
        }
    }

    void CheckGrounded()
    {
        Vector3 raycastOrigin = transform.position + Vector3.up * 0.1f; // Start slightly above the character
        RaycastHit hit;

        if (Physics.Raycast(raycastOrigin, Vector3.down, out hit, raycastDistance) && playerMovement.Velocity <= 0f)
        {
            playerAnimationHandler.TouchedGround();
        }
        else if (!Physics.Raycast(raycastOrigin, Vector3.down, out hit, raycastDistance) && playerAnimationHandler.IsGrounded)
        {
            Debug.Log("Character is not grounded");
            playerAnimationHandler.Falling();
        }
    }
}