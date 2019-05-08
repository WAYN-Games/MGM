using Unity.Entities;
using UnityEngine;
using UnityEngine.Experimental.Input;

namespace MGM
{
    [RequiresEntityConversion]
    public abstract class ControledCapability<T>  : MonoBehaviour, IConvertGameObjectToEntity where T : InputResponse
    {

        [SerializeField] private InputActionProperty ActionReference;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            var inputManager = GameObject.Find("InputManager_"+this.name);
            inputManager = inputManager == null ? new GameObject("InputManager_"+this.name) : inputManager;
            inputManager.AddComponent<T>();
            inputManager.GetComponent<T>().Setup(ActionReference.action, entity, dstManager);
            SetUpCapabilityParameters(entity, dstManager, conversionSystem);
        }

        protected abstract void SetUpCapabilityParameters(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem);

    }
}