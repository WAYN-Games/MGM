using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using UnityEngine;
namespace MGM.Common
{
    public class MGMToolBox
    {
        public static float3 ECS_MouseToWorldPosition(Vector2 mousePos, Camera cam)
        {

            RaycastInput RaycastInput = new RaycastInput
            {
                Start = cam.transform.position,
                End = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.farClipPlane)),

                Filter = CollisionFilter.Default
            };

            CollisionWorld w = World.Active.GetExistingSystem<BuildPhysicsWorld>().PhysicsWorld.CollisionWorld;

            Unity.Physics.RaycastHit hit = new Unity.Physics.RaycastHit();

            RayCastJobUtils.SingleRayCast(w, RaycastInput, ref hit);

            return hit.Fraction == 0 ? RaycastInput.End : hit.Position;

        }
    }
}
