using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;
namespace MGM
{
    public class ShootingInputResponse : InputResponse
    {
        protected override void RespondToAction(InputAction.CallbackContext context)
        {
            ShotTrigger scp = B_EntityManager.GetComponentData<ShotTrigger>(B_Entity);
            scp.IsTriggered = true;
            B_EntityManager.SetComponentData(B_Entity, scp);
        }

    }
}
