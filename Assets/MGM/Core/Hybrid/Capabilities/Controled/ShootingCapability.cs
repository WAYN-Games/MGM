using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace MGM
{
    public class ShootingCapability :   ControledCapability<ShootingInputResponse> , IDeclareReferencedPrefabs
    {
        public GameObject Bullet;
        public float CoolDown;
        public float Speed;

        public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
        {
            referencedPrefabs.Add(Bullet);
        }

        protected override void SetUpCapabilityParameters(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {

            ShootingCapabilityParameters scp = new ShootingCapabilityParameters() {
                spawnCapabilityParameters = new SpawnCapabilityParameters()
                {
                    Spawnable = conversionSystem.GetPrimaryEntity(Bullet),
                    CoolDown = CoolDown,
                    TimeSinceLastTrigger = 0
                },
                Speed = Speed
            };

            dstManager.AddComponentData(entity, scp);

            ShotTrigger st = new ShotTrigger()
            {
                IsTriggered = false
            };

            dstManager.AddComponentData(entity, st);
        }
    }

}
