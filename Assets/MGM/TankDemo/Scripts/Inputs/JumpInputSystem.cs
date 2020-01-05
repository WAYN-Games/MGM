using UnityEngine.InputSystem;

public class JumpInputForwarder : InputActionForwarder<JumpTrigger>
{
    public override void ReadAction(InputAction.CallbackContext ctx)
    {
        ForwardAction(new JumpTrigger() { Value = ctx.ReadValue<float>() > 0 });
    }
}
