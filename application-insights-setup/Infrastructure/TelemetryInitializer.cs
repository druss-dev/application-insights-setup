using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;

namespace application_insights_setup.Infrastructure;


public class TelemetryInitializer : ITelemetryInitializer
{
    private readonly string? _applicationName;

    public TelemetryInitializer(string? applicationName)
    {
        _applicationName = applicationName;
    }

    public void Initialize(ITelemetry telemetry)
    {
        if(!telemetry.Context.GlobalProperties.ContainsKey("app"))
            telemetry.Context.GlobalProperties.Add("app", _applicationName);
    }
}