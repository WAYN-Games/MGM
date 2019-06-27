using MGM.Weapon;
using UnityEngine.InputSystem;
namespace MGM
{
   
    public class ShootingInputListener : InputListener
    {
        /// <summary>
        /// Method listening to the performed action and storing the input to the base shot component.
        /// </summary>
        /// <param name="context"></param>
        protected override void RespondToAction(InputAction.CallbackContext context)
        {

            // Get the current Shot component data to avoid reseting it to default.
            Shot shot = B_EntityManager.GetComponentData<Shot>(B_Entity);
            // Store the input.
            shot.Trigger.IsTriggered = true;
            B_EntityManager.SetComponentData(B_Entity, shot);
        }

    }
}
