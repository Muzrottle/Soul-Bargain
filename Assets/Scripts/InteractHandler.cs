using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractHandler : MonoBehaviour
{
    [SerializeField] GameObject interactText;
    [SerializeField] float raycastDistance = 2f;
    [SerializeField] LayerMask interactableObjectMask;

    Transform interactableObject;

    Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        FindInteractable();

        if (Input.GetKeyDown(KeyCode.E) && interactableObject != null)
        {
            interactableObject.GetComponent<Obstacle>().Interacted();
        }
    }

    private void FindInteractable()
    {
        Vector3 raycastOrigin = mainCamera.transform.position;
        RaycastHit hit;

        if (Physics.Raycast(raycastOrigin, mainCamera.transform.forward, out hit, raycastDistance, interactableObjectMask))
        {
            if (hit.transform.GetComponent<Obstacle>())
            {
                if (hit.transform.GetComponent<Obstacle>().IsInteractable && Vector3.Distance(gameObject.transform.position, hit.transform.position) < 2f)
                {
                    interactableObject = hit.transform;
                    if (!hit.transform.GetComponent<Obstacle>().IsInteracted || hit.transform.GetComponent<Obstacle>().IsClosable)
                    {
                        InteractTextSetActive(true);
                    }
                }
            }
        }
        else
        {
            interactableObject = null;
            InteractTextSetActive(false);
        }
    }

    public void InteractTextSetActive(bool activeState)
    {
        interactText.SetActive(activeState);
    }
}
