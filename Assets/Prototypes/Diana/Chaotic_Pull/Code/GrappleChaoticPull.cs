using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Diana_ChaoticPull
{ 
    public class GrappleChaoticPull : MonoBehaviour
    {
        #region variables

        [Header("Grapple Settings")]
        [SerializeField] bool is2D = false;
        [SerializeField] bool isMultiple = false;
        

        Mouse _mouse;
        private Vector2 _mouseAxis;
        GrappleInteractablesType _interactableType;
        
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
            if (!is2D)
            {
                if (_mouse.leftButton.wasPressedThisFrame)
                {
                    _mouseAxis = _mouse.position.ReadValue();
                    SendRay(_mouseAxis);
                }
            }
        }
        #endregion

        #region private methods

        void SendRay(Vector3 origin)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(origin);
            if (Physics.Raycast(ray, out hit))
            {
                GrappleObject grappleObject = hit.collider.GetComponent<GrappleObject>();
                if(grappleObject != null)
                {
                    _interactableType = grappleObject.SendGrappleInteractableType(transform.position);
                }
            }
        }
        #endregion
        
        
    }

}