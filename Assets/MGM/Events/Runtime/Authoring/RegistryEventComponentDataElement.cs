using System;
using System.Collections.Generic;

using Unity.Entities;
namespace Wayn.Mgm.Event.Registry
{
    [Serializable]
    public abstract class RegistryEventComponentDataElement<ELEMENT, BUFFER> : ISelfRegistringAuhtoringComponent
     where ELEMENT : IRegistryEvent
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
            DynamicBuffer<BUFFER> buffer = ecb.AddBuffer<BUFFER>(entity);
            foreach (ELEMENT regitryEvent in elements)
            {
                buffer.Add(new BUFFER() { RegistryEventReference = AddEventToRegistry(regitryEvent) });
            }
        }

        protected abstract RegistryEventReference AddEventToRegistry(ELEMENT registeryEvent);
    }
}
