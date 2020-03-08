using Unity.Entities;
namespace Wayn.Mgm.Effects
{
    public interface IEffect
    {
        Entity Emmiter { get; set; }
        Entity Other { get; set; }
    }
}