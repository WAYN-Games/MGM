// GENERATED AUTOMATICALLY FROM 'Assets/MGM/Demo/DoodleJump/Configs/DoodleJump.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace MGM.Demo
{
    public class DoodleJump : IInputActionCollection
    {
        private InputActionAsset asset;
        public DoodleJump()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""DoodleJump"",
    ""maps"": [
        {
            ""name"": ""Doodle"",
            ""id"": ""d4864674-15ed-4545-9a0a-be65f9110d3e"",
            ""actions"": [
                {
                    ""name"": ""Jump"",
                    ""id"": ""3620cc33-4109-4255-b739-51269d9ce61e"",
                    ""expectedControlLayout"": """",
                    ""continuous"": false,
                    ""passThrough"": false,
                    ""initialStateCheck"": false,
                    ""processors"": """",
                    ""interactions"": """",
                    ""bindings"": []
                },
                {
                    ""name"": ""Move"",
                    ""id"": ""7fe6db95-bd16-485e-9593-1d9400f0b26b"",
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
                    ""name"": """",
                    ""id"": ""d6d09e95-36bd-4843-93fa-5f054d0af4da"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""c9629bcc-8303-4fe1-b551-d3062801cded"",
                    ""path"": ""2DVector"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                },
                {
                    ""name"": ""left"",
                    ""id"": ""6146645f-39fa-40b9-b9b4-21e49cd5321b"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true,
                    ""modifiers"": """"
                },
                {
                    ""name"": ""right"",
                    ""id"": ""fc5b6880-b862-49a8-baad-2b0b0cda69d3"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true,
                    ""modifiers"": """"
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
            // Doodle
            m_Doodle = asset.GetActionMap("Doodle");
            m_Doodle_Jump = m_Doodle.GetAction("Jump");
            m_Doodle_Move = m_Doodle.GetAction("Move");
        }

        ~DoodleJump()
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

        // Doodle
        private InputActionMap m_Doodle;
        private IDoodleActions m_DoodleActionsCallbackInterface;
        private InputAction m_Doodle_Jump;
        private InputAction m_Doodle_Move;
        public struct DoodleActions
        {
            private DoodleJump m_Wrapper;
            public DoodleActions(DoodleJump wrapper) { m_Wrapper = wrapper; }
            public InputAction @Jump { get { return m_Wrapper.m_Doodle_Jump; } }
            public InputAction @Move { get { return m_Wrapper.m_Doodle_Move; } }
            public InputActionMap Get() { return m_Wrapper.m_Doodle; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled { get { return Get().enabled; } }
            public InputActionMap Clone() { return Get().Clone(); }
            public static implicit operator InputActionMap(DoodleActions set) { return set.Get(); }
            public void SetCallbacks(IDoodleActions instance)
            {
                if (m_Wrapper.m_DoodleActionsCallbackInterface != null)
                {
                    Jump.started -= m_Wrapper.m_DoodleActionsCallbackInterface.OnJump;
                    Jump.performed -= m_Wrapper.m_DoodleActionsCallbackInterface.OnJump;
                    Jump.canceled -= m_Wrapper.m_DoodleActionsCallbackInterface.OnJump;
                    Move.started -= m_Wrapper.m_DoodleActionsCallbackInterface.OnMove;
                    Move.performed -= m_Wrapper.m_DoodleActionsCallbackInterface.OnMove;
                    Move.canceled -= m_Wrapper.m_DoodleActionsCallbackInterface.OnMove;
                }
                m_Wrapper.m_DoodleActionsCallbackInterface = instance;
                if (instance != null)
                {
                    Jump.started += instance.OnJump;
                    Jump.performed += instance.OnJump;
                    Jump.canceled += instance.OnJump;
                    Move.started += instance.OnMove;
                    Move.performed += instance.OnMove;
                    Move.canceled += instance.OnMove;
                }
            }
        }
        public DoodleActions @Doodle
        {
            get
            {
                return new DoodleActions(this);
            }
        }
        public interface IDoodleActions
        {
            void OnJump(InputAction.CallbackContext context);
            void OnMove(InputAction.CallbackContext context);
        }
    }
}
