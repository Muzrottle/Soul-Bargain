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
    [SerializeField] GameObject focusParticle;
    //[SerializeField] GameObject particleCamera;

    Transform currentTarget; // Currently targeted enemy
    public Transform CurrentTarget { get { return currentTarget; } }
    float lockOnAngle = 30f;
    float lockOnRange = 10f; // Maximum range for locking on
    float lockOnCooldown = 1f; // Cooldown between lock-ons

    private bool canLockOn = true;
    bool isLockedOn = false;
    public bool IsLockedOn { get { return isLockedOn; } }

    Camera mainCamera;
    Vector3 direction;
    CinemachineFreeLook freeLookCamera;
    EnemyDetection enemyDetection;

    int verticalAxisIndex = 1;
    float xRotation = 0f;

    public float smoothTime = 0.1f; // Smoothing time
    private float currentVelocity = 0f;

    void Start()
    {
        mainCamera = Camera.main;
        freeLookCamera = FindObjectOfType<CinemachineFreeLook>();
        enemyDetection = GetComponent<EnemyDetection>();
        Cursor.lockState = CursorLockMode.Locked;

        // Disable the Y-axis input
        freeLookCamera.m_YAxis.m_InputAxisName = "Mouse Y";
        
        // Disable the Y-axis input
        freeLookCamera.m_YAxis.Value = 0f;
    }

    void Update()
    {
        ChangeCameraYaxis();

        if (Input.GetKeyDown(KeyCode.Q) && canLockOn)
        {
            if (isLockedOn)
            {
                isLockedOn = false;
            }
            else
            {
                LockOnToNextEnemy();
            }
        }

        if (isLockedOn)
        {
            freeLookCamera.m_XAxis.m_InputAxisName = "";

            if (!focusParticle.activeInHierarchy)
            {
                focusParticle.SetActive(true);
            }
            LookAtEnemy();
        }
        else
        {
            freeLookCamera.m_XAxis.m_InputAxisName = "Mouse X";

            if (focusParticle.activeInHierarchy)
            {
                focusParticle.SetActive(false);
            }
            GetMousePos();
        }
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

    void LockOnToNextEnemy()
    {
        Transform newTarget = FindClosestEnemyInView();

        if (newTarget != null)
        {
            currentTarget = newTarget;
            // Additional logic to adjust camera, UI, player controls, etc.
            Debug.Log("Locked on to: " + currentTarget.name);

            isLockedOn = true;

            // Start cooldown for locking on again
            canLockOn = false;
            Invoke("ResetLockOnCooldown", lockOnCooldown);
        }
        else
        {
            Debug.Log("No enemies in view to lock on to.");
        }
    }

    Transform FindClosestEnemyInView()
    {
        Transform nearestEnemy = null;
        float nearestDistance = Mathf.Infinity;
        Vector3 playerPos = transform.position;

        foreach (Transform enemy in enemyDetection.Enemies)
        {
            Vector3 enemyDir = enemy.position - mainCamera.transform.position;
            float angle = Vector3.Angle(mainCamera.transform.forward, enemyDir);

            if (angle <= lockOnAngle && angle < nearestDistance)
            {
                nearestEnemy = enemy;
                nearestDistance = angle;
            }
        }

        return nearestEnemy;
    }

    void ResetLockOnCooldown()
    {
        canLockOn = true;
    }

    private void LookAtEnemy()
    {
        focusParticle.transform.parent = currentTarget;
        focusParticle.transform.position = currentTarget.transform.position + new Vector3(0, currentTarget.GetComponent<CapsuleCollider>().height / 2, 0);
        //particleCamera.transform.position = mainCamera.transform.position;
        //particleCamera.transform.LookAt(focusParticle.transform);

        Vector3 direction = currentTarget.position - gameObject.transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction, gameObject.transform.up);

        // Smoothly interpolate the camera's rotation towards the target
        freeLookCamera.m_XAxis.Value = Mathf.LerpAngle(freeLookCamera.m_XAxis.Value, targetRotation.eulerAngles.y, Time.deltaTime * 20f);
        //freeLookCamera.m_YAxis.Value = Mathf.LerpAngle(freeLookCamera.m_YAxis.Value, targetRotation.eulerAngles.x, Time.deltaTime * 20f);
    }

    public void RemoveFocus()
    {
        isLockedOn = false;
        focusParticle.transform.parent = gameObject.transform;
    }

}

