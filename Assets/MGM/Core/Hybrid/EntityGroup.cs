using Unity.Entities;
using UnityEngine;

[RequiresEntityConversion]
public class EntityGroup : MonoBehaviour, IConvertGameObjectToEntity
{

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        conversionSystem.DeclareLinkedEntityGroup(gameObject);
    }

}
