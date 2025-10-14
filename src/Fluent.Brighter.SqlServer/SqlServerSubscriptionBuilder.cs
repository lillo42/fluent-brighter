using System;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.MsSql;

namespace Fluent.Brighter.SqlServer;

/// <summary>
/// Provides a fluent builder for configuring a <see cref="Subscription"/> used to consume messages
/// from a SQL Server-backed message queue within the Fluent Brighter library.
/// </summary>
public sealed class SqlServerSubscriptionBuilder
{
    private SubscriptionName? _subscriptionName;

    /// <summary>
    /// Sets the logical name of the subscription, used for identification and correlation.
    /// </summary>
    /// <param name="subscriptionName">The subscription name.</param>
    /// <returns>The current instance of <see cref="SqlServerSubscriptionBuilder"/> to allow method chaining.</returns>
    public SqlServerSubscriptionBuilder SetSubscription(SubscriptionName subscriptionName)
    {
        _subscriptionName = subscriptionName;
        return this;
    }
    
    private ChannelName? _channelName;
    private RoutingKey? _routingKey;

    /// <summary>
    /// Sets the channel (queue) name to consume messages from.
    /// Also configures the routing key to match the channel name.
    /// </summary>
    /// <param name="channelName">The name of the channel (queue).</param>
    /// <returns>The current instance of <see cref="SqlServerSubscriptionBuilder"/> to allow method chaining.</returns>
    /// <remarks>
    /// In SQL Server-based implementations, the channel typically maps to a physical table or queue.
    /// </remarks>
    public SqlServerSubscriptionBuilder SetQueue(ChannelName channelName)
    {
        _channelName = channelName;
        _routingKey = new RoutingKey(channelName.Value);
        return this;
    }
    
    private Type? _dataType;

    /// <summary>
    /// Sets the expected .NET type of the message payload (request/command).
    /// If not already set, this also auto-configures the subscription name, channel name, and routing key
    /// based on the type's name.
    /// </summary>
    /// <param name="dataType">The request type, or <see langword="null"/> to clear.</param>
    /// <returns>The current instance of <see cref="SqlServerSubscriptionBuilder"/> to allow method chaining.</returns>
    public SqlServerSubscriptionBuilder SetDataType(Type? dataType)
    {
        _dataType = dataType;
        if (dataType == null)
        {
            return this;
        }
        
        if (_subscriptionName == null)
        {
            _subscriptionName = new SubscriptionName(dataType.ToString());
        }

        if (ChannelName.IsNullOrEmpty(_channelName))
        {
            _channelName = new ChannelName(dataType.Name);
        }

        if (RoutingKey.IsNullOrEmpty(_routingKey))
        {
            _routingKey = new RoutingKey(dataType.Name);
        }
        
        return this;
    }
    
    private Func<Message, Type>? _getRequestType;

    /// <summary>
    /// Sets a function to dynamically determine the request type from an incoming <see cref="Message"/>.
    /// Used when messages may contain different types (e.g., polymorphic deserialization).
    /// </summary>
    /// <param name="getRequestType">A function that inspects the message and returns its concrete type, or <see langword="null"/> to disable dynamic typing.</param>
    /// <returns>The current instance of <see cref="SqlServerSubscriptionBuilder"/> to allow method chaining.</returns>
    public SqlServerSubscriptionBuilder SetGetRequestType(Func<Message, Type>? getRequestType)
    {
        _getRequestType = getRequestType;
        return this;
    }

    private int _bufferSize = 1;

    /// <summary>
    /// Sets the number of messages to buffer in memory for processing.
    /// </summary>
    /// <param name="bufferSize">The buffer size. Must be ≥ 1.</param>
    /// <returns>The current instance of <see cref="SqlServerSubscriptionBuilder"/> to allow method chaining.</returns>
    public SqlServerSubscriptionBuilder SetBufferSize(int bufferSize)
    {
        _bufferSize = bufferSize;
        return this;
    }
    
    private int _noOfPerformers = 1;

    /// <summary>
    /// Sets the number of concurrent performers (workers) processing messages from the channel.
    /// Controls the degree of parallelism.
    /// </summary>
    /// <param name="noOfPerformers">The number of performers. Must be ≥ 1.</param>
    /// <returns>The current instance of <see cref="SqlServerSubscriptionBuilder"/> to allow method chaining.</returns>
    public SqlServerSubscriptionBuilder SetNumberOfPerformers(int noOfPerformers)
    {
        _noOfPerformers = noOfPerformers;
        return this;
    }
    
    private TimeSpan? _timeOut;

    /// <summary>
    /// Sets the timeout for message processing. If a message takes longer than this duration,
    /// it may be considered failed (depending on the underlying transport and configuration).
    /// </summary>
    /// <param name="timeout">The processing timeout, or <see langword="null"/> for no timeout.</param>
    /// <returns>The current instance of <see cref="SqlServerSubscriptionBuilder"/> to allow method chaining.</returns>
    public SqlServerSubscriptionBuilder SetTimeout(TimeSpan? timeout)
    {
        _timeOut = timeout;
        return this;
    }
    
    private int _requeueCount = -1;

    /// <summary>
    /// Sets the maximum number of times a failed message should be requeued before being dead-lettered or discarded.
    /// A value of -1 means infinite retries.
    /// </summary>
    /// <param name="requeueCount">The retry limit.</param>
    /// <returns>The current instance of <see cref="SqlServerSubscriptionBuilder"/> to allow method chaining.</returns>
    public SqlServerSubscriptionBuilder SetRequeueCount(int requeueCount)
    {
        _requeueCount = requeueCount;
        return this;
    }
    
    private TimeSpan? _requeueDelay;

    /// <summary>
    /// Sets the delay before a failed message is requeued for retry.
    /// </summary>
    /// <param name="requeueDelay">The delay duration, or <see langword="null"/> for immediate retry.</param>
    /// <returns>The current instance of <see cref="SqlServerSubscriptionBuilder"/> to allow method chaining.</returns>
    public SqlServerSubscriptionBuilder SetRequeueDelay(TimeSpan? requeueDelay)
    {
        _requeueDelay = requeueDelay;
        return this;
    }
    
    private int _unacceptableMessageLimit;

    /// <summary>
    /// Sets the number of consecutive unacceptable (e.g., unprocessable or malformed) messages
    /// that will cause the subscription to halt.
    /// </summary>
    /// <param name="unacceptableMessageLimit">The error tolerance threshold.</param>
    /// <returns>The current instance of <see cref="SqlServerSubscriptionBuilder"/> to allow method chaining.</returns>
    public SqlServerSubscriptionBuilder SetUnacceptableMessageLimit(int unacceptableMessageLimit)
    {
        _unacceptableMessageLimit = unacceptableMessageLimit;
        return this;
    }
    
    private MessagePumpType _messagePumpType = MessagePumpType.Proactor;

    /// <summary>
    /// Sets the message pump architecture used for consuming messages.
    /// </summary>
    /// <param name="messagePumpType">The pump type. Defaults to <see cref="MessagePumpType.Proactor"/>.</param>
    /// <returns>The current instance of <see cref="SqlServerSubscriptionBuilder"/> to allow method chaining.</returns>
    public SqlServerSubscriptionBuilder SetMessagePumpType(MessagePumpType messagePumpType)
    {
        _messagePumpType = messagePumpType;
        return this;
    }

    private IAmAChannelFactory? _channelFactory;

    /// <summary>
    /// Sets a custom channel factory used to create the underlying message channel.
    /// If not provided, a default SQL Server channel factory will be used.
    /// </summary>
    /// <param name="channelFactory">The channel factory implementation, or <see langword="null"/> to use the default.</param>
    /// <returns>The current instance of <see cref="SqlServerSubscriptionBuilder"/> to allow method chaining.</returns>
    public SqlServerSubscriptionBuilder SetChannelFactory(IAmAChannelFactory? channelFactory)
    {
        _channelFactory = channelFactory;
        return this;
    }
    
    private OnMissingChannel _onMissingChannel = OnMissingChannel.Create;

    /// <summary>
    /// Specifies the behavior when the configured channel (queue) does not exist at startup.
    /// </summary>
    /// <param name="onMissingChannel">The policy for handling missing channels. Defaults to <see cref="OnMissingChannel.Create"/>.</param>
    /// <returns>The current instance of <see cref="SqlServerSubscriptionBuilder"/> to allow method chaining.</returns>
    public SqlServerSubscriptionBuilder SetMakeChannels(OnMissingChannel onMissingChannel)
    {
        _onMissingChannel = onMissingChannel;
        return this;
    }

    private TimeSpan? _emptyChannelDelay;

    /// <summary>
    /// Sets the delay before polling an empty channel again.
    /// Helps reduce CPU usage during idle periods.
    /// </summary>
    /// <param name="emptyChannelDelay">The delay duration, or <see langword="null"/> for default behavior.</param>
    /// <returns>The current instance of <see cref="SqlServerSubscriptionBuilder"/> to allow method chaining.</returns>
    public SqlServerSubscriptionBuilder SetEmptyChannelDelay(TimeSpan? emptyChannelDelay)
    {
        _emptyChannelDelay = emptyChannelDelay;
        return this;
    }

    private TimeSpan? _channelFailureDelay;

    /// <summary>
    /// Sets the delay before retrying channel operations after a failure (e.g., database connection loss).
    /// </summary>
    /// <param name="channelFailureDelay">The recovery delay, or <see langword="null"/> for default behavior.</param>
    /// <returns>The current instance of <see cref="SqlServerSubscriptionBuilder"/> to allow method chaining.</returns>
    public SqlServerSubscriptionBuilder SetChannelFailureDelay(TimeSpan? channelFailureDelay)
    {
        _channelFailureDelay = channelFailureDelay;
        return this;
    }
    
    /// <summary>
    /// Builds and returns a configured <see cref="Subscription"/> instance.
    /// </summary>
    /// <returns>A fully configured <see cref="Subscription"/> object.</returns>
    /// <exception cref="ConfigurationException">
    /// Thrown if <see cref="SubscriptionName"/>, <see cref="ChannelName"/>, or <see cref="RoutingKey"/> have not been set.
    /// </exception>
    internal MsSqlSubscription Build()
    {
        if (_subscriptionName == null)
        {
            throw new ConfigurationException("SubscriptionName not set");
        }
        
        if (_channelName == null)
        {
            throw new ConfigurationException("ChannelName not set");
        }

        if (_routingKey == null)
        {
            throw new ConfigurationException("RoutingKey not set");
        }
        
        return new MsSqlSubscription(
            subscriptionName: _subscriptionName,
            channelName: _channelName,
            routingKey: _routingKey,
            requestType: _dataType,
            getRequestType: _getRequestType,
            bufferSize: _bufferSize,
            noOfPerformers: _noOfPerformers,
            timeOut: _timeOut,
            requeueCount: _requeueCount,
            requeueDelay: _requeueDelay,
            unacceptableMessageLimit: _unacceptableMessageLimit,
            messagePumpType: _messagePumpType,
            channelFactory: _channelFactory,
            makeChannels: _onMissingChannel,
            emptyChannelDelay: _emptyChannelDelay, 
            channelFailureDelay: _channelFailureDelay);
    }
}