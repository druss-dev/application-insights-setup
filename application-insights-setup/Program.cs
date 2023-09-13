using System.Text.Json.Serialization;
using application_insights_setup.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen();
builder.AddConfigurationOptions();
builder.Services.AddControllers()
    .AddJsonOptions(options=>
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.AddLogging();

var application = builder.Build();

if (application.Environment.IsDevelopment())
{
    application.UseSwagger();
    application.UseSwaggerUI();
}

application.UseHttpsRedirection();
application.UseRouting();
application.MapControllers();

await application.RunAsync();
