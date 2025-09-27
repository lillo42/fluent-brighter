using Org.Apache.Rocketmq;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.RocketMQ;
using Paramore.Brighter.Observability;

namespace Fluent.Brighter.RocketMQ;

using System;

/// <summary>
/// Fluent builder for configuring RocketMQ messaging gateway connections in Brighter pipeline.
/// Implements RocketMQ's high-throughput messaging configuration pattern through a fluent interface.
/// </summary>
public sealed class RocketMessagingGatewayConnectionBuilder
{
    private ClientConfig? _clientConfig;

    public RocketMessagingGatewayConnectionBuilder SetClient(ClientConfig clientConfig)
    {
        _clientConfig = clientConfig ?? throw new ArgumentNullException(nameof(clientConfig));
        return this;
    }
    
    private TimeProvider _timerProvider = TimeProvider.System;
    
    /// <summary>
    /// Sets the time provider for time-sensitive operations.
    /// Used for implementing RocketMQ's message delay and timeout features.
    /// </summary>
    /// <param name="timerProvider">The time provider instance.</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    public RocketMessagingGatewayConnectionBuilder SetTimerProvider(TimeProvider timerProvider)
    {
        _timerProvider = timerProvider ?? throw new ArgumentNullException(nameof(timerProvider));
        return this;
    }

    private int _maxAttempts = 3;
    
    /// <summary>
    /// Sets the maximum retry attempts for failed message operations.
    /// Implements RocketMQ's at-least-once delivery guarantee through retries.
    /// </summary>
    /// <param name="maxAttempts">The maximum number of retry attempts.</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    public RocketMessagingGatewayConnectionBuilder SetMaxAttempts(int maxAttempts)
    {
        if (maxAttempts < 1)
        {
            throw new ArgumentException("Max attempts must be at least 1", nameof(maxAttempts));
        }
        
        _maxAttempts = maxAttempts;
        return this;
    }
    
    
    private ITransactionChecker? _checker;

    /// <summary>
    /// Sets the transaction checker for RocketMQ transactional messages.
    /// Handles local transaction state checks during message recovery.
    /// </summary>
    /// <param name="checker">The transaction checker instance.</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    public RocketMessagingGatewayConnectionBuilder SetTransactionChecker(ITransactionChecker checker)
    {
        _checker = checker ?? throw new ArgumentNullException(nameof(checker));
        return this;
    }

    private InstrumentationOptions _instrumentation = InstrumentationOptions.All;
    
    /// <summary>
    /// Sets the instrumentation options for the gateway connection.
    /// </summary>
    /// <param name="instrumentation">The instrumentation options.</param>
    /// <returns>The builder instance for fluent chaining.</returns>
    public RocketMessagingGatewayConnectionBuilder SetInstrumentation(InstrumentationOptions instrumentation)
    {
        _instrumentation = instrumentation;
        return this;
    }

    /// <summary>
    /// Builds the final RocketMessagingGatewayConnection instance with the configured settings.
    /// </summary>
    /// <returns>A configured instance of RocketMessagingGatewayConnection.</returns>
    internal RocketMessagingGatewayConnection Build()
    {
        if (_clientConfig == null)
        {
            throw new ConfigurationException("ClientConfig must be set before calling this method");
        }
        
        return new RocketMessagingGatewayConnection(_clientConfig)
        {
            TimerProvider = _timerProvider,
            MaxAttempts = _maxAttempts,
            Checker = _checker,
            Instrumentation = _instrumentation
        };
    }
}