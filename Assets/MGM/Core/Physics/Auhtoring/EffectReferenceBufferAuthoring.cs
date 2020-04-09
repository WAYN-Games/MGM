using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using Wayn.Mgm.Effects;
using System;

public abstract class EffectReferenceBufferAuthoring<B> : EffectReferenceBufferAuthoring, IConvertGameObjectToEntity where B : struct,IEffectReferenceBuffer
{
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        // Get the effect registry.
        EffectRegistry er = EffectRegistry.Instance;

        // Add dynamic buffer to store Effect References on the entity
        DynamicBuffer<B> effectBuffer = dstManager.AddBuffer<B>(entity);

        // Fore each effect defined in hte inspector
        foreach (var effectAuthoring in Effects)
        {
            effectBuffer.Add(new B() { EffectReference = EffectRegistry.Instance.AddEffect(effectAuthoring.Effect) });
        }
    }
}

public abstract class EffectReferenceBufferAuthoring : MonoBehaviour
{
    public List<EffectAuthoring> Effects = new List<EffectAuthoring>();
}

[Serializable]
public class EffectAuthoring
{
    [SerializeReference]
    public IEffect Effect;
}
