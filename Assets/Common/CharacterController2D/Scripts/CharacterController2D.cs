using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;

namespace CometaPrototypes.CharacterController2D
{
    public class CharacterController2D : MonoBehaviour
    {
        [Header("Parameters")]
        public CharacterControllerParameters Parameters;

        [Header("Collisions")]
        public LayerMask PlatformMask;

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

        public float DeltaTime { get { return Time.deltaTime; } }

        private Vector2 _speed;
        private float _friction = 0;
        private float _fallSlowFactor;
        private float _currentGravity = 0;
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
        }

        public void SetHorizontalForce(float x)
        {
            _speed.x = x;
            ClampSpeed();

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

                float distance = MMMaths.DistanceBetweenPointAndLine(_belowHitStorage[smallestDistanceIndex].point, _verticalRayCastFromLeft, _verticalRaycastToRight);

                if (_belowHitStorage[i])
                {
                    hitConnected = true;

                }

                if (distance < _smallValue)
                {
                    break;
                }
            }

            if (hitConnected)
            {

            }

        }

        private void ClampSpeed()
        {
            _speed.x = Mathf.Clamp(_speed.x, -Parameters.MaxVelocity.x, Parameters.MaxVelocity.x);
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
    }
}