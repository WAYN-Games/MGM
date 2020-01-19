using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using UnityEngine;
using UnityEngine.InputSystem;

public class AimInputForwarder : InputActionForwarder<AimPosition>
{
  
    private Vector2 PointerPosition = new Vector2(0,0);
    public override void ReadAction(InputAction.CallbackContext ctx)
    {
        if(!ctx.control.IsActuated()) return;        
        PointerPosition = ctx.ReadValue<Vector2>();
    }

    void Update()
    {
        if (PlayerCamera == null) return;
        ForwardAction(new AimPosition() { Value = MouseToWorldPosition(PointerPosition, PlayerCamera) });
    }

    private float3 MouseToWorldPosition(Vector2 mousePos, Camera cam)
    {

        RaycastInput RaycastInput = new RaycastInput
        {
            Start = cam.transform.position,
            End = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.farClipPlane)),

            Filter = CollisionFilter.Default
        };

        CollisionWorld collisionWorld = EntityManager.World.GetExistingSystem<BuildPhysicsWorld>().PhysicsWorld.CollisionWorld;

        Unity.Physics.RaycastHit hit = new Unity.Physics.RaycastHit();

        RayCastJobUtils.SingleRayCast(collisionWorld, RaycastInput, ref hit);

        return hit.Fraction == 0 ? RaycastInput.End : hit.Position;

    }
}
