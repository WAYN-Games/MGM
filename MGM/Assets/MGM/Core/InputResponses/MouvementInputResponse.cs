using Unity.Entities;
using UnityEngine;
using UnityEngine.Experimental.Input;
namespace MGM
{
    public class MouvementInputResponse : InputResponse
    {

        protected override void RespondToAction(InputAction.CallbackContext context)
        {

            MouvementCapabilityParameters mcp = B_EntityManager.GetComponentData<MouvementCapabilityParameters>(B_Entity);
            mcp.direction = context.ReadValue<Vector2>();
            B_EntityManager.SetComponentData(B_Entity, mcp);
        }
    }
}
