using Unity.Entities;
using UnityEngine;

public class ContactInfosAuthoring : MonoBehaviour,IConvertGameObjectToEntity
{
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddBuffer<ContactInfos>(entity);
    }
}
