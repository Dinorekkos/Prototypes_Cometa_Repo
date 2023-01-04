using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Header("Mouse Movement")]
    [Range(1f,20f)]
    [SerializeField] float sensitivityX = 50f;
    [Range(0f,5f)]
    [SerializeField] float sensitivityY= 0.5f;

    [Header("Camera")]
    [SerializeField] Transform transformCamera;

    [SerializeField] private bool visibleCursor = false;
    float cameraLimit = 85f;
    float xRotation = 0;
    float mouseX, mouseY;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        visibleCursor = Cursor.visible;
    }

    private void Update() {
        transform.Rotate(Vector3.up, mouseX * Time.deltaTime);
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -cameraLimit,cameraLimit);
        Vector3 targetRotation = transform.eulerAngles;
        targetRotation.x = xRotation;
        transformCamera.eulerAngles = targetRotation;

        
       

    }
    public void ReceiveInput (Vector2 mouseInput)
    {
        mouseX = mouseInput.x * sensitivityX;
        mouseY = mouseInput.y * sensitivityY;
    }
}
