using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace Diana_ChaoticPull
{ 
    public class PlayerCamera : MonoBehaviour
    {
        
        #region Variables
        
        public static PlayerCamera Instance;

        [Header("Input")] 
        [SerializeField] private InputActionReference _inputMouseX;
        [SerializeField] private InputActionReference _inputMouseY;
        
        
        [Header("Camera")]
        [SerializeField] private Transform _cameraTransform;
        [Range(0f,10f)]
        [SerializeField] private float _speedRotation = 1f;
        [SerializeField] private float _maxAngle = 85f;
        
        
        
        private Vector2 _mousePosition;
        private float _mouseX;
        private float _mouseY;
        
        #endregion
        #region Unity Methods
        
        void Start()
        {
            Instance = this;
            _inputMouseX.action.performed += ctx => _mousePosition.x = ctx.ReadValue<float>();
            _inputMouseY.action.performed += ctx => _mousePosition.y = ctx.ReadValue<float>();
            
        }

        void Update()
        {
             GetMousePosition(_mousePosition);
            
            
        }
        #endregion


        #region public Methods
        #endregion

        #region private Methods

        private void GetMousePosition(Vector2 input)
        {
            Vector2 mousePos = Vector2.zero;

            mousePos.x = input.x;
            mousePos.y = input.y;
            
            Debug.Log(mousePos);
        }
        
        

        #endregion
    }
}
