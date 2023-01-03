using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CometaPrototypes.CharacterController2D
{
    public class InputManager : MonoBehaviour
    {
        public bool jumpPressed;
        public Vector2 movementValue;

        public void OnMovementAction(InputAction.CallbackContext context){
            //movementValue = context.ReadValue<Vector2>();
            Debug.Log ("OnMovementAction!");
        }
        public void OnJumpAction(InputAction.CallbackContext context){
            //jumpPressed = context.ReadValue<bool>();
            Debug.Log ("OnJumpAction!");
        }
    }
}