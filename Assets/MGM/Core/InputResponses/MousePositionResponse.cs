using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;
using UnityEngine;
using UnityEngine.InputSystem;
namespace MGM
{
    public class MousePositionResponse : InputResponse
    {

        protected override void RespondToAction(InputAction.CallbackContext context)
        {
            
            Vector2 mousePos = context.ReadValue<Vector2>();

            Camera cam = Camera.main;
                        
            RaycastInput RaycastInput = new RaycastInput
            {
                Ray = new Unity.Physics.Ray { Origin = cam.transform.position, Direction = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.farClipPlane))
            },
                Filter = CollisionFilter.Default
            };
            
            CollisionWorld w = World.Active.GetExistingSystem<BuildPhysicsWorld>().PhysicsWorld.CollisionWorld;

            Unity.Physics.RaycastHit hit = new Unity.Physics.RaycastHit(); 
            
            RayCastJobUtils.SingleRayCast(w, RaycastInput, ref hit);
            
            Aim aim = B_EntityManager.GetComponentData<Aim>(B_Entity);
            aim.Value = hit.Position;
            B_EntityManager.SetComponentData(B_Entity, aim);

        }
    }
}
