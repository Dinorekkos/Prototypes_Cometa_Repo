using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CometaPrototypes.CharacterController2D
{
    public class Character : MonoBehaviour
    {
        public InputManager _inputManager;
        public CharacterController2D _controller;

        //the current reference movement speed
        public float MovementSpeed { get; set; }
        [Header("Speed")]
        // the speed of the character when it's walking
        public float WalkSpeed = 6f;
        // the multiplier to apply to the horizontal movement
        public float MovementSpeedMultiplier = 1f;

        //the current horizontal movement force
        public float HorizontalMovementForce { get { return _horizontalMovementForce; } }

        [Header("Input")]
        [Tooltip("the threshold after witch input is considered (usually 0.1f to eliminate small joystick noise)")]
        public float InputThreshold = 0.1f;

        private float _horizontalMovement;
        private float _horizontalMovementForce;
        private float _normalizedHorizontalSpeed;

        private void Start()
        {
            Initialization();
        }

        private void Initialization()
        {
            MovementSpeed = WalkSpeed;
        }

        private void Update()
        {
            HandleInput();

            HandleHorizontalMovement();
        }

        private void HandleInput()
        {
            _horizontalMovement = _inputManager.Movement.x;
        }

        private void HandleHorizontalMovement()
        {
            //If the value of the horizontal axis is positive, de character must face right
            if (_horizontalMovement > InputThreshold)
            {
                _normalizedHorizontalSpeed = _horizontalMovement;

            }
            //If its negative, then we're facing left
            else if (_horizontalMovement < -InputThreshold)
            {
                _normalizedHorizontalSpeed = _horizontalMovement;
            }
            else
            {
                _normalizedHorizontalSpeed = 0;
            }


            float groundAcceleration = _controller.Parameters.SpeedAccelerationOnGround;

            float movementFactor = groundAcceleration;
            float movementSpeed = _normalizedHorizontalSpeed * MovementSpeed * _controller.Parameters.SpeedFactor * MovementSpeedMultiplier;

            //If we are not in instant acceleration mode, we lerp our movement speed
            _horizontalMovementForce = Mathf.Lerp(_controller.Speed.x, movementSpeed, Time.deltaTime * movementFactor);

            _controller.SetHorizontalForce(_horizontalMovementForce);
        }
    }
}