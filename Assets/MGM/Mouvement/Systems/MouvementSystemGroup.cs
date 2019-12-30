using Unity.Entities;
using Unity.Transforms;

[UpdateAfter(typeof(InputSystemGroup))]
public class MouvementSystemGroup : ComponentSystemGroup
{ }