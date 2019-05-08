using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using static Unity.Entities.ConvertToEntity;

namespace MGM
{

    public class CopyTranslationData : MonoBehaviour, IConvertGameObjectToEntity
    {
        private Entity _entityTarget;
        private EntityManager _dstManager;
      
        private void Awake()
        {
            if (GetComponent<ConvertToEntity>() == null) gameObject.AddComponent(typeof(ConvertToEntity));
            GetComponent<ConvertToEntity>().ConversionMode = Mode.ConvertAndInjectGameObject;
        }

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            _entityTarget = entity;
            _dstManager = dstManager;
                
        }

        private void Update()
        {
            float3 lookAtTarget = _dstManager.GetComponentData<Translation>(_entityTarget).Value;
            Vector3 entityTargetPosition = new Vector3(lookAtTarget.x, lookAtTarget.y, lookAtTarget.z);
         
             transform.position = entityTargetPosition;

        }

    }
}
