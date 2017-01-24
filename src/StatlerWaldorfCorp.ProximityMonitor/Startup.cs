using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StatlerWaldorfCorp.ProximityMonitor.Queues;
using Steeltoe.Extensions.Configuration;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using Steeltoe.CloudFoundry.Connector.Rabbit;
using StatlerWaldorfCorp.ProximityMonitor.Realtime;
using RabbitMQ.Client.Events;
using StatlerWaldorfCorp.ProximityMonitor.Events;
using Microsoft.Extensions.Options;
using StatlerWaldorfCorp.ProximityMonitor.TeamService;


namespace StatlerWaldorfCorp.ProximityMonitor
{
    public class Startup
    {        
        public Startup(IHostingEnvironment env, ILoggerFactory loggerFactory) 
        {
            loggerFactory.AddConsole();
            loggerFactory.AddDebug();            
            
            var builder = new ConfigurationBuilder()                
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
		        .AddEnvironmentVariables()		    
                .AddCloudFoundry();

	        Configuration = builder.Build();    		        
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services) 
        {
            services.AddMvc();
            services.AddOptions();            
                        
            services.Configure<CloudFoundryApplicationOptions>(Configuration);
            services.Configure<CloudFoundryServicesOptions>(Configuration);                                   

            services.AddRabbitConnection(Configuration);

            services.Configure<QueueOptions>(Configuration.GetSection("QueueOptions"));
            services.Configure<PubnubOptions>(Configuration.GetSection("PubnubOptions"));

            services.AddTransient(typeof(EventingBasicConsumer), typeof(RabbitMQEventingConsumer));
            services.AddSingleton(typeof(IEventSubscriber), typeof(RabbitMQEventSubscriber));
            services.AddSingleton(typeof(IEventProcessor), typeof(ProximityDetectedEventProcessor));
            services.AddTransient(typeof(ITeamServiceClient),typeof(HttpTeamServiceClient));

            services.AddRealtimeService();
            services.AddSingleton(typeof(IRealtimePublisher), typeof(PubnubRealtimePublisher));            
        }

        // Singletons are lazy instantiation.. so if we don't ask for an instance during startup,
        // they'll never get used.
        public void Configure(IApplicationBuilder app, 
                IHostingEnvironment env, 
                ILoggerFactory loggerFactory,
                IEventProcessor eventProcessor,
                IOptions<PubnubOptions> pubnubOptions,
                IRealtimePublisher realtimePublisher)
        {                     
            realtimePublisher.Validate();
            realtimePublisher.Publish(pubnubOptions.Value.StartupChannel, "{'hello': 'world'}");

            eventProcessor.Start();

            app.UseMvc();            
        }        
    }
}