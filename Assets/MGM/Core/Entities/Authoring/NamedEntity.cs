using UnityEngine;
using Unity.Entities;
using Unity.Collections;

public class NamedEntity : MonoBehaviour, IConvertGameObjectToEntity
{
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        BlobBuilder bb = new BlobBuilder(Allocator.Temp);
        ref var root = ref bb.ConstructRoot<BlobStringContainer>();
        bb.AllocateString(ref root.str, gameObject.name);        
        dstManager.AddComponentData(entity,new EntityName() { Value = bb.CreateBlobAssetReference<BlobStringContainer>(Allocator.Persistent) });
        bb.Dispose();
    }
}

public struct BlobStringContainer
{
    public BlobString str;
}

public struct EntityName : IComponentData
{
    public BlobAssetReference<BlobStringContainer> Value;
}