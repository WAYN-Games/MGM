using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementInputForwarder: InputActionForwarder<MovementDirection>
{
    public Camera Camera;

    private void Awake()
    {
        if(Camera == null)
        {
            Camera = Camera.main;
        }
    }

    public override void ReadAction(InputAction.CallbackContext ctx)
    {
        Vector2 InputMouvementDirection = ctx.ReadValue<Vector2>();
        //camera forward and right vectors:
        var forward = Camera.transform.forward;
        var right = Camera.transform.right;

        //project forward and right vectors on the horizontal plane (y = 0)
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();
        //this is the direction in the world space we want to move:
        var desiredMoveDirection = forward * InputMouvementDirection.y + right * InputMouvementDirection.x;
        ForwardAction(new MovementDirection() { Value = desiredMoveDirection });
    }
}