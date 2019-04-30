// GENERATED AUTOMATICALLY FROM 'Assets/MGM/Demo/PONG/Configs/Pong.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Input;
using UnityEngine.Experimental.Input.Utilities;

namespace Pong
{
    public class Pong : IInputActionCollection
    {
        private InputActionAsset asset;
        public Pong()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""Pong"",
    ""maps"": [
        {
            ""name"": ""PONG"",
            ""id"": ""20e4d6c3-d8da-488a-81bb-97ddc49acec4"",
            ""actions"": [
                {
                    ""name"": ""MoveP1"",
                    ""id"": ""83d86f98-d164-4138-a1f7-2d5ca9806fc9"",
                    ""expectedControlLayout"": ""Vector2"",
                    ""continuous"": true,
                    ""passThrough"": false,
                    ""initialStateCheck"": false,
                    ""processors"": """",
                    ""interactions"": """",
                    ""bindings"": []
                },
                {
                    ""name"": ""MoveP2"",
                    ""id"": ""db7679da-3357-4003-841c-49dd52bc2079"",
                    ""expectedControlLayout"": ""Vector2"",
                    ""continuous"": true,
                    ""passThrough"": false,
                    ""initialStateCheck"": false,
                    ""processors"": """",
                    ""interactions"": """",
                    ""bindings"": []
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""primary"",
                    ""id"": ""3c61f3af-78a7-4b15-8a98-61deaef78776"",
                    ""path"": ""2DVector"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveP1"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                },
                {
                    ""name"": ""up"",
                    ""id"": ""10de2413-6523-4e76-ae07-4474d2f8c241"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveP1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true,
                    ""modifiers"": """"
                },
                {
                    ""name"": ""down"",
                    ""id"": ""a00d24a9-57a5-4c07-8827-6aca52c06682"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveP1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true,
                    ""modifiers"": """"
                },
                {
                    ""name"": ""left"",
                    ""id"": ""b5ba6692-bafa-4c10-9a24-9c24faba7ced"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveP1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true,
                    ""modifiers"": """"
                },
                {
                    ""name"": ""right"",
                    ""id"": ""1a94b584-f382-4079-8333-cc7b90501b3d"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveP1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true,
                    ""modifiers"": """"
                },
                {
                    ""name"": ""primary"",
                    ""id"": ""7413be8e-d573-4c77-970a-cfa29b988d13"",
                    ""path"": ""2DVector"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveP2"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                },
                {
                    ""name"": ""up"",
                    ""id"": ""5e5fbdfe-b48a-4d34-a8fc-ec8f0d64bca6"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveP2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true,
                    ""modifiers"": """"
                },
                {
                    ""name"": ""down"",
                    ""id"": ""b70f82df-e540-4127-8e9a-fded0fce31d2"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveP2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true,
                    ""modifiers"": """"
                },
                {
                    ""name"": ""left"",
                    ""id"": ""8f7bc62b-f9c2-4550-acba-48c4e677bdc3"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveP2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true,
                    ""modifiers"": """"
                },
                {
                    ""name"": ""right"",
                    ""id"": ""6eef1172-2663-42b9-9c4b-88b6d3ae11b1"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveP2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true,
                    ""modifiers"": """"
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
            // PONG
            m_PONG = asset.GetActionMap("PONG");
            m_PONG_MoveP1 = m_PONG.GetAction("MoveP1");
            m_PONG_MoveP2 = m_PONG.GetAction("MoveP2");
        }
        ~Pong()
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
        public ReadOnlyArray<InputControlScheme> controlSchemes
        {
            get => asset.controlSchemes;
        }
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
        // PONG
        private InputActionMap m_PONG;
        private IPONGActions m_PONGActionsCallbackInterface;
        private InputAction m_PONG_MoveP1;
        private InputAction m_PONG_MoveP2;
        public struct PONGActions
        {
            private Pong m_Wrapper;
            public PONGActions(Pong wrapper) { m_Wrapper = wrapper; }
            public InputAction @MoveP1 { get { return m_Wrapper.m_PONG_MoveP1; } }
            public InputAction @MoveP2 { get { return m_Wrapper.m_PONG_MoveP2; } }
            public InputActionMap Get() { return m_Wrapper.m_PONG; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled { get { return Get().enabled; } }
            public InputActionMap Clone() { return Get().Clone(); }
            public static implicit operator InputActionMap(PONGActions set) { return set.Get(); }
            public void SetCallbacks(IPONGActions instance)
            {
                if (m_Wrapper.m_PONGActionsCallbackInterface != null)
                {
                    MoveP1.started -= m_Wrapper.m_PONGActionsCallbackInterface.OnMoveP1;
                    MoveP1.performed -= m_Wrapper.m_PONGActionsCallbackInterface.OnMoveP1;
                    MoveP1.cancelled -= m_Wrapper.m_PONGActionsCallbackInterface.OnMoveP1;
                    MoveP2.started -= m_Wrapper.m_PONGActionsCallbackInterface.OnMoveP2;
                    MoveP2.performed -= m_Wrapper.m_PONGActionsCallbackInterface.OnMoveP2;
                    MoveP2.cancelled -= m_Wrapper.m_PONGActionsCallbackInterface.OnMoveP2;
                }
                m_Wrapper.m_PONGActionsCallbackInterface = instance;
                if (instance != null)
                {
                    MoveP1.started += instance.OnMoveP1;
                    MoveP1.performed += instance.OnMoveP1;
                    MoveP1.cancelled += instance.OnMoveP1;
                    MoveP2.started += instance.OnMoveP2;
                    MoveP2.performed += instance.OnMoveP2;
                    MoveP2.cancelled += instance.OnMoveP2;
                }
            }
        }
        public PONGActions @PONG
        {
            get
            {
                return new PONGActions(this);
            }
        }
        public interface IPONGActions
        {
            void OnMoveP1(InputAction.CallbackContext context);
            void OnMoveP2(InputAction.CallbackContext context);
        }
    }
}
