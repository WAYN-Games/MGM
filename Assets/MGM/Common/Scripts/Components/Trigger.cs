using System;
using Unity.Entities;

namespace MGM.Common
{

    [Serializable]
    public struct Trigger : IComponentData
    {
        public bool Triggered; // True if the player want to trigger the action.
        public float CoolDown; // The time between two action trigger.
        public float TimeSinceLastTrigger; // The time elapsed since the last trigger was effectively triggered. 

        public bool IsTriggered(float DeltaTime)
        {
            // Increase the cool down count
            TimeSinceLastTrigger += DeltaTime;

            // Spawn object only when requested
            if (!Triggered) return false;

            // Shoot only if cooled down
            if (TimeSinceLastTrigger < CoolDown) return false;
            // Reset the cool down count
            TimeSinceLastTrigger = 0;
            return true;
        }
    }
}
