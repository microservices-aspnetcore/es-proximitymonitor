using Microsoft.Extensions.Logging;
using PubnubApi;


namespace StatlerWaldorfCorp.ProximityMonitor.Realtime
{
    public class PubnubRealtimePublisher : IRealtimePublisher
    {
        private ILogger logger;

        private Pubnub pubnubClient;

        public PubnubRealtimePublisher(
            ILogger<PubnubRealtimePublisher> logger,
            Pubnub pubnubClient)
        {
            logger.LogInformation("Realtime Publisher (Pubnub) Created.");
            this.logger = logger;
            this.pubnubClient = pubnubClient;            
        }

        public void Validate()        
        { 
            pubnubClient.Time()
                .Async(new PNTimeResultExt(
                (result, status) => {
                    if (status.Error) {
                        logger.LogError($"Unable to connect to Pubnub {status.ErrorData.Information}");
                        throw status.ErrorData.Throwable;
                    } else {
                        logger.LogInformation("Pubnub connection established.");
                    }
                }
            ));        

        }

        public void Publish(string channelName, string message)
        {            
            pubnubClient.Publish()
                .Channel(channelName)
                .Message(message)
                .Async(new PNPublishResultExt(
                    (result, status) => {
                        if (status.Error) {
                            logger.LogError($"Failed to publish on channel {channelName}: {status.ErrorData.Information}");
                        } else {
                            logger.LogInformation($"Published message on channel {channelName}, {status.AffectedChannels.Count} affected channels, code: {status.StatusCode}");
                        }                        
                    }
                ));
        }
    }
}