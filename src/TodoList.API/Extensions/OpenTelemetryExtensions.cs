using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Logs;
using OpenTelemetry.Trace;
using System;
using System.Diagnostics;

namespace TodoList.API.Extensions;

public static class OpenTelemetryExtensions
{
    // Define constant for the application name
    public const string ServiceName = "TodoList.API";
    
    // Register our activity sources
    private static readonly string[] _activitySources = new[]
    {
        ServiceName
    };

    /// <summary>
    /// Adds OpenTelemetry to the application (traces, metrics, and logging)
    /// </summary>
    public static IServiceCollection AddOpenTelemetryServices(this IServiceCollection services, IConfiguration configuration)
    {
        var serviceName = configuration["OpenTelemetry:ServiceName"] ?? ServiceName;
        var serviceVersion = configuration["OpenTelemetry:ServiceVersion"] ?? "1.0.0";

        // Get OTLP endpoint from environment variable or configuration
        var otlpEndpoint = Environment.GetEnvironmentVariable("OTEL_EXPORTER_OTLP_ENDPOINT") 
            ?? configuration["OpenTelemetry:OtlpEndpoint"] 
            ?? "http://localhost:4317";

        // Configure resource
        var resourceBuilder = ResourceBuilder.CreateDefault()
            .AddService(serviceName: serviceName, serviceVersion: serviceVersion)
            .AddTelemetrySdk()
            .AddEnvironmentVariableDetector();

        // Configure traces
        services.AddOpenTelemetry()
            .WithTracing(builder => builder
                .SetResourceBuilder(resourceBuilder)
                .AddSource(_activitySources) // Register our custom activity sources
                .AddAspNetCoreInstrumentation(options =>
                {
                    options.RecordException = true;
                    options.EnrichWithHttpRequest = (activity, request) =>
                    {
                        activity.SetTag("http.request.headers.host", request.Host.ToString());
                    };
                    options.EnrichWithHttpResponse = (activity, response) =>
                    {
                        activity.SetTag("http.response.headers.contenttype", response.ContentType);
                    };
                })
                .AddHttpClientInstrumentation(options =>
                {
                    options.RecordException = true;
                    options.EnrichWithException = (activity, exception) =>
                    {
                        activity.SetTag("error.type", exception.GetType().Name);
                        activity.SetTag("error.message", exception.Message);
                    };
                })
                .AddEntityFrameworkCoreInstrumentation(options =>
                {
                    options.SetDbStatementForText = true;
                    options.SetDbStatementForStoredProcedure = true;
                })
                .AddSqlClientInstrumentation(options =>
                {
                    options.RecordException = true;
                    options.SetDbStatementForText = true;
                    options.EnableConnectionLevelAttributes = true;
                })
                .AddConsoleExporter()
                .AddOtlpExporter(options =>
                {
                    options.Endpoint = new Uri(otlpEndpoint);
                }))
            .WithMetrics(builder => builder
                .SetResourceBuilder(resourceBuilder)
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddRuntimeInstrumentation()
                .AddConsoleExporter()
                .AddOtlpExporter(options =>
                {
                    options.Endpoint = new Uri(otlpEndpoint);
                }));

        return services;
    }

    /// <summary>
    /// Adds OpenTelemetry logging configuration
    /// </summary>
    public static ILoggingBuilder AddOpenTelemetryLogging(this ILoggingBuilder builder, IConfiguration configuration)
    {
        var serviceName = configuration["OpenTelemetry:ServiceName"] ?? ServiceName;
        var serviceVersion = configuration["OpenTelemetry:ServiceVersion"] ?? "1.0.0";

        // Get OTLP endpoint from environment variable or configuration
        var otlpEndpoint = Environment.GetEnvironmentVariable("OTEL_EXPORTER_OTLP_ENDPOINT") 
            ?? configuration["OpenTelemetry:OtlpEndpoint"] 
            ?? "http://localhost:4317";

        // Clear existing providers
        builder.ClearProviders();
        
        // Configure OpenTelemetry for logging
        builder.AddOpenTelemetry(options =>
        {
            options.SetResourceBuilder(
                ResourceBuilder.CreateDefault()
                    .AddService(serviceName: serviceName, serviceVersion: serviceVersion)
                    .AddTelemetrySdk()
                    .AddEnvironmentVariableDetector());
                    
            options.IncludeFormattedMessage = true;
            options.IncludeScopes = true;
            
            // Add console exporter for local development
            options.AddConsoleExporter();
            
            // Add OTLP exporter for production/integration with other systems
            options.AddOtlpExporter(otlpOptions =>
            {
                otlpOptions.Endpoint = new Uri(otlpEndpoint);
            });
        });

        return builder;
    }
} 