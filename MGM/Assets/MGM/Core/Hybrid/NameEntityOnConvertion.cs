using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
namespace MGM
{
    /// <summary>
    /// This allow to automaticaly rename the entity wit the name of the game object on convertion.
    /// </summary>
    public class NameEntityOnConvertion : MonoBehaviour, IConvertGameObjectToEntity
    {
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            #if UNITY_EDITOR
            dstManager.SetName(entity, name);
            #endif
        }
    }
}
