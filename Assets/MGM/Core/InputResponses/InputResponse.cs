using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MGM
{
    /// <summary>
    /// Abstract input listener to extend to take into acount new input action.
    /// </summary>
    public abstract class InputListener : MonoBehaviour
    {
        /// <summary>
        /// Action Mapping.
        /// </summary>
        protected InputAction B_InputAction;
        /// <summary>
        /// Entity affected by the action.
        /// </summary>
        protected Entity B_Entity;
        /// <summary>
        /// Entity Manager.
        /// </summary>
        protected EntityManager B_EntityManager;

        /// <summary>
        /// Setup the action listener.
        /// </summary>
        /// <param name="_act">Action to listen to.</param>
        /// <param name="_entity">Entity on wich to store the input.</param>
        /// <param name="_em">Entity manager to access set the  entity's component.</param>
        public void Setup(InputAction _act, Entity _entity, EntityManager _em)
        {
            // cache parameters for use in child clases.
            B_InputAction = _act;
            B_EntityManager = _em;
            B_Entity = _entity;

            // setup input listener
            Enable();



        }

        /// <summary>
        /// Allow to enable the lister.
        /// </summary>
        private void OnEnable()
        {
            Enable();
        }

        private void Enable()
        {
            if (B_InputAction == null) return;
            B_InputAction.performed += RespondToAction;
            B_InputAction.Enable();
        }

        /// <summary>
        /// Method to implement in the child class to define how the input is stored on the entity.
        /// </summary>
        /// <param name="context"></param>
        protected abstract void RespondToAction(InputAction.CallbackContext context);


        /// <summary>
        /// Allow to disable the input listener.
        /// </summary>
        private void OnDisable()
        {
            Disable();

        }

        private void Disable()
        {
            if (B_InputAction == null) return;
            B_InputAction.performed -= RespondToAction;
            B_InputAction.Disable();
        }
    }
}