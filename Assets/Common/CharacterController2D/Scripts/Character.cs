using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;

namespace CometaPrototypes.CharacterController2D
{
    public class Character : MonoBehaviour
    {
        public InputManager _inputManager;
        public CharacterController2D _controller;

        [Header("Events")]
        public bool SendStateChangeEvents = true;

        public MMStateMachine<CharacterStates.MovementStates> _movementState;

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
            _movementState = new MMStateMachine<CharacterStates.MovementStates>(this.gameObject, SendStateChangeEvents);

            MovementSpeed = WalkSpeed;

            ResetNumberOfJumps();
            ResetInitialNumberOfJumps();
            CanJumpStop = true;
        }

        private void ResetInitialNumberOfJumps()
        {
            _initialNumberOfJumps = NumberOfJumps;
        }

        private void Update()
        {
            HandleInput();

            HandleHorizontalMovement();

            HandleJumpingUpdate();
        }

        private void HandleInput()
        {
            _horizontalMovement = _inputManager.Movement.x;

            if (_inputManager.JumpButton.State.CurrentState == MMInput.ButtonStates.ButtonDown)
            {
                JumpStart();
            }

            // we handle input buffer
            if ((InputBufferDuration > 0f) && (_controller.State.JustGotGrounded))
            {
                if ((_inputManager.JumpButton.TimeSinceLastButtonDown < InputBufferDuration) && (Time.time - _lastJumpAt > InputBufferDuration))
                {
                    NumberOfJumpsLeft = NumberOfJumps;
                    _doubleJumping = false;
                    _inputBuffer = true;
                    _jumpButtonPressed = (_inputManager.JumpButton.State.CurrentState == MMInput.ButtonStates.ButtonPressed);
                    _jumpButtonPressTime = Time.time;
                    _jumpButtonReleased = (_inputManager.JumpButton.State.CurrentState == MMInput.ButtonStates.ButtonPressed);
                    _lastInputBufferJumpAt = Time.time;
                    JumpStart();
                }
            }

            if (_inputManager.JumpButton.State.CurrentState == MMInput.ButtonStates.ButtonUp)
            {
                JumpStop();
            }
        }

        private void HandleHorizontalMovement()
        {
            // check if we just got grounded
            CheckJustGotGrounded();
            StoreLastTimeGrounded();

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

        private void HandleJumpingUpdate()
        {
            JumpHappenedThisFrame = false;

            // if we just got grounded, we reset our number of jumps
            if (_controller.State.JustGotGrounded && !_inputBuffer)
            {
                NumberOfJumpsLeft = NumberOfJumps;
                _doubleJumping = false;
            }

            // if we're grounded, and have jumped a while back but still haven´t gotten our jumps back, we reset them
            if ((_controller.State.IsGrounded) && (Time.time - _lastJumpAt > JumpMinimumAirTime) && (NumberOfJumpsLeft < NumberOfJumps) && !_inputBuffer)
            {
                ResetNumberOfJumps();
            }

            // we store the last timestamp at witch the character was grounded
            if (_controller.State.IsGrounded)
            {
                _lastTimeGrounded = Time.time;
            }

            // If the user releases the jump button and the character is jumping up and enought time since the initial jump has passed, then we make it stop jumping by applying a force down.
            if ((_jumpButtonPressTime != 0)
                && (Time.time - _jumpButtonPressTime >= JumpMinimumAirTime)
                && (_controller.Speed.y > Mathf.Sqrt(Mathf.Abs(_controller.Parameters.Gravity)))
                && (_jumpButtonReleased))
            {
                _jumpButtonReleased = false;
                if (JumpIsProportionalToThePressTime)
                {
                    _jumpButtonPressTime = 0;
                    if (JumpReleaseForceFactor == 0)
                    {
                        _controller.SetVerticalForce(0);
                    } else
                    {
                        _controller.AddVerticalForce(-_controller.Speed.y/JumpReleaseForceFactor);
                    }
                }
            }

            UpdateController();

            _inputBuffer = false;
        }

        protected void CheckJustGotGrounded()
        {
            if (_controller.State.JustGotGrounded)
            {
                if ((_movementState.CurrentState != CharacterStates.MovementStates.Jumping))
                {
                    //if (_controller.State.ColliderResized)
                    //{
                    //    _movementState.ChangeState(CharacterStates.MovementStates.Crouching);
                    //} else
                    //{
                        _movementState.ChangeState(CharacterStates.MovementStates.Idle);
                    //}
                }
            }
        }

        protected void StoreLastTimeGrounded()
        {
            if ((_controller.State.IsGrounded))
            {
                _lastTimeGrounded = Time.time;
            }
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
            Debug.Log("<color=green>JumpStart()</color>");

            if (!EvaluateJumpConditions())
            {
                return;
            }

            _lastJumpAt = Time.time;

            // if we're still here the jump will happen
            // we set out current state to Jumping
            _movementState.ChangeState(CharacterStates.MovementStates.Jumping);

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
            Debug.Log("SetVerticalForce: "+ Mathf.Sqrt(2f * JumpHeight * Mathf.Abs(_controller.Parameters.Gravity)));
            JumpHappenedThisFrame = true;
        }

        public void SetCanJumpStop(bool status)
        {
            CanJumpStop = status;
        }

        public void JumpStop()
        {
            Debug.Log("Call JumpStop");

            if (!CanJumpStop)
            {
                return;
            }
            _jumpButtonPressed = false;
            _jumpButtonReleased = true;
        }

        public void ResetNumberOfJumps()
        {
            NumberOfJumpsLeft = NumberOfJumps;
        }

        private void UpdateController()
        {
            _controller.State.IsJumping = (_movementState.CurrentState == CharacterStates.MovementStates.Jumping
                || _movementState.CurrentState == CharacterStates.MovementStates.DoubleJumping);
        }

        public void SetNumberOfJumpsLeft(int newNumberOfJumps)
        {
            NumberOfJumpsLeft = newNumberOfJumps;
        }
    }
}