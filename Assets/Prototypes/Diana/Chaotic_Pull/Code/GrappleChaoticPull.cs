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
            if (_mouse.leftButton.wasPressedThisFrame)
            {
                Debug.Log("Left Click");
                SendRay();       
            }
        }
        #endregion

        #region private methods

        void SendRay()
        {
            _mouseAxis = _mouse.position.ReadValue();
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(_mouseAxis);
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.collider.gameObject.name);
            }
        }
        #endregion
        
        
    }

}