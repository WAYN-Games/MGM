using UnityEngine;
using UnityEngine.InputSystem;

public class MovementInputForwarder: InputActionForwarder<MovementDirection>
{
    public override void ReadAction(InputAction.CallbackContext ctx)
    {
        if (PlayerCamera == null) PlayerCamera = GetComponent<PlayerInput>().camera;
        var forward = PlayerCamera.transform.forward;
        var right = PlayerCamera.transform.right;

        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector2 InputMouvementDirection = ctx.ReadValue<Vector2>();

        var desiredMoveDirection =  Vector3.right * InputMouvementDirection.x + Vector3.forward* InputMouvementDirection.y;
       
        ForwardAction(new MovementDirection() { Value = desiredMoveDirection });
    }
}