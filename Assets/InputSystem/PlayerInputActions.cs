//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/InputSystem/PlayerInputActions.inputactions
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
            ""name"": ""Player"",
            ""id"": ""9316c947-06d0-42e6-8609-7a7c5430e1d9"",
            ""actions"": [
                {
                    ""name"": ""Lean"",
                    ""type"": ""Value"",
                    ""id"": ""d9c05411-6750-4fbb-8874-b338b3ec9880"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""Value"",
                    ""id"": ""ee8f3c52-fdee-46b5-b443-07c54d6d3178"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Charge Jump"",
                    ""type"": ""Button"",
                    ""id"": ""9b2cf083-f708-4368-8151-d2a68d845959"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Restart"",
                    ""type"": ""Button"",
                    ""id"": ""8a97e615-11ae-4b42-bf75-8052fd7a32e9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""bedb29ee-a64a-4b7d-80b6-b54333c1a560"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Navigate"",
                    ""type"": ""Value"",
                    ""id"": ""211e7e2a-db43-416f-95be-9695ab218683"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Select"",
                    ""type"": ""Button"",
                    ""id"": ""f6c3a713-c475-45a4-89e8-0da56a985de9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Respawn"",
                    ""type"": ""Button"",
                    ""id"": ""a999e88e-d347-4118-bf7a-c15834863e21"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Controller"",
                    ""id"": ""af9cfa3f-6fcc-461f-b759-5badd1e10606"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Lean"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""6b59f0cc-94ca-4ea3-b130-e4ce0b3a856e"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Lean"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""bd4f7653-dedb-46cb-9419-281f84462c0f"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Lean"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""f200492d-b93c-4db7-93cf-06a61a816e19"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Lean"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""58f8e376-e6f7-4c92-a876-3effbdd8cb00"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Lean"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""WASD"",
                    ""id"": ""c7b53796-604f-4e2d-aeca-1f23737e8f81"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Lean"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""c311a4a5-e533-467b-a3bb-fe53034d9c32"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Lean"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""a40fa3ec-6096-41bd-8241-3bba4b96978f"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Lean"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""08af95b6-3fbf-45af-9134-be185a727e5a"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Lean"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""d1a89c3f-be8b-40c1-af4f-df83efc95f97"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Lean"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Arrow Keys"",
                    ""id"": ""20da69e6-b3da-4adb-9da0-7ff52b240ed4"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Lean"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""75a86d9e-c4dd-46fe-b2b5-b93d06c3f5fd"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Lean"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""adceea72-c190-4096-8895-3cea4072c550"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Lean"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""154c76f9-6149-4de1-9a26-072096a61497"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Lean"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""89ecf8b5-7262-45e2-a2eb-d682ac7babe7"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Lean"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""bddca0b4-151d-484e-b6f6-37063d164c1b"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": ""ScaleVector2(x=0.1,y=0.1)"",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""89dc450e-f0cd-42e7-82b2-59d355c7784b"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9c5ececb-2882-4238-bed1-44e56bfff734"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Charge Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a87d2fa0-ee40-41b9-a0ec-5bb694282082"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Charge Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f6ec8379-ae8f-4d9d-88dc-eda0af4dc17e"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Restart"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e27c11d4-b325-409c-92cc-744e97984687"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Restart"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0806b052-3d4e-4437-8c8e-a8358bb8e85e"",
                    ""path"": ""<Keyboard>/p"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b09216c1-b23a-4ace-b8af-5143cfeb665d"",
                    ""path"": ""<DualShockGamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""bd174fa4-fced-4c42-a98e-f5641fb95a36"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""018a07bf-b084-4e23-ba9d-2a2da0e9ffd8"",
                    ""path"": ""<Gamepad>/dpad/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""3a9da86e-114d-4639-b140-8c3447c1877b"",
                    ""path"": ""<Gamepad>/dpad/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""7ca0bb7e-b26a-4e42-8452-1728b3840a7c"",
                    ""path"": ""<Gamepad>/dpad/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""bd493743-d466-4dc2-a73c-a757ade84b7b"",
                    ""path"": ""<Gamepad>/dpad/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""93336c32-918b-4a6d-b86e-cd3a25d048aa"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Select"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""388eac72-e921-43f5-88ad-ba0632a791ab"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Respawn"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""75c18e0c-4c65-414c-85de-78594d50b35d"",
                    ""path"": ""<Keyboard>/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Respawn"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_Lean = m_Player.FindAction("Lean", throwIfNotFound: true);
        m_Player_Look = m_Player.FindAction("Look", throwIfNotFound: true);
        m_Player_ChargeJump = m_Player.FindAction("Charge Jump", throwIfNotFound: true);
        m_Player_Restart = m_Player.FindAction("Restart", throwIfNotFound: true);
        m_Player_Pause = m_Player.FindAction("Pause", throwIfNotFound: true);
        m_Player_Navigate = m_Player.FindAction("Navigate", throwIfNotFound: true);
        m_Player_Select = m_Player.FindAction("Select", throwIfNotFound: true);
        m_Player_Respawn = m_Player.FindAction("Respawn", throwIfNotFound: true);
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

    // Player
    private readonly InputActionMap m_Player;
    private List<IPlayerActions> m_PlayerActionsCallbackInterfaces = new List<IPlayerActions>();
    private readonly InputAction m_Player_Lean;
    private readonly InputAction m_Player_Look;
    private readonly InputAction m_Player_ChargeJump;
    private readonly InputAction m_Player_Restart;
    private readonly InputAction m_Player_Pause;
    private readonly InputAction m_Player_Navigate;
    private readonly InputAction m_Player_Select;
    private readonly InputAction m_Player_Respawn;
    public struct PlayerActions
    {
        private @PlayerInputActions m_Wrapper;
        public PlayerActions(@PlayerInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Lean => m_Wrapper.m_Player_Lean;
        public InputAction @Look => m_Wrapper.m_Player_Look;
        public InputAction @ChargeJump => m_Wrapper.m_Player_ChargeJump;
        public InputAction @Restart => m_Wrapper.m_Player_Restart;
        public InputAction @Pause => m_Wrapper.m_Player_Pause;
        public InputAction @Navigate => m_Wrapper.m_Player_Navigate;
        public InputAction @Select => m_Wrapper.m_Player_Select;
        public InputAction @Respawn => m_Wrapper.m_Player_Respawn;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void AddCallbacks(IPlayerActions instance)
        {
            if (instance == null || m_Wrapper.m_PlayerActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_PlayerActionsCallbackInterfaces.Add(instance);
            @Lean.started += instance.OnLean;
            @Lean.performed += instance.OnLean;
            @Lean.canceled += instance.OnLean;
            @Look.started += instance.OnLook;
            @Look.performed += instance.OnLook;
            @Look.canceled += instance.OnLook;
            @ChargeJump.started += instance.OnChargeJump;
            @ChargeJump.performed += instance.OnChargeJump;
            @ChargeJump.canceled += instance.OnChargeJump;
            @Restart.started += instance.OnRestart;
            @Restart.performed += instance.OnRestart;
            @Restart.canceled += instance.OnRestart;
            @Pause.started += instance.OnPause;
            @Pause.performed += instance.OnPause;
            @Pause.canceled += instance.OnPause;
            @Navigate.started += instance.OnNavigate;
            @Navigate.performed += instance.OnNavigate;
            @Navigate.canceled += instance.OnNavigate;
            @Select.started += instance.OnSelect;
            @Select.performed += instance.OnSelect;
            @Select.canceled += instance.OnSelect;
            @Respawn.started += instance.OnRespawn;
            @Respawn.performed += instance.OnRespawn;
            @Respawn.canceled += instance.OnRespawn;
        }

        private void UnregisterCallbacks(IPlayerActions instance)
        {
            @Lean.started -= instance.OnLean;
            @Lean.performed -= instance.OnLean;
            @Lean.canceled -= instance.OnLean;
            @Look.started -= instance.OnLook;
            @Look.performed -= instance.OnLook;
            @Look.canceled -= instance.OnLook;
            @ChargeJump.started -= instance.OnChargeJump;
            @ChargeJump.performed -= instance.OnChargeJump;
            @ChargeJump.canceled -= instance.OnChargeJump;
            @Restart.started -= instance.OnRestart;
            @Restart.performed -= instance.OnRestart;
            @Restart.canceled -= instance.OnRestart;
            @Pause.started -= instance.OnPause;
            @Pause.performed -= instance.OnPause;
            @Pause.canceled -= instance.OnPause;
            @Navigate.started -= instance.OnNavigate;
            @Navigate.performed -= instance.OnNavigate;
            @Navigate.canceled -= instance.OnNavigate;
            @Select.started -= instance.OnSelect;
            @Select.performed -= instance.OnSelect;
            @Select.canceled -= instance.OnSelect;
            @Respawn.started -= instance.OnRespawn;
            @Respawn.performed -= instance.OnRespawn;
            @Respawn.canceled -= instance.OnRespawn;
        }

        public void RemoveCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IPlayerActions instance)
        {
            foreach (var item in m_Wrapper.m_PlayerActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_PlayerActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public PlayerActions @Player => new PlayerActions(this);
    public interface IPlayerActions
    {
        void OnLean(InputAction.CallbackContext context);
        void OnLook(InputAction.CallbackContext context);
        void OnChargeJump(InputAction.CallbackContext context);
        void OnRestart(InputAction.CallbackContext context);
        void OnPause(InputAction.CallbackContext context);
        void OnNavigate(InputAction.CallbackContext context);
        void OnSelect(InputAction.CallbackContext context);
        void OnRespawn(InputAction.CallbackContext context);
    }
}
