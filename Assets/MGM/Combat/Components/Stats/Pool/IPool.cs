using Unity.Entities;

public interface IPool : IComponentData
{
    float Value { get; set; }
    float MaxValue { get; set; }


}