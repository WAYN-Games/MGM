using UnityEngine.InputSystem;
namespace MGM
{
   
    public class ShootingInputResponse : InputResponse
    {
        protected override void RespondToAction(InputAction.CallbackContext context)
        {
            Shot shot = B_EntityManager.GetComponentData<Shot>(B_Entity);
            shot.Trigger.IsTriggered = true;
            B_EntityManager.SetComponentData(B_Entity, shot);
        }

    }
}
