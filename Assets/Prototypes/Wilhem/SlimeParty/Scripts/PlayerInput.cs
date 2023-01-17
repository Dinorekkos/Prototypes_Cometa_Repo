using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace Prototypes.SlimeParty
{
    public class PlayerInput : MonoBehaviour
    {
        [Header("Settings")]
        public string PlayerID = "Player1";

        public Input_SlimeParty InputActions;

        private List<MMInput.IMButton> ButtonList;

        [SerializeField] private Vector2 _movement;

        public Vector2 Movement { get => _movement; private set => _movement = value; }
        public MMInput.IMButton JumpButton { get; private set; }

        private void Awake()
        {
            InputActions = new Input_SlimeParty();
        }

        private void Start()
        {
            Initialization();
        }

        private void Initialization()
        {
            InputActions.Player.Movement.ApplyBindingOverride(new InputBinding
            {
                groups = "",
                overridePath = "<Keyboar>"
            });

            InputActions.Player.Movement.performed += context => Movement = context.ReadValue<Vector2>();

            //Buttons
            ButtonList = new List<MMInput.IMButton>();

            ButtonList.Add(JumpButton = new MMInput.IMButton(PlayerID, "Jump", null, null, null));
            InputActions.Player.Jump.performed += context => { BindButton(context, JumpButton); };
        }

        protected virtual void BindButton(InputAction.CallbackContext context, MMInput.IMButton imButton)
        {
            var control = context.control;

            if (control is ButtonControl button)
            {
                if (button.wasPressedThisFrame)
                {
                    imButton.State.ChangeState(MMInput.ButtonStates.ButtonDown);
                }
                if (button.wasReleasedThisFrame)
                {
                    imButton.State.ChangeState(MMInput.ButtonStates.ButtonUp);
                }
            }
        }

        private void LateUpdate()
        {
            ProcessButtonStates();
        }

        private void OnEnable()
        {
            InputActions.Enable();
        }

        private void OnDisable()
        {
            InputActions.Disable();
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