using Microsoft.AspNetCore.Mvc;

namespace application_insights_setup.Controllers;

[ApiController]
[Route("api/trigger/")]
public class ApplicationInsightsController : ControllerBase
{
    private readonly ILogger<ApplicationInsightsController> _logger;
    
    public ApplicationInsightsController(ILogger<ApplicationInsightsController> logger)
    {
        _logger = logger;
    }
    
    /// <summary>
    /// Triggers a log trace in application insights
    /// </summary>
    [HttpPost("log")]
    public ActionResult ProduceLog()
    {
        //TODO dynamically pass different log levels instead of just info
        _logger.LogInformation("Log triggered");

        return Ok("Log triggered with the verbiage");
    }

    /// <summary>
    /// Triggers an exception in application insights
    /// </summary>
    [HttpPost("exception")] 
    public void ProduceException()
    {
        throw new Exception("Exception triggered");
    }
    
    /// <summary>
    /// Triggers an alert in application insights
    /// </summary>
    [HttpPost("alert")] 
    public ActionResult ProduceAlert()
    {
        // Note: This requires an alert on the specific verbiage below to be created in Azure Devops for the service that the telemetry key is registered
        _logger.LogInformation("This verbiage triggers an alert");

        return Ok("Alert triggered");
    }
}