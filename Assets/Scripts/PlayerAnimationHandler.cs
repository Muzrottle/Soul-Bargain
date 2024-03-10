using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationHandler : MonoBehaviour
{
    PlayerMovement playerMovement;
    Animator playerAnim;

    bool isMoving;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerMovement.Move != Vector3.zero)
        {
            Move(true);
        }
        else
        {

            Move(false);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerAnim.Play("Dodge");
        }
    }

    private void Move(bool isMoving)
    {
        playerAnim.SetBool("isMoving", isMoving);
    }
}
