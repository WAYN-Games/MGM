using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
namespace MGM.Core
{

    public class SoundFXManager
    {
        public static List<AudioClip> SoundList = new List<AudioClip>();
    }
    public struct SoundFX : IComponentData
    {
        public int Index;
        public Vector3 EmmiterPosition;
        public bool Play;
        public float Volume;

        public void PlaySFXAt(Vector3 position)
        {
            EmmiterPosition = position;
            Play = true;
        }
    }
}
