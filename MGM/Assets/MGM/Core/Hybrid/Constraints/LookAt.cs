using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace MGM
{
    public class LookAt : MonoBehaviour, IConvertGameObjectToEntity,IDeclareReferencedPrefabs
    {
        public GameObject Target;

        public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
        {
            referencedPrefabs.Add(Target);
        }
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            Entity e = conversionSystem.GetPrimaryEntity(Target);

            Target target = new Target()
            {
                    Entity = e
            };

            dstManager.AddComponentData(entity, target);
            if (!dstManager.HasComponent<IsTargeted>(e)) dstManager.AddComponent(e, typeof(IsTargeted));
        }



    }

}
