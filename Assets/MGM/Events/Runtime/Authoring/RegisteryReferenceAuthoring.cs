using System;
using UnityEngine;

namespace Wayn.Mgm.Event.Registry
{
    [Serializable]
    public abstract class RegisteryReferenceAuthoring<ELEMENT> where ELEMENT : IRegistryEvent
    {
        [SerializeReference]
        public ELEMENT Entry;

    }
}