using Microsoft.ApplicationInsights.Channel;
using Serilog.Events;
using Serilog.Sinks.ApplicationInsights.TelemetryConverters;

namespace application_insights_setup.Infrastructure;

public class TelemetryConverter : TraceTelemetryConverter
{
    // Array of key properties
    private static readonly string[] GlobalPropertyKeys =
    {
        //TODO add a log example of a key property being displayed for demo purposes
        "YourKeyProperty"
    };

    /// <summary>
    /// If a log event contains a key property, we write that value to the global telemetry
    /// so that all subsequent traces have that value
    /// </summary>
    public override IEnumerable<ITelemetry> Convert(LogEvent logEvent, IFormatProvider formatProvider)
    {
        foreach (var telemetry in base.Convert(logEvent, formatProvider))
        {
            foreach (var propertyKey in GlobalPropertyKeys)
            {
                if (logEvent.Properties.ContainsKey(propertyKey))
                {
                    telemetry.Context.GlobalProperties.Add(propertyKey, logEvent.Properties[propertyKey].ToString());
                }
            }
        }
        
        return base.Convert(logEvent, formatProvider);
    }
}