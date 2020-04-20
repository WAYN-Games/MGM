using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

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
