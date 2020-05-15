using System;
using Wayn.Mgm.Event.Registry;
using Unity.Entities;
using System.Collections.Generic;

namespace Wayn.Mgm.Event.Registry
{
    [Serializable]
    public class RegistryEventReferenceComponentData : IComponentData
    {
        public List<ISelfRegistringAuhtoringComponent> listOfManagedBuffer = new List<ISelfRegistringAuhtoringComponent>();
    }
}
