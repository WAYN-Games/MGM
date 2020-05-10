using System;

namespace Wayn.Mgm.Events.Registry
{
    public interface IRegistry
    {
        RegistryReference AddEffect(IRegistryElement element);
        event EventHandler NewElementRegistered;
        void SubscribeToElementRegisteredEvent(EventHandler method);
        void UnsubscribeToElementRegisteredEvent(EventHandler method);
        void GetRegisteredEffects<T>(ref Unity.Collections.NativeHashMap<int, T> registeredEvents) where T : struct, IRegistryElement;
    }
}

