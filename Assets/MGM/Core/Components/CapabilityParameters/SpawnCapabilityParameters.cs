using System;
using Unity.Entities;

namespace MGM {
    


    [Serializable]
    public struct Shot : IComponentData
    {
        public Entity Projectile;
        public float Speed;
        public Trigger Trigger;
    }


    [Serializable]
    public struct Trigger : IComponentData
    {
        public bool IsTriggered;
        public float CoolDown;
        public float TimeSinceLastTrigger;
    }

}
