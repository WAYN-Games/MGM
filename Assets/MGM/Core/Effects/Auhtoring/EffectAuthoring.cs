using Wayn.Mgm.Events;
using System;
using Wayn.Mgm.Events.Registry;
using Unity.Entities;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EffectAuthoring : RegisteryReferenceAuthoring<IEffect>
{


}

[Serializable]
public class EffectComponentDataElement<ELEMENT, BUFFER> : ISelfRegistringAuhtoringComponent
     where ELEMENT : IRegistryElement
     where BUFFER : struct, IEffectReferenceBuffer
{
    public int Test;

    [SerializeField]
    public List<ELEMENT> elements;

    public EffectComponentDataElement()
    {
        Test = 10;
        elements = new List<ELEMENT>();
    }
    public EffectComponentDataElement(List<ELEMENT> elements)
    {
        this.Test = elements.Count;
        this.elements = elements;
    }

    public override bool Equals(object obj)
    {
        var element = obj as EffectComponentDataElement<ELEMENT, BUFFER>;
        return element != null &&
               EqualityComparer<List<ELEMENT>>.Default.Equals(elements, element.elements);
    }

    public override int GetHashCode()
    {
        return 272633004 + EqualityComparer<List<ELEMENT>>.Default.GetHashCode(elements);
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

[Serializable]
public class EffectComponentData : IComponentData
{
    public int Test2 = 15;

    [SerializeField]
    public List<ISelfRegistringAuhtoringComponent> listOfManagedBuffer = new List<ISelfRegistringAuhtoringComponent> ();


    public override bool Equals(object obj)
    {
        var data = obj as EffectComponentData;
        return data != null &&
               EqualityComparer<List<ISelfRegistringAuhtoringComponent>>.Default.Equals(listOfManagedBuffer, data.listOfManagedBuffer);
    }

    public override int GetHashCode()
    {
        return -644580469 + EqualityComparer<List<ISelfRegistringAuhtoringComponent>>.Default.GetHashCode(listOfManagedBuffer);
    }
}
