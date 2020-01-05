using UnityEngine;
using com.WaynGroup.RBAW;
using Unity.Physics.Authoring;

[RequireComponent(typeof(JumpForce), typeof(JumpCount), typeof(MaxJumpCount))]
[RequireComponent(typeof(JumpTrigger))]
[RequireComponent(typeof(PhysicsBodyAuthoring))]
public class CanJump : RequirementBasedAuthoringComponent
{
}
