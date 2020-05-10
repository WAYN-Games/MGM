using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
namespace Wayn.Mgm.Events.Registry
{



    /// <summary>
    ///  This class handle all the complexity of managing the population of the DynamicBuffer with the correct RegistryReference
    /// </summary>
    /// <typeparam name="BUFFER"></typeparam>
    /// <typeparam name="ELEMENT"></typeparam>
    /// <typeparam name="AUTHORING"></typeparam>
    /// <typeparam name="REGISTRY"></typeparam>
    public abstract class RegisteryReferenceBufferAuthoring<BUFFER, ELEMENT, AUTHORING, REGISTRY> : MonoBehaviour, IConvertGameObjectToEntity
        where BUFFER : struct, IRegistryReferenceBuffer
        where ELEMENT : IRegistryElement
        where AUTHORING : RegisteryReferenceAuthoring<ELEMENT>
        where REGISTRY : Registry<REGISTRY>
    {
        [SerializeField]
        public List<AUTHORING> Entries = new List<AUTHORING>();

        protected abstract REGISTRY GetRegisteryInstance();
        
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            
            if (!dstManager.HasComponent<RegistryEventReferenceComponentData>(entity)){
                dstManager.AddComponentData(entity, new RegistryEventReferenceComponentData());
            }

            RegistryEventReferenceComponentData component = dstManager.GetComponentData<RegistryEventReferenceComponentData>(entity);

            List<ELEMENT> elems = new List<ELEMENT>();

            foreach(var entry in Entries)
            {
                elems.Add(entry.Entry);
            }

            RegistryEventComponentDataElement<ELEMENT, BUFFER> elem = new EffectComponentDataElement<ELEMENT, BUFFER>();
            elem.SetElementList(elems);
            component.listOfManagedBuffer.Add(elem);
            dstManager.SetComponentData(entity,component);
        }
    }
}