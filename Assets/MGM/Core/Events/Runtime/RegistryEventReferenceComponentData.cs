using System;
using Wayn.Mgm.Events.Registry;
using Unity.Entities;
using System.Collections.Generic;

[Serializable]
public class RegistryEventReferenceComponentData : IComponentData
{
    public List<ISelfRegistringAuhtoringComponent> listOfManagedBuffer = new List<ISelfRegistringAuhtoringComponent> ();
}
