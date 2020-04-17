using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct Health : IComponentData, IPool
{
    public float _value;
    public float _maxValue;
    public float Value { get => _value; set => _value = value; }
    public float MaxValue { get => _maxValue; set => _maxValue = value; }
}

public static class PoolMethods
{
    [BurstCompile]
    public static T SubtractValue<T>(T component, float amount) where T : struct, IComponentData, IPool
    {
        component.Value -= amount;
        return component;
    }

    [BurstCompile]
    public static T AddValue<T>(T component, float amount) where T : struct, IComponentData, IPool
    {
        component.Value = math.clamp(component.Value + amount, 0, component.MaxValue);
        return component;
    }

    [BurstCompile]
    public static T ResetValue<T>(T component, float amount) where T : struct, IComponentData, IPool
    {
        component.Value = component.MaxValue;
        return component;
    }
}

public interface IPool
{
    float Value { get; set; }
    float MaxValue { get; set; }


}