using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;
using UnityEngine;
using UnityEngine.InputSystem;
namespace MGM
{
    public class MousePositionResponse : InputListener
    {

        protected override void RespondToAction(InputAction.CallbackContext context)
        {
            
            Vector2 mousePos = context.ReadValue<Vector2>();
            
            Camera cam = Camera.main;
                        
            RaycastInput RaycastInput = new RaycastInput
            {
                Start = cam.transform.position,
                End = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.farClipPlane)),          
            
                Filter = CollisionFilter.Default
            }; 

            CollisionWorld w = World.Active.GetExistingSystem<BuildPhysicsWorld>().PhysicsWorld.CollisionWorld;

            Unity.Physics.RaycastHit hit = new Unity.Physics.RaycastHit(); 
            
            RayCastJobUtils.SingleRayCast(w, RaycastInput, ref hit);



            Aim aim = B_EntityManager.GetComponentData<Aim>(B_Entity);

            var position = hit.Fraction == 0 ? RaycastInput.End: hit.Position;

            aim.Value = position;
            B_EntityManager.SetComponentData(B_Entity, aim);

        }
    }
}
