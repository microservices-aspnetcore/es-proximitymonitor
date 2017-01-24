namespace StatlerWaldorfCorp.ProximityMonitor.Realtime
{
    public class PubnubOptions
    {
        public string PublishKey { get; set; }

        public string SubscribeKey { get; set; }

        public string StartupChannel { get; set; }

        public string ProximityEventChannel { get; set; }
    }
}