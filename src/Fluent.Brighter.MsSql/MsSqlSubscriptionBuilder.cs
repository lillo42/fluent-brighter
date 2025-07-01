using System;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.MsSql;

namespace Fluent.Brighter.MsSql;

/// <summary>
/// A fluent builder for creating instances of <see cref="MsSqlSubscription"/>.
/// </summary>
public class MsSqlSubscriptionBuilder
{
    private Type? _dataType;

    /// <summary>
    /// Sets the data type.
    /// </summary>
    /// <param name="type">The data type.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public MsSqlSubscriptionBuilder MessageType(Type type)
    {
        _dataType = type ?? throw new ArgumentNullException(nameof(type));
        return this;
    }
    
    /// <summary>
    /// Sets the data type.
    /// </summary>
    /// <returns>The current builder instance for fluent chaining.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public MsSqlSubscriptionBuilder MessageType<T>()
        where T : class, IRequest
    {
        return MessageType(typeof(T));
    }
    
    private SubscriptionName? _subscriptionName;
    
    /// <summary>
    /// Sets the subscription name.
    /// </summary>
    /// <param name="subscriptionName">The name of the subscription.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public MsSqlSubscriptionBuilder SubscriptionName(SubscriptionName? subscriptionName)
    {
        _subscriptionName = subscriptionName;
        return this;
    }

    private ChannelName? _channelName;
    
    /// <summary>
    /// Sets the channel name (queue name).
    /// </summary>
    /// <param name="channelName">The name of the channel to use for the subscription.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public MsSqlSubscriptionBuilder ChannelName(ChannelName? channelName)
    {
        _channelName = channelName;
        return this;
    }

    /// <summary>
    /// Sets the queue name (channel name).
    /// </summary>
    /// <param name="queueName">The name of the queue to use for the subscription.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public MsSqlSubscriptionBuilder Queue(ChannelName? queueName)
        => ChannelName(queueName);
    

    private int _bufferSize = 1;

    /// <summary>
    /// Sets the buffer size.
    /// </summary>
    /// <param name="bufferSize">The number of messages to buffer at once (min 1, max 10).</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public MsSqlSubscriptionBuilder BufferSize(int bufferSize)
    {
        _bufferSize = bufferSize;
        return this;
    }

    private int _noOfPerformers = Environment.ProcessorCount;
    
    /// <summary>
    /// Sets the number of performers (threads) for processing messages.
    /// </summary>
    /// <param name="noOfPerformers">The number of threads reading from the channel.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public MsSqlSubscriptionBuilder NoOfPerformers(int noOfPerformers)
    {
        _noOfPerformers = noOfPerformers;
        return this;
    }

    /// <summary>
    /// Sets the number of threads for processing messages.
    /// </summary>
    /// <param name="noOfPerformers">The number of threads reading from the channel.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public MsSqlSubscriptionBuilder Concurrency(int noOfPerformers) => NoOfPerformers(noOfPerformers);
    
    private TimeSpan? _timeOut;

    /// <summary>
    /// Sets the timeout for message retrieval.
    /// </summary>
    /// <param name="timeOut">The timeout in milliseconds for retrieving messages.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public MsSqlSubscriptionBuilder TimeOut(TimeSpan? timeOut)
    {
        _timeOut = timeOut;
        return this;
    }
    
    private int _requeueCount = -1;

    /// <summary>
    /// Sets the number of times to requeue a failed message.
    /// </summary>
    /// <param name="requeueCount">The number of retries before dropping a message.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public MsSqlSubscriptionBuilder RequeueCount(int requeueCount)
    {
        _requeueCount = requeueCount;
        return this;
    }
    
    private TimeSpan? _requeueDelay;

    /// <summary>
    /// Sets the delay between message requeues.
    /// </summary>
    /// <param name="requeueDelay">The delay before retrying a failed message.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public MsSqlSubscriptionBuilder RequeueDelay(TimeSpan? requeueDelay)
    {
        _requeueDelay = requeueDelay;
        return this;
    }
    
    private int _unacceptableMessageLimit;
    
    /// <summary>
    /// Sets the limit for unacceptable messages before stopping the subscription.
    /// </summary>
    /// <param name="unacceptableMessageLimit">The number of unacceptable messages to tolerate.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public MsSqlSubscriptionBuilder UnacceptableMessageLimit(int unacceptableMessageLimit)
    {
        _unacceptableMessageLimit = unacceptableMessageLimit;
        return this;
    }
    
    private MessagePumpType _messagePumpType = MessagePumpType.Reactor;

    /// <summary>
    /// Sets the message pump type (Reactor or Proactor).
    /// </summary>
    /// <param name="messagePumpType">The message pump type to use.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public MsSqlSubscriptionBuilder MessagePump(MessagePumpType messagePumpType)
    {
        _messagePumpType = messagePumpType;
        return this;
    }

    /// <summary>
    /// Sets the message pump as <see cref="Paramore.Brighter.MessagePumpType.Proactor"/>
    /// </summary>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public MsSqlSubscriptionBuilder AsProactor() => MessagePump(MessagePumpType.Proactor);
    
    /// <summary>
    /// Sets the message pump as <see cref="Paramore.Brighter.MessagePumpType.Reactor"/>
    /// </summary>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public MsSqlSubscriptionBuilder AsReactor() => MessagePump(MessagePumpType.Reactor);
    
    private IAmAChannelFactory? _channelFactory;

    /// <summary>
    /// Sets the channel factory used for creating channels.
    /// </summary>
    /// <param name="channelFactory">The channel factory to use.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public MsSqlSubscriptionBuilder ChannelFactory(IAmAChannelFactory? channelFactory)
    {
        _channelFactory = channelFactory;
        return this;
    }
    
    private OnMissingChannel _makeChannels = OnMissingChannel.Create;
    
    /// <summary>
    /// Sets whether to create channels if they do not exist.
    /// </summary>
    /// <param name="makeChannels">Action to take when the channel is missing.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public MsSqlSubscriptionBuilder MakeChannels(OnMissingChannel makeChannels)
    {
        _makeChannels = makeChannels;
        return this;
    }

    /// <summary>
    /// Set the make channels as <see cref="OnMissingChannel.Create"/>
    /// </summary>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public MsSqlSubscriptionBuilder CreateIfMissing() => MakeChannels(OnMissingChannel.Create);
    
    /// <summary>
    /// Set the make channels as <see cref="OnMissingChannel.Validate"/>
    /// </summary>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public MsSqlSubscriptionBuilder Validate() => MakeChannels(OnMissingChannel.Validate);
    
    /// <summary>
    /// Set the make channels as <see cref="OnMissingChannel.Assume"/>
    /// </summary>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public MsSqlSubscriptionBuilder Assume() => MakeChannels(OnMissingChannel.Assume);
    
    private TimeSpan? _emptyChannelDelay;

    /// <summary>
    /// Sets the delay to use when the channel is empty.
    /// </summary>
    /// <param name="emptyChannelDelay">The delay in milliseconds when no messages are available.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public MsSqlSubscriptionBuilder EmptyDelay(TimeSpan? emptyChannelDelay)
    {
        _emptyChannelDelay = emptyChannelDelay;
        return this;
    }

    private TimeSpan? _channelFailureDelay;
    
    /// <summary>
    /// Sets the delay to use when a channel failure occurs.
    /// </summary>
    /// <param name="channelFailureDelay">The delay in milliseconds when a channel fails.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public MsSqlSubscriptionBuilder FailureDelay(TimeSpan? channelFailureDelay)
    {
        _channelFailureDelay = channelFailureDelay;
        return this;
    }

    /// <summary>
    /// Create a new instance of <see cref="MsSqlSubscription"/> 
    /// </summary>
    /// <returns></returns>
    public MsSqlSubscription Build()
    {
        if (_dataType == null)
        {
            throw new ConfigurationException("Missing data type");
        }
        
        return new MsSqlSubscription(
            dataType: _dataType,
            subscriptionName: _subscriptionName,
            channelName: _channelName,
            bufferSize: _bufferSize,
            noOfPerformers: _noOfPerformers,
            timeOut: _timeOut,
            requeueCount: _requeueCount,
            requeueDelay: _requeueDelay,
            unacceptableMessageLimit: _unacceptableMessageLimit,
            messagePumpType: _messagePumpType,
            channelFactory: _channelFactory,
            makeChannels: _makeChannels,
            emptyChannelDelay: _emptyChannelDelay,
            channelFailureDelay: _channelFailureDelay);
    }
}
