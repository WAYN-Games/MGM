using System;
using Unity.Entities;

namespace MGM.Common
{

    [Serializable]
    public struct Trigger : IComponentData
    {
        public bool IsTriggered; // True if the player want to trigger the action.
        public float CoolDown; // The time between two action trigger.
        public float TimeSinceLastTrigger; // The time elapsed since the last trigger was effectively triggered. 
    }
}
