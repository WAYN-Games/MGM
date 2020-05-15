using Unity.Collections;
using Unity.Entities;

using UnityEngine;

public class NamedEntity : MonoBehaviour, IConvertGameObjectToEntity
{
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        BlobBuilder bb = new BlobBuilder(Allocator.Temp);
        ref BlobStringContainer root = ref bb.ConstructRoot<BlobStringContainer>();
        bb.AllocateString(ref root.str, gameObject.name);
        dstManager.AddComponentData(entity, new EntityName() { Value = bb.CreateBlobAssetReference<BlobStringContainer>(Allocator.Persistent) });
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
