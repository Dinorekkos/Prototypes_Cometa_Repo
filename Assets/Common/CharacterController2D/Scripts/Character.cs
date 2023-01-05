using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CometaPrototypes.CharacterController2D
{
    public class Character : MonoBehaviour
    {
        public InputManager _inputManager;
        public CharacterController2D _controller;

        #region movement
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
        #endregion

        #region jumping
        [Header("Jump Behavior")]
        [Tooltip("the maximum number of jumps allowed (0 : no jump, 1 : normal, 2 : double jump, etc...)")]
        public int NumberOfJumps = 2;
        [Tooltip("defines how high the character can jump")]
        public float JumpHeight = 3.025f;

        [Header("Proportional jumps")]
        [Tooltip("if true, the jump duration/height will be proportional to the duration of the button's press")]
        public bool JumpIsProportionalToThePressTime = true;
        [Tooltip("the minimum time in the air allowed when jumping - this is used for pressure controlled jumps")]
        public float JumpMinimumAirTime = 0.1f;
        [Tooltip("the amount by witch we'll modify the current speed when the jump button gets released")]
        public float JumpReleaseForceFactor = 2f;

        [Header("Quality of Life")]
        [Tooltip("a timeframe during witch, after leaving the ground the character can still trigger a jump")]
        public float CoyoteTime = 0f;
        [Tooltip("if the character lands, and the jump button's been pressed during that InputBufferDuration, a new jump will be triggered")]
        public float InputBufferDuration = 0f;

        [Tooltip("the number of jumps left to the character")]
        public int NumberOfJumpsLeft;
        // wheter or not the jump happened this frame
        public bool JumpHappenedThisFrame { get; set; }
        // wheter or not the jump can be stopped
        public bool CanJumpStop { get; set; }

        private float _jumpButtonPressTime = 0;
        private float _lastJumpAt = 0;
        private bool _jumpButtonPressed = false;
        private bool _jumpButtonReleased = false;
        private bool _doubleJumping = false;
        private int _initialNumberOfJumps;
        private float _lastTimeGrounded = 0f;
        private float _lastInputBufferJumpAt = 0f;
        private bool _coyoteTime = false;
        private bool _inputBuffer = false;
        #endregion

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

            if (_inputManager.JumpPressed)
                JumpStart();
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

        //Evaluates the jump conditions to determine wheter or not a jump can occur
        protected bool EvaluateJumpConditions()
        {
            // if we're not grounded, not on a ladder, and don´t have any jumps left, we do nothing and exit
            if ((!_controller.State.IsGrounded))
            {
                return false;
            }

            if (_controller.State.IsGrounded
                && (NumberOfJumpsLeft <= 0))
            {
                return false;
            }

            return true;
        }

        public void JumpStart()
        {
            if (!EvaluateJumpConditions())
            {
                return;
            }

            _lastJumpAt = Time.time;

            if (NumberOfJumpsLeft != NumberOfJumps)
            {
                _doubleJumping = true;
            }

            // we decrease the number of jumps left
            NumberOfJumpsLeft = NumberOfJumpsLeft - 1;

            if (_controller.State.IsGrounded)
            {
                _controller.TimeAirborne = 0f;
            }

            _controller.GravityActive(true);
            _controller.CollisionsOn();

            // we set our 
            CanJumpStop = true;

            // we make the character jump
            _controller.SetVerticalForce(Mathf.Sqrt( 2f * JumpHeight * Mathf.Abs(_controller.Parameters.Gravity)));
            JumpHappenedThisFrame = true;
        }
    }
}