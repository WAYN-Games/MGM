using Unity.Entities;

[UpdateAfter(typeof(InputSystemGroup))]
public class MovementSystemGroup : ComponentSystemGroup
{ }