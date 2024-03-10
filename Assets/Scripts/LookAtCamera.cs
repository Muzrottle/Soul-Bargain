using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class LookAtCamera : MonoBehaviour
{
    [SerializeField] float turnDuration = 0.3f;

    Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue))
        {
            Vector3 direction = raycastHit.point - transform.position;

            direction.y = 0;

            SetPlayerLookPos(direction);
            //transform.forward = direction;
        }
    }

    public void SetPlayerLookPos(Vector3 newForward)
    {
        gameObject.transform.DORotateQuaternion(Quaternion.LookRotation(newForward), turnDuration)
            .SetEase(Ease.Linear);
    }

}

