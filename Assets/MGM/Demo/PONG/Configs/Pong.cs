// GENERATED AUTOMATICALLY FROM 'Assets/MGM/Demo/PONG/Configs/Pong.inputactions'

using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace MGM.Demo
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
                    ""type"": ""Value"",
                    ""id"": ""83d86f98-d164-4138-a1f7-2d5ca9806fc9"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MoveP2"",
                    ""type"": ""Value"",
                    ""id"": ""db7679da-3357-4003-841c-49dd52bc2079"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
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
                    ""isPartOfComposite"": false
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
                    ""isPartOfComposite"": true
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
                    ""isPartOfComposite"": true
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
                    ""isPartOfComposite"": false
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
                    ""isPartOfComposite"": true
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
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
            // PONG
            m_PONG = asset.FindActionMap("PONG", throwIfNotFound: true);
            m_PONG_MoveP1 = m_PONG.FindAction("MoveP1", throwIfNotFound: true);
            m_PONG_MoveP2 = m_PONG.FindAction("MoveP2", throwIfNotFound: true);
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

        // PONG
        private readonly InputActionMap m_PONG;
        private IPONGActions m_PONGActionsCallbackInterface;
        private readonly InputAction m_PONG_MoveP1;
        private readonly InputAction m_PONG_MoveP2;
        public struct PONGActions
        {
            private Pong m_Wrapper;
            public PONGActions(Pong wrapper) { m_Wrapper = wrapper; }
            public InputAction @MoveP1 => m_Wrapper.m_PONG_MoveP1;
            public InputAction @MoveP2 => m_Wrapper.m_PONG_MoveP2;
            public InputActionMap Get() { return m_Wrapper.m_PONG; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(PONGActions set) { return set.Get(); }
            public void SetCallbacks(IPONGActions instance)
            {
                if (m_Wrapper.m_PONGActionsCallbackInterface != null)
                {
                    MoveP1.started -= m_Wrapper.m_PONGActionsCallbackInterface.OnMoveP1;
                    MoveP1.performed -= m_Wrapper.m_PONGActionsCallbackInterface.OnMoveP1;
                    MoveP1.canceled -= m_Wrapper.m_PONGActionsCallbackInterface.OnMoveP1;
                    MoveP2.started -= m_Wrapper.m_PONGActionsCallbackInterface.OnMoveP2;
                    MoveP2.performed -= m_Wrapper.m_PONGActionsCallbackInterface.OnMoveP2;
                    MoveP2.canceled -= m_Wrapper.m_PONGActionsCallbackInterface.OnMoveP2;
                }
                m_Wrapper.m_PONGActionsCallbackInterface = instance;
                if (instance != null)
                {
                    MoveP1.started += instance.OnMoveP1;
                    MoveP1.performed += instance.OnMoveP1;
                    MoveP1.canceled += instance.OnMoveP1;
                    MoveP2.started += instance.OnMoveP2;
                    MoveP2.performed += instance.OnMoveP2;
                    MoveP2.canceled += instance.OnMoveP2;
                }
            }
        }
        public PONGActions @PONG => new PONGActions(this);
        public interface IPONGActions
        {
            void OnMoveP1(InputAction.CallbackContext context);
            void OnMoveP2(InputAction.CallbackContext context);
        }
    }
}
