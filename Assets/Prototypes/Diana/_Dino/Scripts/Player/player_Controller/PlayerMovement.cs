using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{   
    [Header("Requirements")]
    [SerializeField] private CharacterController characterController;

    [Header("Player Movement")] 
    public bool lockedMovement = false;
    [Range(0f,10f)]
    [SerializeField] private float movementSpeed = 1f;
    [SerializeField] float gravity = -30f;
    Vector3 verticalVelocity = Vector3.zero;
    Vector2 horizontalInput;

    public void ReceiveInput (Vector2 _horizontalInput)
    {
        if(lockedMovement) return;
        horizontalInput = _horizontalInput;
    }
    private void Update() {
        if (lockedMovement) return;
        Vector3 horizontalVelocity = (transform.right * horizontalInput.x + transform.forward * horizontalInput.y) * movementSpeed;
        characterController.Move(horizontalVelocity * Time.deltaTime);
    }

}
