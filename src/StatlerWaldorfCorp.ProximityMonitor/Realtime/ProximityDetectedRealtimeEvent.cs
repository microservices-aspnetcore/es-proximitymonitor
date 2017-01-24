using StatlerWaldorfCorp.ProximityMonitor.Events;

namespace StatlerWaldorfCorp.ProximityMonitor.Realtime
{
    public class ProximityDetectedRealtimeEvent : ProximityDetectedEvent
    {
       public string TeamName { get; set; }
       public string SourceMemberName { get; set; }
       public string TargetMemberName { get; set; } 
    }
}