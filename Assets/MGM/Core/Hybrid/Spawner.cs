using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace MGM
{
    public class Spawner : MonoBehaviour, IConvertGameObjectToEntity,IDeclareReferencedPrefabs
    {
        public GameObject Spawnable;
        public float CoolDown;

        public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
        {
            referencedPrefabs.Add(Spawnable);
        }
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
       /*     SpawnCapabilityParameters scp = new SpawnCapabilityParameters()
            {
                    Spawnable = conversionSystem.GetPrimaryEntity(Spawnable),
                    CoolDown = CoolDown,
                    TimeSinceLastTrigger = 0
            };

            dstManager.AddComponentData(entity, scp);
            */
        }



    }

}
