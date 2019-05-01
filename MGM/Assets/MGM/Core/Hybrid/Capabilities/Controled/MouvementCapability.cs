using Unity.Entities;
using Unity.Physics.Authoring;
using UnityEngine;

namespace MGM
{
    [RequireComponent(typeof(PhysicsBody))]
    public class MouvementCapability : ControledCapability<MouvementInputResponse>
    {
        [SerializeField] private float MouvementSpeed = 5;
        [SerializeField] [Range(0, 1)] private float MovementInertia = 1;
        [SerializeField] private bool ShouldFaceForward = true;
        
        protected override void SetUpCapabilityParameters(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            var data = new MouvementCapabilityParameters
            {
                Speed = MouvementSpeed,
                ShouldFaceForward = ShouldFaceForward,
                MovementInertia = MovementInertia
            };


            dstManager.AddComponentData(entity, data);

        }
    }
}
