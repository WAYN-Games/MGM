using System;
using Wayn.Mgm.Events.Registry;
using Unity.Entities;
using System.Collections.Generic;

[Serializable]
public class EffectComponentData : IComponentData
{
    public List<ISelfRegistringAuhtoringComponent> listOfManagedBuffer = new List<ISelfRegistringAuhtoringComponent> ();
}
