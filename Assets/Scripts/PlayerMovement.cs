using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Transform playersWeaponHand;
    public Transform PlayersWeaponHand { get { return playersWeaponHand; } }

    [SerializeField] float timeToLookAtMouseAgain = 1f;

    Vector3 direction;

    Vector3 move;
    public Vector3 Move { get { return move; } }
    Coroutine startLookingAtMouseCoroutine;
    public Coroutine StartLookingAtMouseCoroutine { get { return startLookingAtMouseCoroutine; } }

    [SerializeField] float gravityMultiplier = 3.0f;
    float gravity = -9.81f;
    float velocity;
    public float Velocity { get { return velocity; } }

    [SerializeField] float forwardSpeed;
    [SerializeField] float jumpSpeed;
    [SerializeField] float jumpHeight;
    bool isMoving = false;

    CinemachineVirtualCamera virtualCamera;
    public float rotationSpeed = 1f;
    LookAtMouse lookAtMouse;
    PlayerAnimationHandler playerAnimationHandler;
    CharacterController characterController;
    Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        lookAtMouse = GetComponent<LookAtMouse>();
        playerAnimationHandler = GetComponent<PlayerAnimationHandler>();
        characterController = GetComponent<CharacterController>();
        virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        mainCamera = Camera.main;
    }

    private void Update()
    {
        //CinemachineTransposer transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        //if (transposer != null)
        //{
        //    if (Input.GetKey(KeyCode.Q))
        //    {
        //        Debug.Log("Rotate left");
        //        transposer.m_FollowOffset.x += rotationSpeed * Time.deltaTime;
        //    }
        //    else if (Input.GetKey(KeyCode.E))
        //    {
        //        Debug.Log("Rotate right");
        //        transposer.m_FollowOffset.x -= rotationSpeed * Time.deltaTime;
        //    }
        //    Debug.Log("Current FollowOffset X: " + transposer.m_FollowOffset.x);
        //}

        ApplyGravity();

        if (playerAnimationHandler.CanMove())
        {
            PlayerMove();
        }

        if (playerAnimationHandler.IsJumping)
        {
            if (playerAnimationHandler.SprintJumped)
            {
                characterController.Move(direction * jumpSpeed * Time.deltaTime * 1.25f);
                characterController.Move(move * forwardSpeed * Time.deltaTime * 2);
            }
            else
            {
                characterController.Move(direction * jumpSpeed * Time.deltaTime);
                characterController.Move(move * forwardSpeed * Time.deltaTime);
            }
        }

        if (!characterController.isGrounded && !playerAnimationHandler.IsJumping)
        {
            //if (playerAnimationHandler.IsSprinting)
            //{
            //    characterController.Move(direction * Time.deltaTime);
            //    characterController.Move(move * forwardSpeed * Time.deltaTime);
            //}
            //else
            //{
            //    characterController.Move(direction * Time.deltaTime);
            //    characterController.Move(move * Time.deltaTime);
            //}
            characterController.Move(direction * Time.deltaTime);
            //    characterController.Move(direction * Time.deltaTime);

        }
    }

    private void ApplyGravity()
    {
        if (playerAnimationHandler.IsGrounded && velocity < 0.0f)
        {
            velocity = 0f;
        }
        else
        {
            velocity += gravity * gravityMultiplier * Time.deltaTime;
        }
        direction.y = velocity;
    }

    private void PlayerMove()
    {
        bool movementState = Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0;

        if (movementState != isMoving)
        {
            if (!movementState)
            {
                StartLookingAtMouse();
            }

            isMoving = movementState;
        }

        if (movementState)
        {
            StopLookingAtMouse();

            if (!playerAnimationHandler.IsJumping)
            {
                // Get input for movement
                float horizontalInput = Input.GetAxis("Horizontal");
                float verticalInput = Input.GetAxis("Vertical");

                // Calculate movement direction relative to camera
                Vector3 cameraForward = mainCamera.transform.forward;
                Vector3 cameraRight = mainCamera.transform.right;
                cameraForward.y = 0f;
                cameraRight.y = 0f;
                cameraForward.Normalize();
                cameraRight.Normalize();

                // Calculate movement direction
                move = cameraForward * verticalInput + cameraRight * horizontalInput;
                //move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                lookAtMouse.SetPlayerLookPos(move);
            }
        }
        else
        {
            SetMoveZero();
        }
    }

    public void PlayerJump()
    {
        velocity += jumpHeight;
    }

    public void StartLookingAtMouse()
    {
        startLookingAtMouseCoroutine = StartCoroutine(LookingAtMouseCoroutine());
    }

    private IEnumerator LookingAtMouseCoroutine()
    {
        yield return new WaitForSeconds(timeToLookAtMouseAgain);
        lookAtMouse.enabled = true;
        startLookingAtMouseCoroutine = null;
    }

    public void StopLookingAtMouse()
    {
        if (startLookingAtMouseCoroutine != null)
        {
            StopCoroutine(startLookingAtMouseCoroutine);
        }

        if (lookAtMouse.enabled)
        {
            //lookAtMouse.enabled = false;
        }
    }

    public void SetMoveZero()
    {
        move = new Vector3(0, 0, 0);
    }
}
