using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Diana_ChaoticPull
{ 
    public class GrappleChaoticPull : MonoBehaviour
    {
        #region variables

        [Header("Grapple Settings")]
        [SerializeField] bool is2D = false;
        [SerializeField] bool isMultiple = false;
        [SerializeField] GrappleInteractablesType _interactableType = GrappleInteractablesType.None;
        [SerializeField] float _grappleSpeed = 10f;

        [Header("Player Settings")]
        [SerializeField] Transform _playerTransform = null;

        [SerializeField] private Camera _playerCamera;
        [SerializeField] private Camera _camera2D;


        private Camera _currentCamera;
        Mouse _mouse;
        private Vector2 _mouseAxis;
        
        #endregion

        #region Unity Methods

        void Start()
        { 
#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_EDITOR || UNITY_STANDALONE_LINUX
            _mouse = Mouse.current;
#endif
        }

        void Update()
        {
            if (_mouse.leftButton.isPressed) 
            {
                if (!is2D)
                {
                    _mouseAxis = _mouse.position.ReadValue();
                    _playerCamera.gameObject.SetActive(true);
                    _camera2D.gameObject.SetActive(false);
                    _currentCamera = _playerCamera;
                    
                     
                }
                else
                {
                    _mouseAxis = _mouse.position.ReadValue();
                    _playerCamera.gameObject.SetActive(false);
                    _camera2D.gameObject.SetActive(true);
                    _currentCamera = _camera2D;
                }
               
                SendRay(_mouseAxis);
            }
            
        }
        #endregion

        #region private methods

        void SendRay(Vector3 origin)
        {
            RaycastHit hit;
            Ray ray = _currentCamera.ScreenPointToRay(origin);
            if (Physics.Raycast(ray, out hit))
            {
                GrappleObject grappleObject = hit.collider.GetComponent<GrappleObject>();
                if(grappleObject != null)
                {
                    _interactableType = grappleObject.SendGrappleInteractableType(transform.position, _grappleSpeed);

                    if (_interactableType == GrappleInteractablesType.TargetObject)
                    {
                        MovePlayerToTarget(hit.transform.position);
                    }
                }
            }
        }
        
        void MovePlayerToTarget(Vector3 targetPosition)
        {
            _playerTransform.position = Vector3.MoveTowards(_playerTransform.position, targetPosition, (_grappleSpeed * 2) * Time.deltaTime);
        }
        
        
        #endregion
        
        
    }

}