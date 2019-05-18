using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
namespace MGM
{
    public class MouvementInputResponse : InputResponse
    {

        protected override void RespondToAction(InputAction.CallbackContext context)
        {
            Heading heading = B_EntityManager.GetComponentData<Heading>(B_Entity);
            var input = context.ReadValue<Vector2>();
            heading.Value = new float3(input.x,0,input.y);
            B_EntityManager.SetComponentData(B_Entity, heading);
        }
    }
}
