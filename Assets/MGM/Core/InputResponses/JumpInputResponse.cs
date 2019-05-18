using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;
namespace MGM
{
    public class JumpInputResponse : InputResponse
    {
        protected override void RespondToAction(InputAction.CallbackContext context)
        {

            JumpCapabilityParameters jcp = B_EntityManager.GetComponentData<JumpCapabilityParameters>(B_Entity);
            jcp.JumpTrigerred = true;
            B_EntityManager.SetComponentData(B_Entity, jcp);
        }
    }
}
