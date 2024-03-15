using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
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

    LookAtMouse lookAtMouse;
    PlayerAnimationHandler playerAnimationHandler;
    CharacterController characterController;

    // Start is called before the first frame update
    void Start()
    {
        lookAtMouse = GetComponent<LookAtMouse>();
        playerAnimationHandler = GetComponent<PlayerAnimationHandler>();
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
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
    }
    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.collider.GetComponent<Obstacle>().StopJumping == true)
    //    {
    //        isGrounded = true;
    //    }
    //}

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
                move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                lookAtMouse.SetPlayerLookPos(move);
            }

        }
        else
        {
            move = new Vector3(0, 0, 0);
        }
    }

    public void PlayerJump()
    {
        velocity += jumpHeight;
        //float inputH = Input.GetAxis("Horizontal");
        //float inputV = Input.GetAxis("Vertical");

        //Vector3 forward = transform.forward * inputV;
        //Vector3 right = transform.right * inputH;

        //// Normalize the movement vector and add forward speed
        //Vector3 moveDirection = (forward + right).normalized * forwardSpeed;
        //characterController.Move(moveDirection * forwardSpeed * Time.deltaTime);
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
            lookAtMouse.enabled = false;
        }
    }
}
