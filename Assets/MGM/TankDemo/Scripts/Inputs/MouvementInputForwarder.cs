using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouvementInputForwarder : InputActionForwarder
{
    public override void ForwardAction(InputAction.CallbackContext ctx)
    {
        Vector2 InputMouvementDirection = ctx.ReadValue<Vector2>();

        float3 m_InputDirection = new float3(InputMouvementDirection.x, 0, InputMouvementDirection.y);

        EntityManager.SetComponentData(PlayerEntity, new MovementDirection() { Value = m_InputDirection });
    }

}