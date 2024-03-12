using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float timeToLookAtMouseAgain = 1f;

    Vector3 move;
    public Vector3 Move { get { return move; } }
    Coroutine startLookingAtMouseCoroutine;
    public Coroutine StartLookingAtMouseCoroutine { get { return startLookingAtMouseCoroutine; } }
    float lastTimeMoved = 0f;
    bool isMoving = false;

    LookAtMouse lookAtMouse;
    PlayerAnimationHandler playerAnimationHandler;

    // Start is called before the first frame update
    void Start()
    {
        lookAtMouse = GetComponent<LookAtMouse>();
        playerAnimationHandler = GetComponent<PlayerAnimationHandler>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (playerAnimationHandler.CanMove())
        {
            MovePlayer();
        }
    }

    private void MovePlayer()
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

            //lastTimeMoved = 0f;

            move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            lookAtMouse.SetPlayerLookPos(move);
        }
        else
        {
            move = new Vector3(0, 0, 0);
        }
    }

    //private void SetMouseLookAtState(bool lookAt)
    //{
    //    if (lookAt && !lookAtMouse.enabled)
    //    {
    //        StopCoroutine(StartLookingAtMouse());
    //        StartCoroutine(StartLookingAtMouse());
    //    }
    //    else
    //    {
    //        StopLookingAtMouse();
    //    }
    //}

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
