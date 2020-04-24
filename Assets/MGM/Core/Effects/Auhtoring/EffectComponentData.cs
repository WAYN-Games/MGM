using System;
using Wayn.Mgm.Events.Registry;
using Unity.Entities;
using System.Collections.Generic;
using UnityEngine;

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
