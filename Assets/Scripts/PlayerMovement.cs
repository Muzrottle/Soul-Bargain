using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float movementSpeed = 5f;

    Vector3 move;
    public Vector3 Move { get { return move; } }
    bool isMoving = false;

    CharacterController characterController;
    LookAtCamera lookAtCamera;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        lookAtCamera = GetComponent<LookAtCamera>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        bool movementState = Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0;

        if (movementState)
        {
            move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            StartCoroutine(ChangeMouseLookState(false));
            lookAtCamera.SetPlayerLookPos(move);
        }
        else
        {
            move = new Vector3(0, 0, 0);
            StartCoroutine(ChangeMouseLookState(true));
        }
    }

    private IEnumerator ChangeMouseLookState(bool mouseLookState)
    {
        if (mouseLookState)
        {
            yield return new WaitForSeconds(1f);
            lookAtCamera.enabled = mouseLookState;
            Debug.Log("true");

        }
        else
        {
            lookAtCamera.enabled = mouseLookState;
            Debug.Log("false");
        }
    }
}
