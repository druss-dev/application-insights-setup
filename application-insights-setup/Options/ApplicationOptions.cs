using System.ComponentModel.DataAnnotations;

namespace application_insights_setup.Options;

public class ApplicationOptions
{
    public string? APPLICATIONINSIGHTS_CONNECTION_STRING { get; set; }
    
    public string? Name { get; set; }
    
    [Required(AllowEmptyStrings = false)]
    [ConfigurationKeyName("ASPNETCORE_ENVIRONMENT")]
    public string? Environment { get; set; }
}