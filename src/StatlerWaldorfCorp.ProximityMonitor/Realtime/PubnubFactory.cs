using System;
using Microsoft.Extensions.Options;
using PubnubApi;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace StatlerWaldorfCorp.ProximityMonitor.Realtime
{
    public class PubnubFactory
    {     
        private PNConfiguration pnConfiguration;

        private ILogger logger;
        
        public PubnubFactory(IOptions<PubnubOptions> pubnubOptions,
            ILogger<PubnubFactory> logger)
        {
            this.logger = logger;
            
            pnConfiguration = new PNConfiguration();
            pnConfiguration.PublishKey = pubnubOptions.Value.PublishKey;
            pnConfiguration.SubscribeKey = pubnubOptions.Value.SubscribeKey;
            pnConfiguration.Secure = false;

            logger.LogInformation($"Pubnub Factory using publish key {pnConfiguration.PublishKey}");
        }

        public Pubnub CreateInstance()
        {
            return new Pubnub(pnConfiguration);    
        }
    }
}