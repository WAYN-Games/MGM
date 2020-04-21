using Wayn.Mgm.Events;
using System;
using Wayn.Mgm.Events.Registry;
using Unity.Entities;
using System.Collections.Generic;

[Serializable]
public class EffectAuthoring : RegisteryReferenceAuthoring<IEffect>
{


}


public class EffectComponentData : IComponentData,IEquatable<EffectComponentData>
{
    public List<ISelfRegistringAuhtoringComponent> listOfManagedBuffer = new List<ISelfRegistringAuhtoringComponent> ();

    public EffectComponentData()
    {
        listOfManagedBuffer = new List<ISelfRegistringAuhtoringComponent>();
    }

    public bool Equals(EffectComponentData other)
    {
        return other.listOfManagedBuffer.Equals(listOfManagedBuffer);
    }

    public override int GetHashCode()
    {
        return -644580469 + EqualityComparer<List<ISelfRegistringAuhtoringComponent>>.Default.GetHashCode(listOfManagedBuffer);
    }
}
