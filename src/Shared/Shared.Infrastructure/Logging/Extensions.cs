using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Filters;
using Shared.Infrastructure.Logging.Options;

namespace Shared.Infrastructure.Logging
{
    public static class Extensions
    {
        private const string ConsoleOutputTemplate = "{Timestamp:HH:mm:ss} [{Level:u3}] {Message}{NewLine}{Exception}";

        private const string FileOutputTemplate =
            "{Timestamp:HH:mm:ss} [{Level:u3}] ({SourceContext}.{Method}) {Message}{NewLine}{Exception}";

        public static IServiceCollection AddLoggingDecorators(this IServiceCollection services)
        {
            // services.TryDecorate(typeof(ICommandHandler<>), typeof(LoggingCommandHandlerDecorator<>));
            // services.TryDecorate(typeof(IEventHandler<>), typeof(LoggingEventHandlerDecorator<>));
            // services.TryDecorate(typeof(IQueryHandler<,>), typeof(LoggingQueryHandlerDecorator<,>));
            return services;
        }

        public static IApplicationBuilder UseLogging(this IApplicationBuilder app)
        {
            app.Use(async (ctx, next) =>
            {
                // var logger = ctx.RequestServices.GetRequiredService<ILogger<IContext>>();
                // var context = ctx.RequestServices.GetRequiredService<IContext>();
                // logger.LogInformation("Started processing a request [Request ID: '{RequestId}', Correlation ID: '{CorrelationId}', Trace ID: '{TraceId}', User ID: '{UserId}']...",
                //     context.RequestId, context.CorrelationId, context.TraceId, context.Identity.IsAuthenticated ? context.Identity.Id : String.Empty);
                //
                await next();
                //
                // logger.LogInformation("Finished processing a request with status code: {StatusCode} [Request ID: '{RequestId}', Correlation ID: '{CorrelationId}', Trace ID: '{TraceId}', User ID: '{UserId}']",
                // ctx.Response.StatusCode, context.RequestId, context.CorrelationId, context.TraceId, context.Identity.IsAuthenticated ? context.Identity.Id : String.Empty);
            });

            return app;
        }

        public static IHostBuilder ConfigureLogging(this IHostBuilder builder,
            Action<LoggerConfiguration>? configure = null)
            => builder.UseSerilog((context, loggerConfiguration) =>
            {
                var appOptions = context.Configuration.GetOptions<AppSettings>(nameof(AppSettings));
                var loggerOptions = context.Configuration.GetOptions<LoggerSettings>(nameof(LoggerSettings));
                MapOptions(loggerOptions, appOptions, loggerConfiguration, context.HostingEnvironment.EnvironmentName);
                configure?.Invoke(loggerConfiguration);
                if (loggerOptions.Console is null) return;
                if (loggerOptions.Console.Enabled && !string.IsNullOrWhiteSpace(appOptions.Name))
                {
                    Console.WriteLine(Figgle.FiggleFonts.Doom.Render($"{appOptions.Name} {appOptions.Version}"));
                }

            });

        private static void MapOptions(LoggerSettings loggerSettings, AppSettings appSettings,
            LoggerConfiguration loggerConfiguration, string environmentName)
        {
            var level = GetLogEventLevel(loggerSettings.Level);

            loggerConfiguration.Enrich.FromLogContext()
                .MinimumLevel.Is(level)
                .Enrich.WithProperty("Environment", environmentName)
                .Enrich.WithProperty("Application", appSettings.Name)
                .Enrich.WithProperty("Instance", appSettings.Instance)
                .Enrich.WithProperty("Version", appSettings.Version);

            foreach (var (key, value) in loggerSettings.Tags ?? new Dictionary<string, object>())
            {
                loggerConfiguration.Enrich.WithProperty(key, value);
            }

            foreach (var (key, value) in loggerSettings.Overrides ?? new Dictionary<string, string>())
            {
                var logLevel = GetLogEventLevel(value);
                loggerConfiguration.MinimumLevel.Override(key, logLevel);
            }

            loggerSettings.ExcludePaths?.ToList().ForEach(p => loggerConfiguration.Filter
                .ByExcluding(Matching.WithProperty<string>("RequestPath", n => n.EndsWith(p))));

            loggerSettings.ExcludeProperties?.ToList().ForEach(p => loggerConfiguration.Filter
                .ByExcluding(Matching.WithProperty(p)));

            Configure(loggerConfiguration, loggerSettings);
        }

        private static void Configure(LoggerConfiguration loggerConfiguration, LoggerSettings settings)
        {
            var consoleOptions = settings.Console ?? new ConsoleOptions();
            var fileOptions = settings.Files ?? new FilesOptions();
            var seqOptions = settings.Seq ?? new SeqOptions();

            if (consoleOptions.Enabled)
            {
                loggerConfiguration.WriteTo.Console(outputTemplate: ConsoleOutputTemplate);
            }

            if (fileOptions.Enabled)
            {
                var path = string.IsNullOrWhiteSpace(fileOptions.Path) ? "logs/logs.txt" : fileOptions.Path;
                if (!Enum.TryParse<RollingInterval>(fileOptions.Interval, true, out var interval))
                {
                    interval = RollingInterval.Day;
                }

                loggerConfiguration.WriteTo.File(path, rollingInterval: interval, outputTemplate: FileOutputTemplate);
            }

            if (seqOptions.Enabled)
            {
                loggerConfiguration.WriteTo.Seq(seqOptions.Url, apiKey: seqOptions.ApiKey);
            }
        }

        private static LogEventLevel GetLogEventLevel(string? level)
            => Enum.TryParse<LogEventLevel>(level, true, out var logLevel)
                ? logLevel
                : LogEventLevel.Information;
    }
}