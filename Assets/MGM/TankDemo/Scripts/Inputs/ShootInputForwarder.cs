using UnityEngine.InputSystem;

public class ShootInputForwarder : InputActionForwarder<ShootTrigger>
{
    public override void ReadAction(InputAction.CallbackContext ctx)
    {
        ForwardAction(new ShootTrigger() { Value = ctx.ReadValue<float>() > 0 });
    }
}
