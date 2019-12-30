// GENERATED AUTOMATICALLY FROM 'Assets/MGM/Mouvement/Inputs/MouvementControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @MouvementControls : IInputActionCollection, IDisposable
{
    private InputActionAsset asset;
    public @MouvementControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""MouvementControls"",
    ""maps"": [
        {
            ""name"": ""Mouvement"",
            ""id"": ""f7d9e26f-c90a-427d-b480-5414dac15696"",
            ""actions"": [
                {
                    ""name"": ""MouvementDirection"",
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
                    ""expectedControlType"": ""Vector3"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Keyboard"",
                    ""id"": ""e374642f-da55-4cb1-bfc7-ac67eae8958f"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MouvementDirection"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""26bdefc6-413b-4b4e-8d65-ecb3a68b8fcb"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MouvementDirection"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""130571e0-41f6-4fe6-8a8d-55f67b63aec8"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MouvementDirection"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""e1429d93-6752-45b0-a430-b6b04a22fb04"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MouvementDirection"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""cffbbe28-4925-4210-8ac0-0325a629c0e2"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MouvementDirection"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Gamepad"",
                    ""id"": ""fdc8f012-0551-453a-bdfe-e74a84d6aa78"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MouvementDirection"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""a29b5328-898f-4609-a49e-4502999f0aa7"",
                    ""path"": ""<XInputController>/dpad/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MouvementDirection"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""81b44e2b-698c-4530-b075-16b0f2c4d4a0"",
                    ""path"": ""<XInputController>/dpad/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MouvementDirection"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""aed348e6-fdfb-4aa9-88c1-7bd75640895f"",
                    ""path"": ""<XInputController>/dpad/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MouvementDirection"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""b5609f16-2cae-4b39-9c4b-c8449e27d9f4"",
                    ""path"": ""<XInputController>/dpad/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MouvementDirection"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""a5e2c753-6642-4322-8d31-7bb4c937109e"",
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
                    ""id"": ""36226541-728e-49ae-b582-9c3a2ba208bd"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
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
                    ""groups"": """",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d31356f7-d474-4754-8bcd-7678af49f0bf"",
                    ""path"": ""<Pen>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""670dd5e4-4d7f-4e86-b9f5-c02194c3b903"",
                    ""path"": ""<Touchscreen>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Mouvement
        m_Mouvement = asset.FindActionMap("Mouvement", throwIfNotFound: true);
        m_Mouvement_MouvementDirection = m_Mouvement.FindAction("MouvementDirection", throwIfNotFound: true);
        m_Mouvement_Jump = m_Mouvement.FindAction("Jump", throwIfNotFound: true);
        m_Mouvement_Aim = m_Mouvement.FindAction("Aim", throwIfNotFound: true);
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

    // Mouvement
    private readonly InputActionMap m_Mouvement;
    private IMouvementActions m_MouvementActionsCallbackInterface;
    private readonly InputAction m_Mouvement_MouvementDirection;
    private readonly InputAction m_Mouvement_Jump;
    private readonly InputAction m_Mouvement_Aim;
    public struct MouvementActions
    {
        private @MouvementControls m_Wrapper;
        public MouvementActions(@MouvementControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @MouvementDirection => m_Wrapper.m_Mouvement_MouvementDirection;
        public InputAction @Jump => m_Wrapper.m_Mouvement_Jump;
        public InputAction @Aim => m_Wrapper.m_Mouvement_Aim;
        public InputActionMap Get() { return m_Wrapper.m_Mouvement; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MouvementActions set) { return set.Get(); }
        public void SetCallbacks(IMouvementActions instance)
        {
            if (m_Wrapper.m_MouvementActionsCallbackInterface != null)
            {
                @MouvementDirection.started -= m_Wrapper.m_MouvementActionsCallbackInterface.OnMouvementDirection;
                @MouvementDirection.performed -= m_Wrapper.m_MouvementActionsCallbackInterface.OnMouvementDirection;
                @MouvementDirection.canceled -= m_Wrapper.m_MouvementActionsCallbackInterface.OnMouvementDirection;
                @Jump.started -= m_Wrapper.m_MouvementActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_MouvementActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_MouvementActionsCallbackInterface.OnJump;
                @Aim.started -= m_Wrapper.m_MouvementActionsCallbackInterface.OnAim;
                @Aim.performed -= m_Wrapper.m_MouvementActionsCallbackInterface.OnAim;
                @Aim.canceled -= m_Wrapper.m_MouvementActionsCallbackInterface.OnAim;
            }
            m_Wrapper.m_MouvementActionsCallbackInterface = instance;
            if (instance != null)
            {
                @MouvementDirection.started += instance.OnMouvementDirection;
                @MouvementDirection.performed += instance.OnMouvementDirection;
                @MouvementDirection.canceled += instance.OnMouvementDirection;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Aim.started += instance.OnAim;
                @Aim.performed += instance.OnAim;
                @Aim.canceled += instance.OnAim;
            }
        }
    }
    public MouvementActions @Mouvement => new MouvementActions(this);
    public interface IMouvementActions
    {
        void OnMouvementDirection(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnAim(InputAction.CallbackContext context);
    }
}
