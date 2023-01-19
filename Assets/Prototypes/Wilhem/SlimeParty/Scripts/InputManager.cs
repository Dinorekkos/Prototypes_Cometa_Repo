using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace Prototypes.SlimeParty
{
    public class InputManager : MonoBehaviour
    {
        [Header("Settings")]
        public string PlayerID = "Player1";

        private List<MMInput.IMButton> ButtonList;

        [SerializeField] private Vector2 _movement;

        public Vector2 Movement { get => _movement; private set => _movement = value; }
        public MMInput.IMButton JumpButton { get; private set; }

        private void Start()
        {
            Initialization();
        }

        private void Initialization()
        {
            //Buttons
            ButtonList = new List<MMInput.IMButton>();

            ButtonList.Add(JumpButton = new MMInput.IMButton(PlayerID, "Jump", null, null, null));
        }

        public void OnMovement(InputAction.CallbackContext context)
        {
            Movement = context.ReadValue<Vector2>();
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            var control = context.control;

            if (control is ButtonControl button)
            {
                if (button.wasPressedThisFrame)
                {
                    JumpButton.State.ChangeState(MMInput.ButtonStates.ButtonDown);
                }

                if (button.wasReleasedThisFrame)
                {
                    JumpButton.State.ChangeState(MMInput.ButtonStates.ButtonUp);
                }
            }
        }

        private void LateUpdate()
        {
            ProcessButtonStates();
        }

        private void ProcessButtonStates()
        {
            foreach (MMInput.IMButton button in ButtonList)
            {
                if (button.State.CurrentState == MMInput.ButtonStates.ButtonDown)
                {
                    button.State.ChangeState(MMInput.ButtonStates.ButtonPressed);
                }
                if (button.State.CurrentState == MMInput.ButtonStates.ButtonUp)
                {
                    button.State.ChangeState(MMInput.ButtonStates.Off);
                }
            }
        }
    }
}