using Cinemachine;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class LookAtMouse : MonoBehaviour
{
    [SerializeField] float turnDuration = 0.3f;
    [SerializeField] float mouseSensitivity = 100f;
    [SerializeField] float wheelSpeed = 1f;

    Camera mainCamera;
    Vector3 direction;
    CinemachineFreeLook freeLookCamera;

    int verticalAxisIndex = 1;
    float xRotation = 0f;

    public float smoothTime = 0.1f; // Smoothing time
    private float currentVelocity = 0f;

    void Start()
    {
        mainCamera = Camera.main;
        freeLookCamera = FindObjectOfType<CinemachineFreeLook>();
        Cursor.lockState = CursorLockMode.Locked;

        // Disable the Y-axis input
        freeLookCamera.m_YAxis.m_InputAxisName = "";
        
        // Disable the Y-axis input
        freeLookCamera.m_YAxis.Value = 0f;
    }

    void Update()
    {
        ChangeCameraYaxis();
        GetMousePos();
    }

    private void ChangeCameraYaxis()
    {
        // Get the mouse wheel input
        float mouseWheelInput = Input.GetAxis("Mouse ScrollWheel");

        // Adjust the new Y-axis value based on mouse wheel input
        float newYAxis = mouseWheelInput * wheelSpeed;

        // Update the second orbit (Y-axis) radius
        freeLookCamera.m_YAxis.Value += newYAxis * Time.deltaTime;
    }

    public void GetMousePos()
    {
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        //float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Rotate the Cinemachine FreeLook camera
        freeLookCamera.m_XAxis.m_InputAxisValue += -mouseX;
        //freeLookCamera.m_YAxis.m_InputAxisValue += mouseY;

        
        
        //Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        //if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue))
        //{
        //    if (raycastHit.collider.gameObject.GetComponent<Enemy>())
        //    {
        //        direction = raycastHit.collider.transform.position - transform.position;
        //    }
        //    else
        //    {
        //        direction = raycastHit.point - transform.position;
        //    }

        //    direction.y = 0;

        //    SetPlayerLookPos(direction);

        //    //transform.forward = direction;
        //}
    }

    public void SetPlayerLookPos(Vector3 newForward)
    {
        gameObject.transform.DORotateQuaternion(Quaternion.LookRotation(newForward), turnDuration)
            .SetEase(Ease.Linear);
    }

}

