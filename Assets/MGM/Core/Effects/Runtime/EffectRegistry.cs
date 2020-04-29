using Unity.Entities;

namespace Wayn.Mgm.Events.Registry
{

    public class EffectRegistry : Registry<EffectRegistry, IEffect>
    {
        // This is mandatory to enfore the singleton.
        private EffectRegistry() { }

        // Declare the event and delegate.
        public delegate void NewEffectRegisteredHandler();
        public event NewEffectRegisteredHandler NewEffectRegisteredEvent;

        protected override void OnNewElementRegistered()
        {   
            NewEffectRegisteredEvent?.Invoke();
        }

  

    }
}

