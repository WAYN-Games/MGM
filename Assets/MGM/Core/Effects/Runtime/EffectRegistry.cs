using Unity.Entities;

namespace Wayn.Mgm.Event.Registry
{

    public class EffectRegistry : Registry<EffectRegistry>
    {
        // This is mandatory to enfore the singleton.
        private EffectRegistry() { }

    }
}

