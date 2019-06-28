using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using MGM.Common;
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

            Aim aim = B_EntityManager.GetComponentData<Aim>(B_Entity);

            aim.Value = MGMToolBox.ECS_MouseToWorldPosition(mousePos, cam);

            B_EntityManager.SetComponentData(B_Entity, aim);
        }




    }
}
