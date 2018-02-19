using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;

namespace Axoom.MyService
{
    /// <summary>
    /// Provides error handling and retry policies.
    /// </summary>
    public interface IPolicies
    {
        /// <summary>
        /// Policy for handling connection problems with external services during startup.
        /// </summary>
        Task StartupAsync(Func<Task> action);
    }

    /// <summary>
    /// Provides error handling and retry policies.
    /// </summary>
    public class Policies : IPolicies
    {
        private readonly IOptions<PolicyOptions> _options;
        private readonly ILogger<Policies> _logger;

        public Policies(IOptions<PolicyOptions> options, ILogger<Policies> logger)
        {
            _options = options;
            _logger = logger;
        }

        /// <inheritdoc />
        public Task StartupAsync(Func<Task> action) => Policy
            .Handle<HttpRequestException>()
            .WaitAndRetryAsync(
                sleepDurations: _options.Value.StartupRetries,
                onRetry: (ex, timeSpan) => _logger.LogWarning($"Problem connecting to external service; retrying in {timeSpan}. ({ex.GetType().Name}: {ex.Message})"))
            .ExecuteAsync(action);
    }

    /// <summary>
    /// Options for <see cref="Policies" />.
    /// </summary>
    public class PolicyOptions
    {
        /// <summary>
        /// The delays between subsequent retry attemps at startup.
        /// </summary>
        public ICollection<TimeSpan> StartupRetries { get; } = new List<TimeSpan>();
    }

    public static class PolicyExtensions
    {
        public static IServiceCollection AddPolicies(this IServiceCollection services, IConfiguration config) => services
            .Configure<PolicyOptions>(config)
            .AddSingleton<IPolicies, Policies>();
    }
}