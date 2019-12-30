using UnityEngine;
using com.WaynGroup.RBAW;
using Unity.Entities;
using UnityEngine.InputSystem;

[RequireComponent(typeof(AimPosition))]
public class CanAim : RequirementBasedAuthoringComponent
{
    public InputActionReference AimInputControls;

    private void OnEnable()
    {
        if (AimInputControls == null && GetComponent<ControledByPlayer>() != null)
            Debug.LogError($"{name} is player controlled but miss the Input Action Reference for {GetType().Name} ");

        if (AimInputControls != null && GetComponent<ControledByPlayer>() == null)
            Debug.LogWarning($"{name} has an Input Action Reference for {GetType().Name} but miss the ControledByPlayer component. This will result in no action being taken into account.");

        if (AimInputControls == null) return;

        World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<AimInputSystem>().SetInputControls(AimInputControls);        
    }
}
