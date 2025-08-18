using System;

using Paramore.Brighter.Outbox.Hosting;

namespace Fluent.Brighter;

/// <summary>
/// A fluent builder for configuring <see cref="TimedOutboxSweeperOptions"/>.
/// </summary>
/// <remarks>
/// This builder uses a compositional approach to configure outbox sweeper options.
/// Each configuration method adds an action that will be applied to the options instance during building.
/// 
/// Example usage:
/// <code>
/// var builder = new TimedOutboxSweeperOptionsBuilder()
///     .SetTimerInterval(10)
///     .SetMinimumMessageAge(TimeSpan.FromSeconds(30))
///     .SetBatchSize(200)
///     .SetBulk(true)
///     .SetArg("ConnectionString", "my_connection_string");
/// 
/// Action&lt;TimedOutboxSweeperOptions&gt; configAction = builder.Build();
/// </code>
/// </remarks>
public class TimedOutboxSweeperOptionsBuilder
{
    private Action<TimedOutboxSweeperOptions> _options = _ => { };

    /// <summary>
    /// Configures the interval at which the sweeper runs
    /// </summary>
    /// <param name="timerIntervalSeconds">The interval in seconds between sweeper executions</param>
    /// <returns>The builder instance for method chaining</returns>
    public TimedOutboxSweeperOptionsBuilder SetTimerInterval(int timerIntervalSeconds)
    {
        _options += opt => opt.TimerInterval = timerIntervalSeconds;
        return this;
    }

    /// <summary>
    /// Configures the minimum age a message must be before it's eligible for sweeping
    /// </summary>
    /// <param name="minimumAge">The minimum age threshold as a TimeSpan</param>
    /// <returns>The builder instance for method chaining</returns>
    public TimedOutboxSweeperOptionsBuilder SetMinimumMessageAge(TimeSpan minimumAge)
    {
        _options += opt => opt.MinimumMessageAge = minimumAge;
        return this;
    }

    /// <summary>
    /// Configures the maximum number of messages to process in a single sweep
    /// </summary>
    /// <param name="batchSize">The maximum number of messages per sweep batch</param>
    /// <returns>The builder instance for method chaining</returns>
    public TimedOutboxSweeperOptionsBuilder SetBatchSize(int batchSize)
    {
        _options += opt => opt.BatchSize = batchSize;
        return this;
    }

    /// <summary>
    /// Configures whether to use bulk message dispatch operations
    /// </summary>
    /// <param name="useBulk">True to enable bulk dispatch, false to use individual dispatch (default: true)</param>
    /// <returns>The builder instance for method chaining</returns>
    public TimedOutboxSweeperOptionsBuilder SetBulk(bool useBulk = true)
    {
        _options += opt => opt.UseBulk = useBulk;
        return this;
    }

    /// <summary>
    /// Adds a custom argument to the outbox sweeper configuration
    /// </summary>
    /// <param name="key">The argument key</param>
    /// <param name="value">The argument value</param>
    /// <returns>The builder instance for method chaining</returns>
    /// <remarks>
    /// Used to provide implementation-specific parameters to the outbox.
    /// These arguments are passed to the underlying outbox implementation.
    /// </remarks>
    public TimedOutboxSweeperOptionsBuilder SetArg(string key, object value)
    {
        _options += opt => opt.Args[key] = value;
        return this;
    }

    /// <summary>
    /// Builds the configuration action
    /// </summary>
    /// <returns>An action that will apply all configured options to a TimedOutboxSweeperOptions instance</returns>
    /// <remarks>
    /// This method is typically used internally by framework code. Consumers would call this to get the final configuration delegate.
    /// </remarks>
    internal Action<TimedOutboxSweeperOptions> Build()
    {
        return _options;
    }
}