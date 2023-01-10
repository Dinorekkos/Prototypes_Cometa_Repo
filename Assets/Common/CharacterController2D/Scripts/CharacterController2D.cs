using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;

namespace CometaPrototypes.CharacterController2D
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class CharacterController2D : MonoBehaviour
    {
        public CharacterController2DState State { get; private set; }

        [Header("Parameters")]
        public CharacterControllerParameters Parameters;

        [Header("Collisions")]
        [Tooltip("The layer mask the platforms are on")]
        public LayerMask PlatformMask;

        [Header("Safe Mode")]
        [Tooltip("Whether or not to perform additional checks when setting the transform's position. Slightly more expensive in terms of performance, but also safer. ")]
        public bool SafeSetTransform = false;
        [Tooltip("if this is true, this controller will set a number of physics settings automatically on init, to ensure they're correct")]
        public bool AutomaticallySetPhysicsSettings = true;

        [Tooltip("gives you the object the character is standing on")]
        public GameObject StandingOn;
        //the object the character was standing on last frame
        public GameObject StandingOnLastFrame { get; private set; }
        //the object the collider the character is standing on
        public Collider2D StandingOnCollider { get; private set; }
        public Vector2 Speed { get { return _speed; } }
        //the world speed of the characgter
        public Vector2 WorldSpeed { get { return _worldSpeed; } }

        [Header("Raycasting")]
        [Tooltip("the number of ray cast horizontally")]
        public int NumberOfHorizontalRays = 8;
        [Tooltip("the number of ray cast vertically")]
        public int NumberOfVerticalRays = 8;
        [Tooltip("a small value added to all horizontal raycasts to accomodate for edge cases")]
        public float RayOffsetHorizontal = 0.05f;
        [Tooltip("a small value added to all vertic al raycasts to accomodate for edge cases")]
        public float RayOffsetVertical = 0.05f;
        [Tooltip("the maximum length of the ray used to detect the distance to the ground")]
        public float DistanceToTheGroundRayMaximumLength = 100f;


        [Tooltip("the time (inseconds) since the last time the character was grounded")]
        public float TimeAirborne = 0f;

        public float DeltaTime { get { return Time.deltaTime; } }

        public Vector2 ExternalForce
        {
            get
            {
                return _externalForce;
            }
        }

        private Vector2 _speed;
        private float _friction = 0;
        private float _fallSlowFactor;
        private float _currentGravity = 0;
        private Vector2 _externalForce;
        private Vector2 _newPosition;
        private Transform _transform;
        private BoxCollider2D _boxCollider;
        private LayerMask _platformMaskSave;
        private LayerMask _raysBelowLayerMaskPlatforms;
        private int _savedBelowLayer;
        private bool _gravityActive = true;
        private Collider2D _ignoredCollider = null;

        private const float _smallValue = 0.0001f;

        private RaycastHit2D[] _belowHitStorage;
        private RaycastHit2D _distanceToTheGroundRaycast;

        private Vector2 _horizontalRayCastFromBottom = Vector2.zero;
        private Vector2 _horizontalRayCastToTop = Vector2.zero;
        private Vector2 _verticalRayCastFromLeft = Vector2.zero;
        private Vector2 _verticalRaycastToRight = Vector2.zero;
        private Vector2 _rayCastOrigin = Vector2.zero;

        private Vector2 _boundsTopLeftCorner;
        private Vector2 _boundsBottomLeftCorner;
        private Vector2 _boundsTopRightCorner;
        private Vector2 _boundsBottomRightCorner;
        private Vector2 _boundsCenter;
        private Vector2 _bounds;
        private float _boundsWidth;
        private float _boundsHeight;
        private float _distanceToTheGround;
        private Vector2 _worldSpeed;

        private List<RaycastHit2D> _contactList;
        private bool _shouldComputeNewSpeed = false;

        private void Awake()
        {
            Initialization();
        }

        private void Initialization()
        {
            _transform = transform;
            _boxCollider = GetComponent<BoxCollider2D>();

            _contactList = new List<RaycastHit2D>();
            State = new CharacterController2DState();

            _platformMaskSave = PlatformMask;

            _belowHitStorage = new RaycastHit2D[NumberOfVerticalRays];

            State.Reset();
            SetRaysParameters();

            //ApplyGravitySettings();
            ApplyPhysicsSettings();
        }

        private void ApplyPhysicsSettings()
        {
            if (AutomaticallySetPhysicsSettings)
            {
                Physics2D.queriesHitTriggers = true;
                Physics2D.queriesStartInColliders = true;
                Physics2D.callbacksOnDisable = true;
                Physics2D.reuseCollisionCallbacks = false;
                Physics2D.autoSyncTransforms = true;
            }
        }

        public void AddForce(Vector2 force)
        {
            _speed += force;
            _externalForce += force;
            ClampSpeed();
            ClampExternalForce();
        }

        public void AddHorizontalForce(float x)
        {
            _speed.x += x;
            _externalForce.x += x;
            ClampSpeed();
            ClampExternalForce();
        }

        public void AddVerticalForce(float y)
        {
            _speed.y += y;
            _externalForce.y += y;
            ClampSpeed();
            ClampExternalForce();
        }

        public void SetForce(Vector2 force)
        {
            _speed = force;
            _externalForce = force;
            ClampSpeed();
            ClampExternalForce();
        }

        public void SetHorizontalForce(float x)
        {
            _speed.x = x;
            _externalForce.x = x;
            ClampSpeed();
            ClampExternalForce();
        }

        public void SetVerticalForce(float y)
        {
            _speed.y = y;
            _externalForce.y = y;
            ClampSpeed();
            ClampExternalForce();
        }

        private void Update()
        {
            EveryFrame();
        }

        private void EveryFrame()
        {
            if (Time.timeScale == 0f)
            {
                return;
            }

            ApplyGravity();
            FrameInitialization();

            // we initialize our rays
            SetRaysParameters();

            CastRaysBelow();

            MoveTransform();

            SetRaysParameters();
            ComputeNewSpeed();
            SetStates();
            ComputeDistanceToTheGround();

            _externalForce.x = 0f;
            _externalForce.y = 0f;

            FrameExit();

            _worldSpeed = Speed;
        }

        private void FrameInitialization()
        {
            _contactList.Clear();
            // we initialize our newposition, witch we'll use in all the next computations
            _newPosition = Speed * DeltaTime;

            //if (Speed.y > 0)
            //{
            //    Debug.Log("FrameInitialization, _newPosition: "+_newPosition.ToString("F4")+"Speed: "+Speed.ToString("F4"));
            //}

            State.WasGroundedLastFrame = State.IsCollidingBelow;
            StandingOnLastFrame = StandingOn;

            _shouldComputeNewSpeed = true;
            State.Reset();
        }

        private void FrameExit()
        {
            // on frame exit we put our standing on last frame object back to where it belongs
            if (StandingOnLastFrame != null)
            {
                StandingOnLastFrame.layer = _savedBelowLayer;
            }
        }

        private void MoveTransform()
        {
            _transform.Translate(_newPosition, Space.Self);

            //string moveText = "MoveTransform, newPosition: " + _newPosition.ToString("F4");

            //if (_newPosition.y > 0)
            //{
            //    Debug.Log("<color=green>" + moveText + "</color>");
            //}
            //else
            //{
            //    Debug.Log(moveText);
            //}
        }

        private void ApplyGravity()
        {
            _currentGravity = Parameters.Gravity;

            if (_speed.y > 0)
            {
                _currentGravity = _currentGravity / Parameters.AscentMultiplier;
            }
            if (_speed.y < 0)
            {
                _currentGravity = _currentGravity / Parameters.FallMultiplier;
            }

            if (_gravityActive)
            {
                _speed.y += (_currentGravity) * DeltaTime;
            }

            if (_fallSlowFactor != 0)
            {
                _speed.y *= _fallSlowFactor;
            }
        }

        private void CastRaysBelow()
        {
            //Debug.Log("CastRaysBelow");
            _friction = 0;

            if (_newPosition.y < -_smallValue)
            {
                State.IsFalling = true;
            }
            else
            {
                State.IsFalling = false;
            }

            if ((Parameters.Gravity > 0) && (!State.IsFalling))
            {
                State.IsCollidingBelow = false;
                return;
            }

            float rayLength = (_boundsHeight / 2) + RayOffsetVertical;

            if (_newPosition.y < 0)
            {
                rayLength += Mathf.Abs(_newPosition.y);
            }

            _verticalRayCastFromLeft = (_boundsBottomLeftCorner + _boundsTopLeftCorner) / 2;
            _verticalRaycastToRight = (_boundsBottomRightCorner + _boundsTopRightCorner) / 2;
            _verticalRayCastFromLeft += (Vector2)transform.up * RayOffsetVertical;
            _verticalRaycastToRight += (Vector2)transform.up * RayOffsetVertical;
            _verticalRayCastFromLeft += (Vector2)transform.right * _newPosition.x;
            _verticalRaycastToRight += (Vector2)transform.right * _newPosition.x;

            if (_belowHitStorage.Length != NumberOfVerticalRays)
            {
                _belowHitStorage = new RaycastHit2D[NumberOfVerticalRays];
            }

            _raysBelowLayerMaskPlatforms = PlatformMask;

            // if what we're standing on is a mid height oneway platform, we turn it into a regular platform for this frame only
            if (StandingOnLastFrame != null)
            {
                _savedBelowLayer = StandingOnLastFrame.layer;
                //if (MidHeightOneWayPlatformMask.MMContains(StandingOnLastFrame.layer))
                //{
                //    StandingOnLastFrame.layer = LayerMask.NameToLayer("Platforms");
                //}
            }

            float smallestDistance = float.MaxValue;
            int smallestDistanceIndex = 0;
            bool hitConnected = false;

            for (int i=0;i<NumberOfVerticalRays;i++)
            {
                Vector2 rayOriginPoint = Vector2.Lerp(_verticalRayCastFromLeft, _verticalRaycastToRight, (float)i/(float)(NumberOfVerticalRays - 1));

                //if ((_newPosition.y > 0) && (!State.WasGroundedLastFrame))
                //{
                //    //_belowHitStorage[i] = MMDebug.RayCast(rayOriginPoint, -transform.up, rayLength, _raysBelowLayerMaskPlatformsWithoutOneWay, Color.blue, Parameters.DrawRaycastsGizmos);
                //    _belowHitStorage[i] = MMDebug.RayCast(rayOriginPoint, -transform.up, rayLength, _raysBelowLayerMaskPlatforms, Color.blue, Parameters.DrawRaycastsGizmos);
                //} else
                //{
                //    _belowHitStorage[i] = MMDebug.RayCast(rayOriginPoint, -transform.up, rayLength, _raysBelowLayerMaskPlatforms, Color.blue, Parameters.DrawRaycastsGizmos);
                //}

                _belowHitStorage[i] = MMDebug.RayCast(rayOriginPoint, -transform.up, rayLength, _raysBelowLayerMaskPlatforms, Color.blue, Parameters.DrawRaycastsGizmos);
                //Debug.Log("Raycast, rayOriginPoint: "+rayOriginPoint+", direction: "+
                //    (-_transform.up)+", rayLenght: "+rayLength+", layermask: "+_raysBelowLayerMaskPlatforms.value);

                float distance = MMMaths.DistanceBetweenPointAndLine(_belowHitStorage[smallestDistanceIndex].point, _verticalRayCastFromLeft, _verticalRaycastToRight);

                if (_belowHitStorage[i])
                {
                    hitConnected = true;

                    if (_belowHitStorage[i].distance < smallestDistance)
                    {
                        smallestDistanceIndex = i;
                        smallestDistance = _belowHitStorage[i].distance;
                    }
                }

                if (distance < _smallValue)
                {
                    break;
                }
            }

            if (hitConnected)
            {
                StandingOn = _belowHitStorage[smallestDistanceIndex].collider.gameObject;
                StandingOnCollider = _belowHitStorage[smallestDistanceIndex].collider;

                //Debug.Log("<color=cyan>Hit CONNECTED, colliding with : "+StandingOn.name+"</color>");

                State.IsFalling = false;
                State.IsCollidingBelow = true;

                // if we're applying an external force (jumping, jetpack...) we only apply that
                if (_externalForce.y > 0 && _speed.y > 0)
                {
                    _newPosition.y = _speed.y * DeltaTime;
                    State.IsCollidingBelow = false;

                }
                // if not, we just adjust the position based on the raycast hit
                else
                {
                    float distance = MMMaths.DistanceBetweenPointAndLine(_belowHitStorage[smallestDistanceIndex].point, _verticalRayCastFromLeft, _verticalRaycastToRight);

                    _newPosition.y = -distance
                        + _boundsHeight / 2
                        + RayOffsetVertical;
                }

                if (!State.WasGroundedLastFrame && _speed.y > 0)
                {
                    _newPosition.y += _speed.y * DeltaTime;
                }

                if (Mathf.Abs(_newPosition.y) < _smallValue)
                {
                    _newPosition.y = 0;
                }
            }
            else
            {
                //Debug.Log("<color=red>Hit not connected</color>");

                State.IsCollidingBelow = false;
            }

        }

        //Computes the new speed of the character
        private void ComputeNewSpeed()
        {
            // we compute the new speed
            if ((DeltaTime > 0) && _shouldComputeNewSpeed)
            {
                _speed = _newPosition / DeltaTime;
            }

            //// we apply our slope speed factor based on the slope's angle
            //if (State.IsGrounded)
            //{
            //    _speed.x *= Parameters.SlopeAngleSpeedFactor.Evaluate(Mathf.Abs(State.BelowSlopeAngle) * Mathf.Sign(_speed.y));
            //}

            //if (!State.OnAMovingPlatform)
            //{
                // we make sure the velocity doesn´t exeed the MaxVelocity specified in the parameters
                ClampSpeed();
                ClampExternalForce();
            //}
        }

        private void ClampSpeed()
        {
            _speed.x = Mathf.Clamp(_speed.x, -Parameters.MaxVelocity.x, Parameters.MaxVelocity.x);
            _speed.y = Mathf.Clamp(_speed.y, -Parameters.MaxVelocity.y, Parameters.MaxVelocity.y);
        }

        private void ClampExternalForce()
        {
            _externalForce.x = Mathf.Clamp(_externalForce.x, -Parameters.MaxVelocity.x, Parameters.MaxVelocity.x);
            _externalForce.y = Mathf.Clamp(_externalForce.y, -Parameters.MaxVelocity.y, Parameters.MaxVelocity.y);
        }

        private void SetStates()
        {
            if (!State.WasGroundedLastFrame && State.IsCollidingBelow)
            {
                State.JustGotGrounded = true;
            }

            //if (State.IsCollidingLeft || State.IsCollidingRight || State.IsCollidingBelow || State.IsCollidingAbove)
            if (State.IsCollidingBelow)
            {
                OnCharacterColliderHit();
            }
        }

        private void ComputeDistanceToTheGround()
        {
            if (DistanceToTheGroundRayMaximumLength <= 0)
            {
                return;
            }

            if (State.IsGrounded)
            {
                _distanceToTheGround = 0f;
                return;
            }

            //_rayCastOrigin.x = (State.BellowSlopeAngle < 0) ? _boundsBottomLeftCorner.x : _boundsBottomRightCorner.x;
            _rayCastOrigin.x = _boundsBottomRightCorner.x;
            _rayCastOrigin.y = _boundsCenter.y;

            _distanceToTheGroundRaycast = MMDebug.RayCast(_rayCastOrigin, -transform.up, DistanceToTheGroundRayMaximumLength, _raysBelowLayerMaskPlatforms, MMColors.CadetBlue, true);

            if (_distanceToTheGroundRaycast)
            {
                if (_distanceToTheGroundRaycast.collider == _ignoredCollider)
                {
                    _distanceToTheGround = -1f;
                    return;
                }
                _distanceToTheGround = _distanceToTheGroundRaycast.distance - _boundsHeight / 2f;
            } else
            {
                _distanceToTheGround = -1f;
            }
        }

        private void SetRaysParameters()
        {
            float top = _boxCollider.offset.y + (_boxCollider.size.y / 2f);
            float botton = _boxCollider.offset.y - (_boxCollider.size.y / 2f);
            float left = _boxCollider.offset.x - (_boxCollider.size.x / 2f);
            float right = _boxCollider.offset.x + (_boxCollider.size.x / 2f);

            _boundsTopLeftCorner.x = left;
            _boundsTopLeftCorner.y = top;

            _boundsTopRightCorner.x = right;
            _boundsTopRightCorner.y = top;

            _boundsBottomLeftCorner.x = left;
            _boundsBottomLeftCorner.y = botton;

            _boundsBottomRightCorner.x = right;
            _boundsBottomRightCorner.y = botton;

            _boundsTopLeftCorner = transform.TransformPoint(_boundsTopLeftCorner);
            _boundsTopRightCorner = transform.TransformPoint(_boundsTopRightCorner);
            _boundsBottomLeftCorner = transform.TransformPoint(_boundsBottomLeftCorner);
            _boundsBottomRightCorner = transform.TransformPoint(_boundsBottomRightCorner);
            _boundsCenter = _boxCollider.bounds.center;

            _boundsWidth = Vector2.Distance(_boundsBottomLeftCorner, _boundsBottomRightCorner);
            _boundsHeight = Vector2.Distance(_boundsBottomLeftCorner, _boundsTopLeftCorner);
        }

        public void CollisionsOn()
        {
            PlatformMask = _platformMaskSave;
        }

        public void GravityActive(bool state)
        {
            if (state)
            {
                _gravityActive = true;
            }
            else
            {
                _gravityActive = false;
            }
        }

        private void OnCharacterColliderHit()
        {
            foreach (RaycastHit2D hit in _contactList)
            {
                if (Parameters.Physics2DInteraction)
                {
                    Rigidbody2D body = hit.collider.attachedRigidbody;
                    if (body == null || body.isKinematic || body.bodyType == RigidbodyType2D.Static)
                    {
                        return;
                    }
                    Vector3 pushDirection = new Vector3(_externalForce.x, 0, 0);
                    body.velocity = pushDirection.normalized * Parameters.Physics2DPushForce;
                }
            }
        }
    }
}