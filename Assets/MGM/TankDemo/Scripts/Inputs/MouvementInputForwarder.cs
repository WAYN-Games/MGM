using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouvementInputForwarder: InputActionForwarder<MovementDirection>
{
    public override void ReadAction(InputAction.CallbackContext ctx)
    {
        Vector2 InputMouvementDirection = ctx.ReadValue<Vector2>();
        ForwardAction(new MovementDirection() { Value = new float3(InputMouvementDirection.x, 0, InputMouvementDirection.y) });
    }

}