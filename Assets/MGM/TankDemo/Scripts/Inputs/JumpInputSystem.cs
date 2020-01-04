using Unity.Entities;
using UnityEngine.InputSystem;

public class JumpInputForwarder : InputActionForwarder
{
    public override void ForwardAction(InputAction.CallbackContext ctx)
    { 
        EntityManager.SetComponentData(PlayerEntity, new JumpTrigger() { Value = ctx.ReadValue<float>() > 0 });
    }
}
