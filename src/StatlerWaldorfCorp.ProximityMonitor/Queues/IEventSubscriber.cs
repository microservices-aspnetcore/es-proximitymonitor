using StatlerWaldorfCorp.ProximityMonitor.Events;

namespace StatlerWaldorfCorp.ProximityMonitor.Queues
{
    public delegate void ProximityDetectedEventReceivedDelegate(ProximityDetectedEvent evt);

    public interface IEventSubscriber
    {
        void Subscribe();
        void Unsubscribe();
        
        event ProximityDetectedEventReceivedDelegate ProximityDetectedEventReceived;
    }
}