using UnityEditor;
using Wayn.Mgm.Effects;


[CustomEditor(typeof(EffectsBufferAuthoring<>), true)]
public class EffectsBufferAuthoringEditor : RegistryReferenceBufferAuthoringEditor<IEffect,EffectAuthoring>
{

}
