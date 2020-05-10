using Wayn.Mgm.Events;
using System;
using Wayn.Mgm.Events.Registry;
using Unity.Entities;
using System.Collections.Generic;

[Serializable]
public abstract class RegistryEventComponentDataElement<ELEMENT, BUFFER> : ISelfRegistringAuhtoringComponent
     where ELEMENT : IRegistryElement
     where BUFFER : struct, IRegistryReferenceBuffer
{
    public int number; // Without this, the subscene fails to load

    public List<ELEMENT> elements;

    public RegistryEventComponentDataElement()
    {
        elements = new List<ELEMENT>();
        number = elements.Count;
    }
    public void SetElementList(List<ELEMENT> elements)
    {
        this.elements = elements;
    }

    public void Register(EntityCommandBuffer ecb, Entity entity)
    {
        var buffer = ecb.AddBuffer<BUFFER>(entity);
        foreach (ELEMENT regitryEvent in elements)
        {
            buffer.Add(new BUFFER() { RegistryEventReference = AddEventToRegistry(regitryEvent) });
        }
    }

    protected abstract RegistryReference AddEventToRegistry(ELEMENT registeryEvent);
}
