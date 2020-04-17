using Wayn.Mgm.Events;
using System;
using Wayn.Mgm.Events.Registry;
using Unity.Entities;
using System.Collections.Generic;

[Serializable]
public class EffectAuthoring : RegisteryReferenceAuthoring<IEffect>
{


}

public class ManagedBuffer : IEquatable<ManagedBuffer>
{
    public string BufferAssemblyQualifiedName = "";
    public List<object> Effects = new List<object>();

    public ManagedBuffer()
    {
        BufferAssemblyQualifiedName = "";
        Effects = new List<object>();
    }

    public bool Equals(ManagedBuffer other)
    {
        return BufferAssemblyQualifiedName.Equals(other.BufferAssemblyQualifiedName) && Effects.Equals(other.Effects);
    }

    public override int GetHashCode()
    {
        var hashCode = -927730715;
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(BufferAssemblyQualifiedName);
        hashCode = hashCode * -1521134295 + EqualityComparer<List<object>>.Default.GetHashCode(Effects);
        return hashCode;
    }
}

public class EffectComponentData : IComponentData,IEquatable<EffectComponentData>
{
    public List<ManagedBuffer> listOfManagedBuffer = new List<ManagedBuffer>();

    public EffectComponentData()
    {
        listOfManagedBuffer = new List<ManagedBuffer>();
    }

    public bool Equals(EffectComponentData other)
    {
        return other.listOfManagedBuffer.Equals(listOfManagedBuffer);
    }

    public override int GetHashCode()
    {
        return -644580469 + EqualityComparer<List<ManagedBuffer>>.Default.GetHashCode(listOfManagedBuffer);
    }
}
