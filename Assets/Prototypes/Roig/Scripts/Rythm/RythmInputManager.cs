using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RythmInputManager : MonoBehaviour
{
    [SerializeField] private RythmPlayerMovement movement;

    RythmControls controls;
    RythmControls.NormalActions normalMovement;

    private Vector2 horizontalInput;

    void Awake()
    {
        controls = new RythmControls();
        normalMovement = controls.Normal;

        normalMovement.Movement.performed += ctx => horizontalInput = ctx.ReadValue<Vector2>();
    }

    void Update()
    {
        movement.ReceiveInput(horizontalInput);
    }

    void OnEnable()
    {
        controls.Enable();
    }

    void OnDisable()
    {
        controls.Disable();
    }
}
