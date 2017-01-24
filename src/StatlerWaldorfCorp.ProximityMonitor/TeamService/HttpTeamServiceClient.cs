using System;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using System.Linq;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace StatlerWaldorfCorp.ProximityMonitor.TeamService
{
    public class HttpTeamServiceClient : ITeamServiceClient
    {
        private readonly Service teamServiceBinding;

        private readonly ILogger logger;

        private HttpClient httpClient;
        
        public HttpTeamServiceClient(ILogger<HttpTeamServiceClient> logger,
            IOptions<CloudFoundryServicesOptions> cfOptions)
        {
            this.logger = logger;               
            this.teamServiceBinding = cfOptions.Value.Services.FirstOrDefault( s => s.Name == "teamservice");
            logger.LogInformation("Team Service HTTP client using URL {0}",
                teamServiceBinding.Credentials["url"].Value);

            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(teamServiceBinding.Credentials["url"].Value);
        }

        public Team GetTeam(Guid teamId)
        {
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = httpClient.GetAsync(String.Format("/teams/{0}", teamId)).Result;

            Team teamResponse = null;
            if (response.IsSuccessStatusCode) {
                string json = response.Content.ReadAsStringAsync().Result;
                teamResponse = JsonConvert.DeserializeObject<Team>(json);                
            }
            return teamResponse;
        }

        public Member GetMember(Guid teamId, Guid memberId)
        {
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = httpClient.GetAsync(String.Format("/teams/{0}/members/{1}", teamId, memberId)).Result;

            Member memberResponse = null;
            if (response.IsSuccessStatusCode) {
                string json = response.Content.ReadAsStringAsync().Result;
                memberResponse = JsonConvert.DeserializeObject<Member>(json);
            }
            return memberResponse;
        }
    }
}