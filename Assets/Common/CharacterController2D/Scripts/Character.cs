using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CometaPrototypes.CharacterController2D
{
    public class Character : MonoBehaviour
    {
        public InputManager myInputManager;

        private void Update()
        {
            if (myInputManager.Movement.x != 0)
            {
                Debug.Log("Move");
            }

            if (myInputManager.JumpPressed)
            {
                Debug.Log("Jump");
            }
        }
    }
}