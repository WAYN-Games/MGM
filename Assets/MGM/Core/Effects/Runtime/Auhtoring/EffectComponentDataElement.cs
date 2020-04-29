using Wayn.Mgm.Events;
using System;
using Wayn.Mgm.Events.Registry;
using Unity.Entities;
using System.Collections.Generic;

[Serializable]
public class EffectComponentDataElement<ELEMENT, BUFFER> : ISelfRegistringAuhtoringComponent
     where ELEMENT : IRegistryElement
     where BUFFER : struct, IEffectReferenceBuffer
{
    public int number; // Without this, the subscene fails to load

    public List<ELEMENT> elements;

    public EffectComponentDataElement()
    {
        elements = new List<ELEMENT>();
        number = elements.Count;
    }
    public EffectComponentDataElement(List<ELEMENT> elements)
    {
        this.elements = elements;
    }

    public void Register(EntityCommandBuffer ecb, Entity entity)
    {
        var buffer = ecb.AddBuffer<BUFFER>(entity);
        foreach (var effect in elements)
        {
            buffer.Add(new BUFFER() { EffectReference = EffectRegistry.Instance.AddEffect((IEffect)effect) });
        }
    }
}
