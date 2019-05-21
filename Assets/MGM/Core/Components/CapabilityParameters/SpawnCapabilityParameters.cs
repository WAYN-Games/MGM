using System;
using Unity.Entities;
using UnityEngine;

namespace MGM {
    [Serializable]
    public struct SpawnCapabilityParameters : IComponentData
    {
        public Entity Spawnable;
        public float CoolDown;
        public float TimeSinceLastTrigger;
    }

    [Serializable]
    public struct ShootingCapabilityParameters : IComponentData
    {
        public SpawnCapabilityParameters spawnCapabilityParameters;
        public float Speed;
    }
    [Serializable]
    public struct ShotTrigger : IComponentData
    {
        public bool IsTriggered;
    }

}
