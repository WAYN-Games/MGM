// GENERATED AUTOMATICALLY FROM 'Assets/MGM/TankDemo/Scripts/Inputs/TankDemotControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @TankDemotControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @TankDemotControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""TankDemotControls"",
    ""maps"": [
        {
            ""name"": ""Tank"",
            ""id"": ""f7d9e26f-c90a-427d-b480-5414dac15696"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""PassThrough"",
                    ""id"": ""33c5b070-a728-4b2b-acf6-41e84745aa61"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""PassThrough"",
                    ""id"": ""aec4de21-eb04-41df-923f-9efb3c1f5c59"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Aim"",
                    ""type"": ""PassThrough"",
                    ""id"": ""d9e083d0-1c0c-4bbf-bb77-20ff6151fd6e"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Shoot"",
                    ""type"": ""PassThrough"",
                    ""id"": ""00af218f-a2e1-452f-b5dd-fa417495c93d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ToogleGraphy"",
                    ""type"": ""Button"",
                    ""id"": ""74c44634-f362-46d9-8883-e461fbc71d2b"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Keyboard"",
                    ""id"": ""e374642f-da55-4cb1-bfc7-ac67eae8958f"",
                    ""path"": ""2DVector(normalize=false)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""26bdefc6-413b-4b4e-8d65-ecb3a68b8fcb"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""130571e0-41f6-4fe6-8a8d-55f67b63aec8"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""e1429d93-6752-45b0-a430-b6b04a22fb04"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""cffbbe28-4925-4210-8ac0-0325a629c0e2"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""6e796870-68cd-40da-8f21-d90f1bd517ae"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a5e2c753-6642-4322-8d31-7bb4c937109e"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""36226541-728e-49ae-b582-9c3a2ba208bd"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2c63ec01-0ab7-47e2-b12f-d2053e88c3bc"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""aa41c3e4-3c49-4d3e-9ee3-f8c627b4662a"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": ""ToPointerPosition"",
                    ""groups"": ""GamePad"",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9c285fe1-7fc3-4836-b98d-db11028d619b"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d245075d-c0bb-43c1-88cd-50d3e9fc10de"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad"",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8af4a17f-dd9f-4a8d-b665-707a0226a29e"",
                    ""path"": ""<Keyboard>/g"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""ToogleGraphy"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2c8d0f94-6e06-458b-ad38-b75115903382"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GamePad"",
                    ""action"": ""ToogleGraphy"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""GamePad"",
            ""bindingGroup"": ""GamePad"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""PC"",
            ""bindingGroup"": ""PC"",
            ""devices"": [
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Tank
        m_Tank = asset.FindActionMap("Tank", throwIfNotFound: true);
        m_Tank_Move = m_Tank.FindAction("Move", throwIfNotFound: true);
        m_Tank_Jump = m_Tank.FindAction("Jump", throwIfNotFound: true);
        m_Tank_Aim = m_Tank.FindAction("Aim", throwIfNotFound: true);
        m_Tank_Shoot = m_Tank.FindAction("Shoot", throwIfNotFound: true);
        m_Tank_ToogleGraphy = m_Tank.FindAction("ToogleGraphy", throwIfNotFound: true);
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

    // Tank
    private readonly InputActionMap m_Tank;
    private ITankActions m_TankActionsCallbackInterface;
    private readonly InputAction m_Tank_Move;
    private readonly InputAction m_Tank_Jump;
    private readonly InputAction m_Tank_Aim;
    private readonly InputAction m_Tank_Shoot;
    private readonly InputAction m_Tank_ToogleGraphy;
    public struct TankActions
    {
        private @TankDemotControls m_Wrapper;
        public TankActions(@TankDemotControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Tank_Move;
        public InputAction @Jump => m_Wrapper.m_Tank_Jump;
        public InputAction @Aim => m_Wrapper.m_Tank_Aim;
        public InputAction @Shoot => m_Wrapper.m_Tank_Shoot;
        public InputAction @ToogleGraphy => m_Wrapper.m_Tank_ToogleGraphy;
        public InputActionMap Get() { return m_Wrapper.m_Tank; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(TankActions set) { return set.Get(); }
        public void SetCallbacks(ITankActions instance)
        {
            if (m_Wrapper.m_TankActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_TankActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_TankActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_TankActionsCallbackInterface.OnMove;
                @Jump.started -= m_Wrapper.m_TankActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_TankActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_TankActionsCallbackInterface.OnJump;
                @Aim.started -= m_Wrapper.m_TankActionsCallbackInterface.OnAim;
                @Aim.performed -= m_Wrapper.m_TankActionsCallbackInterface.OnAim;
                @Aim.canceled -= m_Wrapper.m_TankActionsCallbackInterface.OnAim;
                @Shoot.started -= m_Wrapper.m_TankActionsCallbackInterface.OnShoot;
                @Shoot.performed -= m_Wrapper.m_TankActionsCallbackInterface.OnShoot;
                @Shoot.canceled -= m_Wrapper.m_TankActionsCallbackInterface.OnShoot;
                @ToogleGraphy.started -= m_Wrapper.m_TankActionsCallbackInterface.OnToogleGraphy;
                @ToogleGraphy.performed -= m_Wrapper.m_TankActionsCallbackInterface.OnToogleGraphy;
                @ToogleGraphy.canceled -= m_Wrapper.m_TankActionsCallbackInterface.OnToogleGraphy;
            }
            m_Wrapper.m_TankActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Aim.started += instance.OnAim;
                @Aim.performed += instance.OnAim;
                @Aim.canceled += instance.OnAim;
                @Shoot.started += instance.OnShoot;
                @Shoot.performed += instance.OnShoot;
                @Shoot.canceled += instance.OnShoot;
                @ToogleGraphy.started += instance.OnToogleGraphy;
                @ToogleGraphy.performed += instance.OnToogleGraphy;
                @ToogleGraphy.canceled += instance.OnToogleGraphy;
            }
        }
    }
    public TankActions @Tank => new TankActions(this);
    private int m_GamePadSchemeIndex = -1;
    public InputControlScheme GamePadScheme
    {
        get
        {
            if (m_GamePadSchemeIndex == -1) m_GamePadSchemeIndex = asset.FindControlSchemeIndex("GamePad");
            return asset.controlSchemes[m_GamePadSchemeIndex];
        }
    }
    private int m_PCSchemeIndex = -1;
    public InputControlScheme PCScheme
    {
        get
        {
            if (m_PCSchemeIndex == -1) m_PCSchemeIndex = asset.FindControlSchemeIndex("PC");
            return asset.controlSchemes[m_PCSchemeIndex];
        }
    }
    public interface ITankActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnAim(InputAction.CallbackContext context);
        void OnShoot(InputAction.CallbackContext context);
        void OnToogleGraphy(InputAction.CallbackContext context);
    }
}
