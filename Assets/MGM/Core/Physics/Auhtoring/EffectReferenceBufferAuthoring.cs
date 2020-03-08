using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using Wayn.Mgm.Effects;
using System;
using Wayn.Mgm.Effects.Generated;

public abstract class EffectReferenceBufferAuthoring<B> : EffectReferenceBufferAuthoring, IConvertGameObjectToEntity where B : struct,IEffectReferenceBuffer
{
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        EffectRegistry er = dstManager.World.GetOrCreateSystem<EffectBufferSystem>().EffectBuffer.effectRegistry;
        DynamicBuffer<B> effectBuffer = dstManager.AddBuffer<B>(entity);
        foreach (var effectAuthoring in Effects)
        {
            IEffect effect = effectAuthoring.Effect;
            effectBuffer.Add(new B() { EffectReference = new EffectReference() { TypeId = EffectReference.GetTypeId(effect.GetType()), VersionId = er.AddEffectVerion(effect) } });
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
    public int Test;
    [SerializeReference]
    public IEffect Effect;
}
