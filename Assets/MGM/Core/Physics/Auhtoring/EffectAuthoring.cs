using Wayn.Mgm.Events;
using System;
using Wayn.Mgm.Events.Registry;
using Unity.Entities;
using System.Collections.Generic;

[assembly: RegisterGenericComponentType(typeof(EffectComponentData<IEffect>))]
[assembly: RegisterGenericComponentType(typeof(ManagedBuffer<IEffect>))]

[Serializable]
public class EffectAuthoring : RegisteryReferenceAuthoring<IEffect>
{


}

public class ManagedBuffer<ELEMENT> : IEquatable<ManagedBuffer<ELEMENT>>
{
    public string BufferAssemblyQualifiedName = default;
    public List<ELEMENT> Effects = new List<ELEMENT>();

    public bool Equals(ManagedBuffer<ELEMENT> other)
    {
        return BufferAssemblyQualifiedName.Equals(other.BufferAssemblyQualifiedName) && Effects.Equals(other.Effects);
    }

    public override int GetHashCode()
    {
        var hashCode = -927730715;
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(BufferAssemblyQualifiedName);
        hashCode = hashCode * -1521134295 + EqualityComparer<List<ELEMENT>>.Default.GetHashCode(Effects);
        return hashCode;
    }
}

public class EffectComponentData<ELEMENT> : IComponentData,IEquatable<EffectComponentData<ELEMENT>>
{
    public List<ManagedBuffer<ELEMENT>> listOfManagedBuffer;

    public EffectComponentData()
    {
        listOfManagedBuffer = new List<ManagedBuffer<ELEMENT>>();
    }

    public bool Equals(EffectComponentData<ELEMENT> other)
    {
        return other.listOfManagedBuffer.Equals(listOfManagedBuffer);
    }

    public override int GetHashCode()
    {
        return -644580469 + EqualityComparer<List<ManagedBuffer<ELEMENT>>>.Default.GetHashCode(listOfManagedBuffer);
    }
}
