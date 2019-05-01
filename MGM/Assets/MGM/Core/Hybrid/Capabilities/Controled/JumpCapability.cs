using Unity.Entities;
using UnityEngine;
using Unity.Physics.Authoring;

namespace MGM
{
    [RequireComponent(typeof(PhysicsBody))]
    public class JumpCapability : ControledCapability<JumpInputResponse>
    {
        [SerializeField] private float Force = 5;
        [SerializeField] private int MaxJumpNumber = 1;



        protected override void SetUpCapabilityParameters(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {

            Debug.Log(this.name);
            Debug.Log(entity);
            var data = new JumpCapabilityParameters
            {
                Force = Force,
                MaxJumpNumber = MaxJumpNumber,
                CurrentJumpCount = 0
            };

            dstManager.AddComponentData(entity, data);
        }
    }
}
