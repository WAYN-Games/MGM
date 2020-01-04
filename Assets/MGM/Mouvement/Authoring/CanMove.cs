using UnityEngine;
using com.WaynGroup.RBAW;
using Unity.Physics.Authoring;
using Unity.Entities;
using UnityEngine.InputSystem;

[RequireComponent(typeof(MovementDirection), typeof(MovementSpeed), typeof(PhysicsBodyAuthoring))]
//[RequireComponent(typeof(ContactInfosAuthoring))]
[RequireComponent(typeof(AlwaysFaceMovementDirection))]
public class CanMove : RequirementBasedAuthoringComponent
{
}
