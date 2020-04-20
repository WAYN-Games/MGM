using Unity.Entities;

[GenerateAuthoringComponent]
public struct OwnershipPoint : IPool
{
    public float _value;
    public float _maxValue;
    public float Value { get => _value; set => _value = value; }
    public float MaxValue { get => _maxValue; set => _maxValue = value; }
}
