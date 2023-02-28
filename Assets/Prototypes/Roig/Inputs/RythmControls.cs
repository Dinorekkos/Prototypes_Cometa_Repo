// GENERATED AUTOMATICALLY FROM 'Assets/Prototypes/Roig/Inputs/RythmControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @RythmControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @RythmControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""RythmControls"",
    ""maps"": [
        {
            ""name"": ""Normal"",
            ""id"": ""47af91b7-90c0-48d1-bda3-ee8782ab9a2c"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""PassThrough"",
                    ""id"": ""15a6021c-d3c8-42e6-97d8-a5a1780ba968"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""BlueAttack"",
                    ""type"": ""Button"",
                    ""id"": ""1a88fd86-5c86-496e-9f67-73ea429a1ebb"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""RedAttack"",
                    ""type"": ""Button"",
                    ""id"": ""df7a1c0e-3c4a-43ad-b0fc-0f57d0db09a6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""7f329bd3-6afc-417b-9c28-61147a28767a"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""9e706a74-5702-4139-b561-9b546343afb4"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""e73cd0fb-a71b-4262-8d81-611498581445"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""a381df49-79f5-4bc3-bc85-c80bcefdf329"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""89b52008-fd05-438d-a08a-17d3ae722be4"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""5ffcf7f4-419a-4043-94de-720a519b35f0"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""069e3c49-8168-4420-a536-465219fd21e6"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""BlueAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""24e0660e-7307-4d36-bc1b-c88a7d6ed25f"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""BlueAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fe67a3e5-a5f9-4e00-9653-5565e977b24d"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""RedAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d163915a-1676-4176-9f3c-a08a8cc26457"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""RedAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard"",
            ""bindingGroup"": ""Keyboard"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": true,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Controller"",
            ""bindingGroup"": ""Controller"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": true,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Normal
        m_Normal = asset.FindActionMap("Normal", throwIfNotFound: true);
        m_Normal_Movement = m_Normal.FindAction("Movement", throwIfNotFound: true);
        m_Normal_BlueAttack = m_Normal.FindAction("BlueAttack", throwIfNotFound: true);
        m_Normal_RedAttack = m_Normal.FindAction("RedAttack", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Normal
    private readonly InputActionMap m_Normal;
    private INormalActions m_NormalActionsCallbackInterface;
    private readonly InputAction m_Normal_Movement;
    private readonly InputAction m_Normal_BlueAttack;
    private readonly InputAction m_Normal_RedAttack;
    public struct NormalActions
    {
        private @RythmControls m_Wrapper;
        public NormalActions(@RythmControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_Normal_Movement;
        public InputAction @BlueAttack => m_Wrapper.m_Normal_BlueAttack;
        public InputAction @RedAttack => m_Wrapper.m_Normal_RedAttack;
        public InputActionMap Get() { return m_Wrapper.m_Normal; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(NormalActions set) { return set.Get(); }
        public void SetCallbacks(INormalActions instance)
        {
            if (m_Wrapper.m_NormalActionsCallbackInterface != null)
            {
                @Movement.started -= m_Wrapper.m_NormalActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_NormalActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_NormalActionsCallbackInterface.OnMovement;
                @BlueAttack.started -= m_Wrapper.m_NormalActionsCallbackInterface.OnBlueAttack;
                @BlueAttack.performed -= m_Wrapper.m_NormalActionsCallbackInterface.OnBlueAttack;
                @BlueAttack.canceled -= m_Wrapper.m_NormalActionsCallbackInterface.OnBlueAttack;
                @RedAttack.started -= m_Wrapper.m_NormalActionsCallbackInterface.OnRedAttack;
                @RedAttack.performed -= m_Wrapper.m_NormalActionsCallbackInterface.OnRedAttack;
                @RedAttack.canceled -= m_Wrapper.m_NormalActionsCallbackInterface.OnRedAttack;
            }
            m_Wrapper.m_NormalActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @BlueAttack.started += instance.OnBlueAttack;
                @BlueAttack.performed += instance.OnBlueAttack;
                @BlueAttack.canceled += instance.OnBlueAttack;
                @RedAttack.started += instance.OnRedAttack;
                @RedAttack.performed += instance.OnRedAttack;
                @RedAttack.canceled += instance.OnRedAttack;
            }
        }
    }
    public NormalActions @Normal => new NormalActions(this);
    private int m_KeyboardSchemeIndex = -1;
    public InputControlScheme KeyboardScheme
    {
        get
        {
            if (m_KeyboardSchemeIndex == -1) m_KeyboardSchemeIndex = asset.FindControlSchemeIndex("Keyboard");
            return asset.controlSchemes[m_KeyboardSchemeIndex];
        }
    }
    private int m_ControllerSchemeIndex = -1;
    public InputControlScheme ControllerScheme
    {
        get
        {
            if (m_ControllerSchemeIndex == -1) m_ControllerSchemeIndex = asset.FindControlSchemeIndex("Controller");
            return asset.controlSchemes[m_ControllerSchemeIndex];
        }
    }
    public interface INormalActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnBlueAttack(InputAction.CallbackContext context);
        void OnRedAttack(InputAction.CallbackContext context);
    }
}
