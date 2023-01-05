using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using MoreMountains.Tools;
using UnityEngine.InputSystem.Controls;

namespace CometaPrototypes.CharacterController2D
{
    public class InputManager : MonoBehaviour
    {
        public Input_CharacterController2D InputActions;

        [SerializeField] private Vector2 _movement;
        [SerializeField] private bool _jumpPressed;

        public Vector2 Movement { get => _movement; private set => _movement = value; }
        public bool JumpPressed { get => _jumpPressed; private set => _jumpPressed = value; }

        private void Awake()
        {
            InputActions = new Input_CharacterController2D();
        }

        private void Start()
        {
            Initialization();
        }

        private void Initialization()
        {
            InputActions.Player2D.Movement.performed += context => Movement = context.ReadValue<Vector2>();
            InputActions.Player2D.Jump.performed += context => { OnJumpPerformed(context); };
        }

        private void OnJumpPerformed(InputAction.CallbackContext context)
        {
            var control = context.control;

            if (control is ButtonControl button)
            {
                if (button.wasPressedThisFrame)
                {
                    JumpPressed = true;
                }
                if (button.wasReleasedThisFrame)
                {
                    JumpPressed = false;
                }
            }
        }

        private void OnEnable()
        {
            InputActions.Enable();
        }

        private void OnDisable()
        {
            InputActions.Disable();
        }
    }
}