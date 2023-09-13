using application_insights_setup.Infrastructure;
using application_insights_setup.Options;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using TelemetryConverter = application_insights_setup.Infrastructure.TelemetryConverter;

namespace application_insights_setup.Extensions;

public static class ProgramExtensions
{
    /// <summary>
    /// adds configuration options to the host container
    /// </summary>
    public static void AddConfigurationOptions(this WebApplicationBuilder builder)
    {
        builder.Services.AddOptions<ApplicationOptions>()
            .Bind(builder.Configuration)
            .ValidateDataAnnotations()
            .ValidateOnStart();
    }
    
     /// <summary>
    /// adds logging configuration to the host container
    /// </summary>
    public static void AddLogging(this WebApplicationBuilder builder)
    {
        builder.Services.AddApplicationInsightsTelemetry();
        builder.Services.AddSingleton<ITelemetryInitializer>(provider =>
        {
            var applicationOptions = provider.GetRequiredService<IOptions<ApplicationOptions>>().Value;
            return new TelemetryInitializer(applicationOptions.Name);
        });
        
        builder.Host
            .UseSerilog((_, provider, loggerConfiguration) =>
            {
                // remove noisy logs
                loggerConfiguration
                    .MinimumLevel
                    .Override("Microsoft.AspNetCore", LogEventLevel.Warning);
                
                // add enrichers
                loggerConfiguration
                    .Enrich
                    .FromLogContext()
                    .Enrich.WithThreadId()
                    .Enrich.WithMachineName()
                    .Enrich.WithCorrelationId();
                
                // application insights logger
                loggerConfiguration
                    .WriteTo
                    .ApplicationInsights(provider.GetRequiredService<TelemetryConfiguration>(),
                        new TelemetryConverter(),
                        LogEventLevel.Information);
            
                // console logger
                loggerConfiguration
                    .WriteTo
                    .Console(theme: AnsiConsoleTheme.Code);
                
            });
    }
}