using System;
using UnityEngine;
namespace Wayn.Mgm.Events.Registry
{
    [Serializable]
    public abstract class RegisteryReferenceAuthoring<ELEMENT> where ELEMENT : IRegistryElement
    {
        [SerializeReference]
        public ELEMENT Entry;

    }
}