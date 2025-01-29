//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.11.2
//     from Assets/Scripts/Player/PlayerInputActions.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @PlayerInputActions: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInputActions"",
    ""maps"": [
        {
            ""name"": ""WeaponControl"",
            ""id"": ""83afe50f-f7e6-45d9-b36f-907bc3269dae"",
            ""actions"": [
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""a75e9419-18b6-4e48-8f6d-ef2fe22aaa1e"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": ""Tap"",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Tilt"",
                    ""type"": ""PassThrough"",
                    ""id"": ""99f6d6c8-7262-4676-97bc-b57aed7cb108"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""CameraTilt"",
                    ""type"": ""PassThrough"",
                    ""id"": ""14abacb5-1552-40fe-8566-fbdae43bb048"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""CameraLock"",
                    ""type"": ""Button"",
                    ""id"": ""0614ed4a-3886-401a-b886-1ddbd3846d4f"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""CameraSwap"",
                    ""type"": ""Button"",
                    ""id"": ""20ac30b4-e1cd-4e44-be37-9bbc76bf2097"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=1)"",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""5953af2c-3a56-4371-99c0-7c548582b5c8"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ed20cae4-5dfe-4579-8536-69739035d1ab"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""da0366e1-0816-4e0b-9e06-3052741febe6"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Tilt"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""366166db-784b-433b-8f8f-add9cf7dd4f2"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Tilt"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ad8f0149-12bc-474e-969e-12d262a55da7"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraTilt"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0158aaa4-e9ff-4928-af59-c5d4f87ac2e5"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraTilt"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""26eecfc8-66d8-46f3-bb60-2cc0315debe8"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": ""Hold"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraLock"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5c848099-682d-474b-81a8-3dc75fd8caf8"",
                    ""path"": ""<Gamepad>/leftStickPress"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraLock"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""32eb52fe-8fa3-48cf-8531-39be32d9c0a9"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraSwap"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // WeaponControl
        m_WeaponControl = asset.FindActionMap("WeaponControl", throwIfNotFound: true);
        m_WeaponControl_Jump = m_WeaponControl.FindAction("Jump", throwIfNotFound: true);
        m_WeaponControl_Tilt = m_WeaponControl.FindAction("Tilt", throwIfNotFound: true);
        m_WeaponControl_CameraTilt = m_WeaponControl.FindAction("CameraTilt", throwIfNotFound: true);
        m_WeaponControl_CameraLock = m_WeaponControl.FindAction("CameraLock", throwIfNotFound: true);
        m_WeaponControl_CameraSwap = m_WeaponControl.FindAction("CameraSwap", throwIfNotFound: true);
    }

    ~@PlayerInputActions()
    {
        UnityEngine.Debug.Assert(!m_WeaponControl.enabled, "This will cause a leak and performance issues, PlayerInputActions.WeaponControl.Disable() has not been called.");
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

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // WeaponControl
    private readonly InputActionMap m_WeaponControl;
    private List<IWeaponControlActions> m_WeaponControlActionsCallbackInterfaces = new List<IWeaponControlActions>();
    private readonly InputAction m_WeaponControl_Jump;
    private readonly InputAction m_WeaponControl_Tilt;
    private readonly InputAction m_WeaponControl_CameraTilt;
    private readonly InputAction m_WeaponControl_CameraLock;
    private readonly InputAction m_WeaponControl_CameraSwap;
    public struct WeaponControlActions
    {
        private @PlayerInputActions m_Wrapper;
        public WeaponControlActions(@PlayerInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Jump => m_Wrapper.m_WeaponControl_Jump;
        public InputAction @Tilt => m_Wrapper.m_WeaponControl_Tilt;
        public InputAction @CameraTilt => m_Wrapper.m_WeaponControl_CameraTilt;
        public InputAction @CameraLock => m_Wrapper.m_WeaponControl_CameraLock;
        public InputAction @CameraSwap => m_Wrapper.m_WeaponControl_CameraSwap;
        public InputActionMap Get() { return m_Wrapper.m_WeaponControl; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(WeaponControlActions set) { return set.Get(); }
        public void AddCallbacks(IWeaponControlActions instance)
        {
            if (instance == null || m_Wrapper.m_WeaponControlActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_WeaponControlActionsCallbackInterfaces.Add(instance);
            @Jump.started += instance.OnJump;
            @Jump.performed += instance.OnJump;
            @Jump.canceled += instance.OnJump;
            @Tilt.started += instance.OnTilt;
            @Tilt.performed += instance.OnTilt;
            @Tilt.canceled += instance.OnTilt;
            @CameraTilt.started += instance.OnCameraTilt;
            @CameraTilt.performed += instance.OnCameraTilt;
            @CameraTilt.canceled += instance.OnCameraTilt;
            @CameraLock.started += instance.OnCameraLock;
            @CameraLock.performed += instance.OnCameraLock;
            @CameraLock.canceled += instance.OnCameraLock;
            @CameraSwap.started += instance.OnCameraSwap;
            @CameraSwap.performed += instance.OnCameraSwap;
            @CameraSwap.canceled += instance.OnCameraSwap;
        }

        private void UnregisterCallbacks(IWeaponControlActions instance)
        {
            @Jump.started -= instance.OnJump;
            @Jump.performed -= instance.OnJump;
            @Jump.canceled -= instance.OnJump;
            @Tilt.started -= instance.OnTilt;
            @Tilt.performed -= instance.OnTilt;
            @Tilt.canceled -= instance.OnTilt;
            @CameraTilt.started -= instance.OnCameraTilt;
            @CameraTilt.performed -= instance.OnCameraTilt;
            @CameraTilt.canceled -= instance.OnCameraTilt;
            @CameraLock.started -= instance.OnCameraLock;
            @CameraLock.performed -= instance.OnCameraLock;
            @CameraLock.canceled -= instance.OnCameraLock;
            @CameraSwap.started -= instance.OnCameraSwap;
            @CameraSwap.performed -= instance.OnCameraSwap;
            @CameraSwap.canceled -= instance.OnCameraSwap;
        }

        public void RemoveCallbacks(IWeaponControlActions instance)
        {
            if (m_Wrapper.m_WeaponControlActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IWeaponControlActions instance)
        {
            foreach (var item in m_Wrapper.m_WeaponControlActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_WeaponControlActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public WeaponControlActions @WeaponControl => new WeaponControlActions(this);
    public interface IWeaponControlActions
    {
        void OnJump(InputAction.CallbackContext context);
        void OnTilt(InputAction.CallbackContext context);
        void OnCameraTilt(InputAction.CallbackContext context);
        void OnCameraLock(InputAction.CallbackContext context);
        void OnCameraSwap(InputAction.CallbackContext context);
    }
}
