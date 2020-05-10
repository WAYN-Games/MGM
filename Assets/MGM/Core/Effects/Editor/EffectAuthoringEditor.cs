using UnityEditor;
using Wayn.Mgm.Events;
using Wayn.Mgm.Events.Registry;

[CustomEditor(typeof(EffectsBufferAuthoring<>), true)]
public class EffectsBufferAuthoringEditor : RegistryReferenceBufferAuthoringEditor<IEffect,EffectAuthoring>
{
      
}
