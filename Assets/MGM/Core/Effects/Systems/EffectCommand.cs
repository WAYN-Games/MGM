using Unity.Entities;

namespace Wayn.Mgm.Effects
{
    public struct EffectCommand
    {
        public Entity Emitter;
        public Entity Target;
        public EffectReference EffectReference;
    }
}
