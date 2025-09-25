using System;

using Paramore.Brighter.Observability;
using Paramore.Brighter.Outbox.Hosting;

namespace Fluent.Brighter;

/// <summary>
/// A fluent builder for configuring <see cref="TimedOutboxArchiverOptions"/>
/// </summary>
/// <remarks>
/// Provides a convenient way to configure outbox archiving parameters using a fluent interface.
/// 
/// Example usage:
/// <code>
/// var options = new TimedOutboxArchiverOptionsBuilder()
///     .SetTimerInterval(30)
///     .SetMinimumAge(TimeSpan.FromHours(48))
///     .SetArchiveBatchSize(500)
///     .SetInstrumentation(InstrumentationOptions.Sampled)
///     .Build();
/// </code>
/// </remarks>
public class TimedOutboxArchiverOptionsBuilder
{
    private Action<TimedOutboxArchiverOptions> _options = _ => { };

    /// <summary>
    /// Configures the interval at which the archiver runs
    /// </summary>
    /// <param name="seconds">The interval in seconds between archiver executions</param>
    /// <returns>The builder instance for method chaining</returns>
    public TimedOutboxArchiverOptionsBuilder SetTimerInterval(int seconds)
    {
        _options += opt => opt.TimerInterval = seconds;
        return this;
    }

    /// <summary>
    /// Configures the minimum age a message must be before it's eligible for archiving
    /// </summary>
    /// <param name="minimumAge">The minimum age threshold as a TimeSpan</param>
    /// <returns>The builder instance for method chaining</returns>
    /// <remarks>
    /// Messages younger than this value will not be archived
    /// </remarks>
    public TimedOutboxArchiverOptionsBuilder SetMinimumAge(TimeSpan minimumAge)
    {
        _options += opt => opt.MinimumAge = minimumAge;
        return this;
    }

    /// <summary>
    /// Configures the minimum age using hours for convenience
    /// </summary>
    /// <param name="hours">The minimum age in hours</param>
    /// <returns>The builder instance for method chaining</returns>
    public TimedOutboxArchiverOptionsBuilder SetMinimumAgeInHours(int hours)
    {
        _options += opt => opt.MinimumAge = TimeSpan.FromHours(hours);
        return this;
    }

    /// <summary>
    /// Configures the number of messages to process in each archiving batch
    /// </summary>
    /// <param name="batchSize">The number of messages per archive batch</param>
    /// <returns>The builder instance for method chaining</returns>
    public TimedOutboxArchiverOptionsBuilder SetArchiveBatchSize(int batchSize)
    {
        _options += opt => opt.ArchiveBatchSize = batchSize;
        return this;
    }

    /// <summary>
    /// Configures instrumentation options for archiving operations
    /// </summary>
    /// <param name="instrumentation">The instrumentation level to apply</param>
    /// <returns>The builder instance for method chaining</returns>
    public TimedOutboxArchiverOptionsBuilder SetInstrumentation(InstrumentationOptions instrumentation)
    {
        _options += opt => opt.Instrumentation = instrumentation;
        return this;
    }

    /// <summary>
    /// Builds the configuration action
    /// </summary>
    /// <returns>An action that will apply all configured options to a TimedOutboxArchiverOptions instance</returns>
    public Action<TimedOutboxArchiverOptions> Build()
    {
        return _options;
    }
}