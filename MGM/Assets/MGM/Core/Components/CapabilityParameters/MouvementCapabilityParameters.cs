using System;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace MGM {
    [Serializable]
    public struct MouvementCapabilityParameters : IComponentData
    {
        public float Speed;
        public bool ShouldFaceForward;
        public float MovementInertia;
    }
}
