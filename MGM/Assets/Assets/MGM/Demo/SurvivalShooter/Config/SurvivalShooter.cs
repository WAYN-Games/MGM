// GENERATED AUTOMATICALLY FROM 'Assets/MGM/Demo/SurvivalShooter/Config/SurvivalShooter.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Input;
using UnityEngine.Experimental.Input.Utilities;

namespace MGM.Demo
{
    public class SurvivalShooter : IInputActionCollection
    {
        private InputActionAsset asset;
        public SurvivalShooter()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""SurvivalShooter"",
    ""maps"": [
        {
            ""name"": ""Character"",
            ""id"": ""a368e887-82ea-4d8b-8066-c7543a840ce2"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""id"": ""924c4250-865d-4252-9911-f0c50d733139"",
                    ""expectedControlLayout"": ""Vector2"",
                    ""continuous"": true,
                    ""passThrough"": false,
                    ""initialStateCheck"": false,
                    ""processors"": """",
                    ""interactions"": """",
                    ""bindings"": []
                },
                {
                    ""name"": ""Aim"",
                    ""id"": ""5070d6d9-84f9-4219-9e24-924e18eaa336"",
                    ""expectedControlLayout"": """",
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
                    ""name"": ""2D Vector"",
                    ""id"": ""5b226dad-24c6-439a-a6e3-10b16171173f"",
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
                    ""id"": ""68f2b4c4-a347-4be4-86d1-a87a1c12ce91"",
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
                    ""id"": ""2a0d6965-d370-402f-a2fa-b5cf1cd872cb"",
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
                    ""id"": ""effec8cc-3b05-4124-b843-49ead4cf6682"",
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
                    ""id"": ""afee72d4-9396-4114-8337-9af5bf9825a6"",
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
                    ""id"": ""377ffe57-6af8-497e-9e18-dc2aa1ae1750"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false,
                    ""modifiers"": """"
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
            // Character
            m_Character = asset.GetActionMap("Character");
            m_Character_Move = m_Character.GetAction("Move");
            m_Character_Aim = m_Character.GetAction("Aim");
        }
        ~SurvivalShooter()
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
        // Character
        private InputActionMap m_Character;
        private ICharacterActions m_CharacterActionsCallbackInterface;
        private InputAction m_Character_Move;
        private InputAction m_Character_Aim;
        public struct CharacterActions
        {
            private SurvivalShooter m_Wrapper;
            public CharacterActions(SurvivalShooter wrapper) { m_Wrapper = wrapper; }
            public InputAction @Move { get { return m_Wrapper.m_Character_Move; } }
            public InputAction @Aim { get { return m_Wrapper.m_Character_Aim; } }
            public InputActionMap Get() { return m_Wrapper.m_Character; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled { get { return Get().enabled; } }
            public InputActionMap Clone() { return Get().Clone(); }
            public static implicit operator InputActionMap(CharacterActions set) { return set.Get(); }
            public void SetCallbacks(ICharacterActions instance)
            {
                if (m_Wrapper.m_CharacterActionsCallbackInterface != null)
                {
                    Move.started -= m_Wrapper.m_CharacterActionsCallbackInterface.OnMove;
                    Move.performed -= m_Wrapper.m_CharacterActionsCallbackInterface.OnMove;
                    Move.cancelled -= m_Wrapper.m_CharacterActionsCallbackInterface.OnMove;
                    Aim.started -= m_Wrapper.m_CharacterActionsCallbackInterface.OnAim;
                    Aim.performed -= m_Wrapper.m_CharacterActionsCallbackInterface.OnAim;
                    Aim.cancelled -= m_Wrapper.m_CharacterActionsCallbackInterface.OnAim;
                }
                m_Wrapper.m_CharacterActionsCallbackInterface = instance;
                if (instance != null)
                {
                    Move.started += instance.OnMove;
                    Move.performed += instance.OnMove;
                    Move.cancelled += instance.OnMove;
                    Aim.started += instance.OnAim;
                    Aim.performed += instance.OnAim;
                    Aim.cancelled += instance.OnAim;
                }
            }
        }
        public CharacterActions @Character
        {
            get
            {
                return new CharacterActions(this);
            }
        }
        public interface ICharacterActions
        {
            void OnMove(InputAction.CallbackContext context);
            void OnAim(InputAction.CallbackContext context);
        }
    }
}
