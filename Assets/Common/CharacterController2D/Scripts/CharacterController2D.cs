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

        [Tooltip("gives you the object the character is standing on")]
        public GameObject StandingOn;
        //the object the character was standing on last frame
        public GameObject StandingOnLastFrame { get; private set; }
        //the object the collider the character is standing on
        public Collider2D StandingOnCollider { get; private set; }
        public Vector2 Speed { get { return _speed; } }

        [Header("Raycasting")]
        [Tooltip("the number of ray cast horizontally")]
        public int NumberOfHorizontalRays = 8;
        [Tooltip("the number of ray cast vertically")]
        public int NumberOfVerticalRays = 8;
        [Tooltip("a small value added to all horizontal raycasts to accomodate for edge cases")]
        public float RayOffsetHorizontal = 0.05f;
        [Tooltip("a small value added to all vertic al raycasts to accomodate for edge cases")]
        public float RayOffsetVertical = 0.05f;

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
        private bool _gravityActive = true;

        private const float _smallValue = 0.0001f;

        private RaycastHit2D[] _belowHitStorage;

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

        private List<RaycastHit2D> _contactList;

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

            _belowHitStorage = new RaycastHit2D[NumberOfVerticalRays];

            State.Reset();
            SetRaysParameters();
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
            ApplyGravity();
            FrameInitialization();

            // we initialize our rays
            SetRaysParameters();

            CastRaysBelow();

            MoveTransform();
        }

        private void FrameInitialization()
        {
            _newPosition = Speed * DeltaTime;
        }

        private void MoveTransform()
        {
            _transform.Translate(_newPosition, Space.Self);
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
                _speed.y = _currentGravity * DeltaTime;
            }

            if (_fallSlowFactor != 0)
            {
                _speed.y *= _fallSlowFactor;
            }
        }

        private void CastRaysBelow()
        {
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

            float smallestDistance = float.MaxValue;
            int smallestDistanceIndex = 0;
            bool hitConnected = false;

            for (int i=0;i<NumberOfVerticalRays;i++)
            {
                Vector2 rayOriginPoint = Vector2.Lerp(_verticalRayCastFromLeft, _verticalRaycastToRight, (float)i/(float)(NumberOfVerticalRays - 1));

                if ((_newPosition.y > 0) && (!State.WasGroundedLastFrame))
                {

                } else
                {
                    _belowHitStorage[i] = MMDebug.RayCast(rayOriginPoint, -transform.up, rayLength, _raysBelowLayerMaskPlatforms, Color.blue, Parameters.DrawRaycastsGizmos);
                }

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
                State.IsCollidingBelow = false;
            }

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
    }
}