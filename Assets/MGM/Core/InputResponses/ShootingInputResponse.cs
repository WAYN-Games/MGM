using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;
namespace MGM
{
    public class ShootingInputResponse : InputResponse
    {
        protected override void RespondToAction(InputAction.CallbackContext context)
        {
            ShootingCapabilityParameters scp = B_EntityManager.GetComponentData<ShootingCapabilityParameters>(B_Entity);
            scp.spawnCapabilityParameters.SpawnTrigerred = true;
            B_EntityManager.SetComponentData(B_Entity, scp);
        }

    }
}
