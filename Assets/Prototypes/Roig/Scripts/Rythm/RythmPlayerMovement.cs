using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RythmPlayerMovement : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] float speed = 11f;
    Vector2 horizontalInput;

    Vector3 horizontalVelocity;

    void Update()
    {
        horizontalVelocity = (Vector3.right * horizontalInput.x + Vector3.forward * horizontalInput.y) * speed;
        characterController.Move(horizontalVelocity * Time.deltaTime);
    }

    public void ReceiveInput(Vector2 input)
    {
        horizontalInput = input;
    }
}
