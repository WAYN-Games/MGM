// GENERATED AUTOMATICALLY FROM 'Assets/MGMInputConfig.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Input;
using UnityEngine.Experimental.Input.Utilities;

namespace MGM
{
    public class MGMInputConfig : IInputActionCollection
    {
        private InputActionAsset asset;
        public MGMInputConfig()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""MGMInputConfig"",
    ""maps"": [
        {
            ""name"": ""MGM"",
            ""id"": ""37e9f8db-4968-4bae-98d9-be78a1e95576"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""id"": ""15093b53-8bf0-4098-92d5-659db76120d6"",
                    ""expectedControlLayout"": ""Vector2"",
                    ""continuous"": true,
                    ""passThrough"": false,
                    ""initialStateCheck"": false,
                    ""processors"": """",
                    ""interactions"": """",
                    ""bindings"": []
                },
                {
                    ""name"": ""Jump"",
                    ""id"": ""e9f244c6-557d-4645-b369-5e7cc5f73388"",
                    ""expectedControlLayout"": ""Key"",
                    ""continuous"": false,
                    ""passThrough"": false,
                    ""initialStateCheck"": false,
                    ""processors"": """",
                    ""interactions"": """",
                    ""bindings"": []
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""zqsd"",
                    ""id"": ""baacb385-1178-453f-9078-209b95f0c182"",
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
                    ""name"": ""up"",
                    ""id"": ""6275717d-382a-43ec-a616-2f4d1bf1571d"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true,
                    ""modifiers"": """"
                },
                {
                    ""name"": ""down"",
                    ""id"": ""3b34ac60-6f15-4b7d-843c-f4fc430e1b6f"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true,
                    ""modifiers"": """"
                },
                {
                    ""name"": ""left"",
                    ""id"": ""6b7476ed-1057-4a76-9072-4065f748d83a"",
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
                    ""id"": ""fe2e7a25-505b-4983-8c64-9266d532c6b8"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true,
                    ""modifiers"": """"
                },
                {
                    ""name"": """",
                    ""id"": ""99d894d4-50b5-458b-88fe-6ee44eb506a2"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
            // MGM
            m_MGM = asset.GetActionMap("MGM");
            m_MGM_Move = m_MGM.GetAction("Move");
            m_MGM_Jump = m_MGM.GetAction("Jump");
        }
        ~MGMInputConfig()
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
        // MGM
        private InputActionMap m_MGM;
        private IMGMActions m_MGMActionsCallbackInterface;
        private InputAction m_MGM_Move;
        private InputAction m_MGM_Jump;
        public struct MGMActions
        {
            private MGMInputConfig m_Wrapper;
            public MGMActions(MGMInputConfig wrapper) { m_Wrapper = wrapper; }
            public InputAction @Move { get { return m_Wrapper.m_MGM_Move; } }
            public InputAction @Jump { get { return m_Wrapper.m_MGM_Jump; } }
            public InputActionMap Get() { return m_Wrapper.m_MGM; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled { get { return Get().enabled; } }
            public InputActionMap Clone() { return Get().Clone(); }
            public static implicit operator InputActionMap(MGMActions set) { return set.Get(); }
            public void SetCallbacks(IMGMActions instance)
            {
                if (m_Wrapper.m_MGMActionsCallbackInterface != null)
                {
                    Move.started -= m_Wrapper.m_MGMActionsCallbackInterface.OnMove;
                    Move.performed -= m_Wrapper.m_MGMActionsCallbackInterface.OnMove;
                    Move.cancelled -= m_Wrapper.m_MGMActionsCallbackInterface.OnMove;
                    Jump.started -= m_Wrapper.m_MGMActionsCallbackInterface.OnJump;
                    Jump.performed -= m_Wrapper.m_MGMActionsCallbackInterface.OnJump;
                    Jump.cancelled -= m_Wrapper.m_MGMActionsCallbackInterface.OnJump;
                }
                m_Wrapper.m_MGMActionsCallbackInterface = instance;
                if (instance != null)
                {
                    Move.started += instance.OnMove;
                    Move.performed += instance.OnMove;
                    Move.cancelled += instance.OnMove;
                    Jump.started += instance.OnJump;
                    Jump.performed += instance.OnJump;
                    Jump.cancelled += instance.OnJump;
                }
            }
        }
        public MGMActions @MGM
        {
            get
            {
                return new MGMActions(this);
            }
        }
        public interface IMGMActions
        {
            void OnMove(InputAction.CallbackContext context);
            void OnJump(InputAction.CallbackContext context);
        }
    }
}
