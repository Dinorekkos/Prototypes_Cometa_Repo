using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CometaPrototypes.CharacterController2D
{
    public class CharacterStates : MonoBehaviour
    {
        public enum MovementStates
        {
            Null,
            Idle,
            Walking,
            Falling,
            Jumping,
            DoubleJumping
        }
    }
}