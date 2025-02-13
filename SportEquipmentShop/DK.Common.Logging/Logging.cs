using Elastic.Serilog.Sinks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog.Events;
using Serilog;
using Serilog.Exceptions;
using Elastic.Ingest.Elasticsearch;
using Elastic.Channels;
using Elastic.Ingest.Elasticsearch.DataStreams;

namespace DK.Common.Logging
{
    public static class Logging
    {
        public static Action<HostBuilderContext, LoggerConfiguration> ConfigureLogger =>
            (context, loggerConfiguration) =>
            {
                var env = context.HostingEnvironment;
                loggerConfiguration.MinimumLevel.Information()
                    .Enrich.FromLogContext()
                    .Enrich.WithProperty("ApplicationName", env.ApplicationName)
                    .Enrich.WithProperty("EnvironmentName", env.EnvironmentName)
                    .Enrich.WithExceptionDetails()
                    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                    .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                    .WriteTo.Console();
                if (context.HostingEnvironment.IsDevelopment())
                {
                    loggerConfiguration.MinimumLevel.Override("Catalog", LogEventLevel.Debug);
                    loggerConfiguration.MinimumLevel.Override("Basket", LogEventLevel.Debug);
                    loggerConfiguration.MinimumLevel.Override("Discount", LogEventLevel.Debug);
                    loggerConfiguration.MinimumLevel.Override("Ordering", LogEventLevel.Debug);
                }

                var elasticUrl = context.Configuration.GetValue<string>("ElasticConfiguration:Uri");
                if (!string.IsNullOrEmpty(elasticUrl))
                {
                    loggerConfiguration.WriteTo.Elasticsearch(new[] { new Uri(elasticUrl) }, opts =>
                        {

                            opts.DataStream = new DataStreamName("logs", "console-example", "demo");
                            opts.BootstrapMethod = BootstrapMethod.Failure;
                            opts.ConfigureChannel = channelOpts =>
                            {
                                channelOpts.BufferOptions = new BufferOptions
                                {
                                    ExportMaxConcurrency = 10
                                };
                            };
                        }, transport =>
                        {
                            // transport.Authentication(new BasicAuthentication(username, password)); // Basic Auth
                            // transport.Authentication(new ApiKey(base64EncodedApiKey)); // ApiKey
                        });
                }
            };
    }
}
