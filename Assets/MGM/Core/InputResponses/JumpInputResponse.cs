using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;
namespace MGM
{
    public class JumpInputResponse : InputListener
    {
        protected override void RespondToAction(InputAction.CallbackContext context)
        {

            JumpTriger jcp = B_EntityManager.GetComponentData<JumpTriger>(B_Entity);
            jcp.IsTriggered = true;
            B_EntityManager.SetComponentData(B_Entity, jcp);
        }
    }
}
