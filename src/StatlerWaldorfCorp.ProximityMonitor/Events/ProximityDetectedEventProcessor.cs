using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StatlerWaldorfCorp.ProximityMonitor.Queues;
using StatlerWaldorfCorp.ProximityMonitor.Realtime;
using StatlerWaldorfCorp.ProximityMonitor.TeamService;

namespace StatlerWaldorfCorp.ProximityMonitor.Events
{
    public class ProximityDetectedEventProcessor : IEventProcessor
    {
        private ILogger logger;
        private IRealtimePublisher publisher;
        private IEventSubscriber subscriber;

        private PubnubOptions pubnubOptions;

        public ProximityDetectedEventProcessor(
            ILogger<ProximityDetectedEventProcessor> logger,
            IRealtimePublisher publisher,
            IEventSubscriber subscriber,
            ITeamServiceClient teamClient,
            IOptions<PubnubOptions> pubnubOptions)
        {
            this.logger = logger;
            this.pubnubOptions = pubnubOptions.Value;
            this.publisher = publisher;
            this.subscriber = subscriber;            

            logger.LogInformation("Created Proximity Event Processor.");        

            subscriber.ProximityDetectedEventReceived += (pde) => {
                Team t = teamClient.GetTeam(pde.TeamID);
                Member sourceMember = teamClient.GetMember(pde.TeamID, pde.SourceMemberID);
                Member targetMember = teamClient.GetMember(pde.TeamID, pde.TargetMemberID);

                ProximityDetectedRealtimeEvent outEvent = new ProximityDetectedRealtimeEvent 
                {
                    TargetMemberID = pde.TargetMemberID,
                    SourceMemberID = pde.SourceMemberID,
                    DetectionTime = pde.DetectionTime,                    
                    SourceMemberLocation = pde.SourceMemberLocation,
                    TargetMemberLocation = pde.TargetMemberLocation,
                    MemberDistance = pde.MemberDistance,
                    TeamID = pde.TeamID,
                    TeamName = t.Name,
                    SourceMemberName = $"{sourceMember.FirstName} {sourceMember.LastName}",
                    TargetMemberName = $"{targetMember.FirstName} {targetMember.LastName}"
                };
                publisher.Publish(this.pubnubOptions.ProximityEventChannel, outEvent.toJson());
            };            
        }    
        
        public void Start()
        {
            subscriber.Subscribe();
        }

        public void Stop()
        {
            subscriber.Unsubscribe();
        }
    }
}