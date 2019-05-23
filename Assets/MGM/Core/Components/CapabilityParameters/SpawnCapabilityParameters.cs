using System;
using Unity.Entities;

namespace MGM {
    
    /// <summary>
    /// Minimal data required by the shot mechanic.
    /// </summary>
    [Serializable]
    public struct Shot : IComponentData
    {
        public Entity Projectile; // The projectile to be shot.
        public float Speed; // The speed at which the projectile will move
        public Trigger Trigger; // The trigger data to know when to shoot.
    }


    [Serializable]
    public struct Magazine : IComponentData
    {
        public int MaxCapacity;
        public int CurrentCapacity;
    }


    [Serializable]
    public struct Trigger : IComponentData
    {
        public bool IsTriggered; // True if the player want to trigger the action.
        public float CoolDown; // The time between two action trigger.
        public float TimeSinceLastTrigger; // The time elapsed since the last trigger was effectively triggered. 
    }

}
