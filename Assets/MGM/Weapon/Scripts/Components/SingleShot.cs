using System;
using Unity.Entities;
namespace MGM.Weapon
{
    [Serializable]
    [WriteGroup(typeof(Shot))]
    public struct SingleShot : IComponentData
    {
        // No particullar data requiered for the single shot mechanique. Just a component to allow the overriding of the base shot system.
    }
}

