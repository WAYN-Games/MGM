using Unity.Entities;
using UnityEngine;
using UnityEngine.Experimental.Input;

namespace MGM
{
    public abstract class InputResponse : MonoBehaviour
    {




        protected InputAction B_InputAction;
        protected Entity B_Entity;
        protected EntityManager B_EntityManager;

        public void Setup(InputAction _act, Entity _entity, EntityManager _em)
        {
            // cache parameters for letter use
            B_InputAction = _act;
            B_EntityManager = _em;
            B_Entity = _entity;

            // setup input response
            B_InputAction.performed += RespondToAction;
            B_InputAction.Enable();



        }

        private void OnEnable()
        {
            if (B_InputAction == null) return;
            B_InputAction.performed += RespondToAction;
            B_InputAction.Enable();
        }


        protected abstract void RespondToAction(InputAction.CallbackContext context);



        private void OnDisable()
        {
            if (B_InputAction == null) return;
            B_InputAction.performed -= RespondToAction;
            B_InputAction.Disable();

        }
    }
}