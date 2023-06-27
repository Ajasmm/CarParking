//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.6.1
//     from Assets/Script/Input/MyInput.inputactions
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

public partial class @MyInput: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @MyInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""MyInput"",
    ""maps"": [
        {
            ""name"": ""GamePlay"",
            ""id"": ""c70611e7-2ed3-4c7c-ae56-e42f4e0f5a36"",
            ""actions"": [
                {
                    ""name"": ""Steering"",
                    ""type"": ""Value"",
                    ""id"": ""69a11798-3e75-47e9-9fc6-6038e2946bcc"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Acceleration"",
                    ""type"": ""Button"",
                    ""id"": ""7ccd0e80-ae45-4cc7-b57a-3e795879f43b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Brake"",
                    ""type"": ""Button"",
                    ""id"": ""8c86ffe7-9d8f-4fd0-a06a-3ba9785e36d0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""HandBrake"",
                    ""type"": ""Button"",
                    ""id"": ""edbbab4b-e505-4940-a91e-0e9e737f9f72"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Camera"",
                    ""type"": ""Button"",
                    ""id"": ""20bc074d-159d-4368-9dd6-3db436dcb784"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Drive"",
                    ""type"": ""Button"",
                    ""id"": ""793b757a-2f66-40fd-9d38-5e5a6518d1ae"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Nuteral"",
                    ""type"": ""Button"",
                    ""id"": ""d20e4374-3b33-4b7d-9ead-9186ae5e97bc"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Reverce"",
                    ""type"": ""Button"",
                    ""id"": ""36b2acc4-686b-4e5e-a956-40add1e521ec"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Escape"",
                    ""type"": ""Button"",
                    ""id"": ""04301da0-d8be-4174-84de-ced58f7b3091"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""AD"",
                    ""id"": ""9bb25596-7e25-44c0-a3c2-20cfe49c9384"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Steering"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""a76a100f-6a10-4576-b138-a5fdc56bbc93"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Steering"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""c87ec19e-d8bb-422b-9094-70f65230a8c4"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Steering"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Arrow"",
                    ""id"": ""7ec35219-da50-4f8d-9bf3-99f323b4ff91"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Steering"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""43fe20b1-2329-4abb-89d6-00f7e4b7f80b"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Steering"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""dc0dabf9-c9b9-4765-8e68-a24198487348"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Steering"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""03dc729c-85b0-43d8-8764-942ac8a15c9c"",
                    ""path"": ""<Gamepad>/rightStick/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Steering"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e0cf935f-7ca9-4412-a46b-545bfb10d3da"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Acceleration"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ca6fe875-4f77-4992-a0a6-4ba9a18da97e"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Acceleration"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bb8f2a34-626c-4b1c-82f6-d60e6595a3c7"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Brake"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""091d2cc0-7756-4f12-b3d7-3fb99a92a82b"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Brake"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e33a14fb-59b2-4b59-8260-4c5290bd1290"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HandBrake"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2c19bbcb-519c-42c1-9e8f-d2f8ad6e7d01"",
                    ""path"": ""<Keyboard>/c"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Camera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""37288eda-f4fb-43fa-9c2e-9a6096f534e5"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Drive"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9908b8a4-f255-4545-9b68-ff63c1985acc"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Nuteral"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2088ff18-5966-453e-a8da-18cdd8ddfcd9"",
                    ""path"": ""<Keyboard>/v"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Reverce"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""09e62143-06b1-40a9-b769-9570474b6d51"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Escape"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Menu"",
            ""id"": ""0d62eb84-7250-4268-b998-5e44ccac542f"",
            ""actions"": [
                {
                    ""name"": ""Escape"",
                    ""type"": ""Button"",
                    ""id"": ""5a08deb0-1478-4687-80f7-f746ed17b3a5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""2361c4db-279b-4aa0-b1e9-c9144b7ba214"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Escape"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""TimeLine"",
            ""id"": ""0d32adf1-5dfc-4fe8-80a7-831cb2b11a2f"",
            ""actions"": [
                {
                    ""name"": ""Skip"",
                    ""type"": ""Button"",
                    ""id"": ""5584d711-0d1d-416d-9c07-4e7ed1a6c163"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""2d351ba7-e023-4c1a-9878-1279c45a9529"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Skip"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // GamePlay
        m_GamePlay = asset.FindActionMap("GamePlay", throwIfNotFound: true);
        m_GamePlay_Steering = m_GamePlay.FindAction("Steering", throwIfNotFound: true);
        m_GamePlay_Acceleration = m_GamePlay.FindAction("Acceleration", throwIfNotFound: true);
        m_GamePlay_Brake = m_GamePlay.FindAction("Brake", throwIfNotFound: true);
        m_GamePlay_HandBrake = m_GamePlay.FindAction("HandBrake", throwIfNotFound: true);
        m_GamePlay_Camera = m_GamePlay.FindAction("Camera", throwIfNotFound: true);
        m_GamePlay_Drive = m_GamePlay.FindAction("Drive", throwIfNotFound: true);
        m_GamePlay_Nuteral = m_GamePlay.FindAction("Nuteral", throwIfNotFound: true);
        m_GamePlay_Reverce = m_GamePlay.FindAction("Reverce", throwIfNotFound: true);
        m_GamePlay_Escape = m_GamePlay.FindAction("Escape", throwIfNotFound: true);
        // Menu
        m_Menu = asset.FindActionMap("Menu", throwIfNotFound: true);
        m_Menu_Escape = m_Menu.FindAction("Escape", throwIfNotFound: true);
        // TimeLine
        m_TimeLine = asset.FindActionMap("TimeLine", throwIfNotFound: true);
        m_TimeLine_Skip = m_TimeLine.FindAction("Skip", throwIfNotFound: true);
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

    // GamePlay
    private readonly InputActionMap m_GamePlay;
    private List<IGamePlayActions> m_GamePlayActionsCallbackInterfaces = new List<IGamePlayActions>();
    private readonly InputAction m_GamePlay_Steering;
    private readonly InputAction m_GamePlay_Acceleration;
    private readonly InputAction m_GamePlay_Brake;
    private readonly InputAction m_GamePlay_HandBrake;
    private readonly InputAction m_GamePlay_Camera;
    private readonly InputAction m_GamePlay_Drive;
    private readonly InputAction m_GamePlay_Nuteral;
    private readonly InputAction m_GamePlay_Reverce;
    private readonly InputAction m_GamePlay_Escape;
    public struct GamePlayActions
    {
        private @MyInput m_Wrapper;
        public GamePlayActions(@MyInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Steering => m_Wrapper.m_GamePlay_Steering;
        public InputAction @Acceleration => m_Wrapper.m_GamePlay_Acceleration;
        public InputAction @Brake => m_Wrapper.m_GamePlay_Brake;
        public InputAction @HandBrake => m_Wrapper.m_GamePlay_HandBrake;
        public InputAction @Camera => m_Wrapper.m_GamePlay_Camera;
        public InputAction @Drive => m_Wrapper.m_GamePlay_Drive;
        public InputAction @Nuteral => m_Wrapper.m_GamePlay_Nuteral;
        public InputAction @Reverce => m_Wrapper.m_GamePlay_Reverce;
        public InputAction @Escape => m_Wrapper.m_GamePlay_Escape;
        public InputActionMap Get() { return m_Wrapper.m_GamePlay; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GamePlayActions set) { return set.Get(); }
        public void AddCallbacks(IGamePlayActions instance)
        {
            if (instance == null || m_Wrapper.m_GamePlayActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_GamePlayActionsCallbackInterfaces.Add(instance);
            @Steering.started += instance.OnSteering;
            @Steering.performed += instance.OnSteering;
            @Steering.canceled += instance.OnSteering;
            @Acceleration.started += instance.OnAcceleration;
            @Acceleration.performed += instance.OnAcceleration;
            @Acceleration.canceled += instance.OnAcceleration;
            @Brake.started += instance.OnBrake;
            @Brake.performed += instance.OnBrake;
            @Brake.canceled += instance.OnBrake;
            @HandBrake.started += instance.OnHandBrake;
            @HandBrake.performed += instance.OnHandBrake;
            @HandBrake.canceled += instance.OnHandBrake;
            @Camera.started += instance.OnCamera;
            @Camera.performed += instance.OnCamera;
            @Camera.canceled += instance.OnCamera;
            @Drive.started += instance.OnDrive;
            @Drive.performed += instance.OnDrive;
            @Drive.canceled += instance.OnDrive;
            @Nuteral.started += instance.OnNuteral;
            @Nuteral.performed += instance.OnNuteral;
            @Nuteral.canceled += instance.OnNuteral;
            @Reverce.started += instance.OnReverce;
            @Reverce.performed += instance.OnReverce;
            @Reverce.canceled += instance.OnReverce;
            @Escape.started += instance.OnEscape;
            @Escape.performed += instance.OnEscape;
            @Escape.canceled += instance.OnEscape;
        }

        private void UnregisterCallbacks(IGamePlayActions instance)
        {
            @Steering.started -= instance.OnSteering;
            @Steering.performed -= instance.OnSteering;
            @Steering.canceled -= instance.OnSteering;
            @Acceleration.started -= instance.OnAcceleration;
            @Acceleration.performed -= instance.OnAcceleration;
            @Acceleration.canceled -= instance.OnAcceleration;
            @Brake.started -= instance.OnBrake;
            @Brake.performed -= instance.OnBrake;
            @Brake.canceled -= instance.OnBrake;
            @HandBrake.started -= instance.OnHandBrake;
            @HandBrake.performed -= instance.OnHandBrake;
            @HandBrake.canceled -= instance.OnHandBrake;
            @Camera.started -= instance.OnCamera;
            @Camera.performed -= instance.OnCamera;
            @Camera.canceled -= instance.OnCamera;
            @Drive.started -= instance.OnDrive;
            @Drive.performed -= instance.OnDrive;
            @Drive.canceled -= instance.OnDrive;
            @Nuteral.started -= instance.OnNuteral;
            @Nuteral.performed -= instance.OnNuteral;
            @Nuteral.canceled -= instance.OnNuteral;
            @Reverce.started -= instance.OnReverce;
            @Reverce.performed -= instance.OnReverce;
            @Reverce.canceled -= instance.OnReverce;
            @Escape.started -= instance.OnEscape;
            @Escape.performed -= instance.OnEscape;
            @Escape.canceled -= instance.OnEscape;
        }

        public void RemoveCallbacks(IGamePlayActions instance)
        {
            if (m_Wrapper.m_GamePlayActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IGamePlayActions instance)
        {
            foreach (var item in m_Wrapper.m_GamePlayActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_GamePlayActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public GamePlayActions @GamePlay => new GamePlayActions(this);

    // Menu
    private readonly InputActionMap m_Menu;
    private List<IMenuActions> m_MenuActionsCallbackInterfaces = new List<IMenuActions>();
    private readonly InputAction m_Menu_Escape;
    public struct MenuActions
    {
        private @MyInput m_Wrapper;
        public MenuActions(@MyInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Escape => m_Wrapper.m_Menu_Escape;
        public InputActionMap Get() { return m_Wrapper.m_Menu; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MenuActions set) { return set.Get(); }
        public void AddCallbacks(IMenuActions instance)
        {
            if (instance == null || m_Wrapper.m_MenuActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_MenuActionsCallbackInterfaces.Add(instance);
            @Escape.started += instance.OnEscape;
            @Escape.performed += instance.OnEscape;
            @Escape.canceled += instance.OnEscape;
        }

        private void UnregisterCallbacks(IMenuActions instance)
        {
            @Escape.started -= instance.OnEscape;
            @Escape.performed -= instance.OnEscape;
            @Escape.canceled -= instance.OnEscape;
        }

        public void RemoveCallbacks(IMenuActions instance)
        {
            if (m_Wrapper.m_MenuActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IMenuActions instance)
        {
            foreach (var item in m_Wrapper.m_MenuActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_MenuActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public MenuActions @Menu => new MenuActions(this);

    // TimeLine
    private readonly InputActionMap m_TimeLine;
    private List<ITimeLineActions> m_TimeLineActionsCallbackInterfaces = new List<ITimeLineActions>();
    private readonly InputAction m_TimeLine_Skip;
    public struct TimeLineActions
    {
        private @MyInput m_Wrapper;
        public TimeLineActions(@MyInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Skip => m_Wrapper.m_TimeLine_Skip;
        public InputActionMap Get() { return m_Wrapper.m_TimeLine; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(TimeLineActions set) { return set.Get(); }
        public void AddCallbacks(ITimeLineActions instance)
        {
            if (instance == null || m_Wrapper.m_TimeLineActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_TimeLineActionsCallbackInterfaces.Add(instance);
            @Skip.started += instance.OnSkip;
            @Skip.performed += instance.OnSkip;
            @Skip.canceled += instance.OnSkip;
        }

        private void UnregisterCallbacks(ITimeLineActions instance)
        {
            @Skip.started -= instance.OnSkip;
            @Skip.performed -= instance.OnSkip;
            @Skip.canceled -= instance.OnSkip;
        }

        public void RemoveCallbacks(ITimeLineActions instance)
        {
            if (m_Wrapper.m_TimeLineActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(ITimeLineActions instance)
        {
            foreach (var item in m_Wrapper.m_TimeLineActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_TimeLineActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public TimeLineActions @TimeLine => new TimeLineActions(this);
    public interface IGamePlayActions
    {
        void OnSteering(InputAction.CallbackContext context);
        void OnAcceleration(InputAction.CallbackContext context);
        void OnBrake(InputAction.CallbackContext context);
        void OnHandBrake(InputAction.CallbackContext context);
        void OnCamera(InputAction.CallbackContext context);
        void OnDrive(InputAction.CallbackContext context);
        void OnNuteral(InputAction.CallbackContext context);
        void OnReverce(InputAction.CallbackContext context);
        void OnEscape(InputAction.CallbackContext context);
    }
    public interface IMenuActions
    {
        void OnEscape(InputAction.CallbackContext context);
    }
    public interface ITimeLineActions
    {
        void OnSkip(InputAction.CallbackContext context);
    }
}
