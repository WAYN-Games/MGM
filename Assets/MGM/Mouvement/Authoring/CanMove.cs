using UnityEngine;
using com.WaynGroup.RBAW;
using Unity.Physics.Authoring;
using Unity.Entities;
using UnityEngine.InputSystem;

[RequireComponent(typeof(MovementDirection), typeof(MovementSpeed), typeof(PhysicsBodyAuthoring))]
[RequireComponent(typeof(GroundInfo))]
[RequireComponent(typeof(AlwaysFaceMovementDirection))]
public class CanMove : RequirementBasedAuthoringComponent
{
    public InputActionReference MouvementInputControls;

    private void OnEnable()
    {
        if (MouvementInputControls == null && GetComponent<ControledByPlayer>() != null)
            Debug.LogError($"{name} is player controlled but miss the Input Action Reference for {GetType().Name} ");

        if (MouvementInputControls != null && GetComponent<ControledByPlayer>() == null)
            Debug.LogWarning($"{name} has an Input Action Reference for {GetType().Name} but miss the ControledByPlayer component. This will result in no action being taken into account.");

        if (MouvementInputControls == null) return;

        World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<MovementInputSystem>().SetInputControls(MouvementInputControls);        
    }
}
