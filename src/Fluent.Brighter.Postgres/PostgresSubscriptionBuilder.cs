using System;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.Postgres;

namespace Fluent.Brighter.Postgres;

public sealed class PostgresSubscriptionBuilder
{
    private SubscriptionName? _subscriptionName;

    public PostgresSubscriptionBuilder SetSubscription(SubscriptionName subscriptionName)
    {
        _subscriptionName = subscriptionName;
        return this;
    }
    
    private ChannelName? _channelName;
    private RoutingKey? _routingKey;
    public PostgresSubscriptionBuilder SetQueue(ChannelName channelName)
    {
        _channelName = channelName;
        _routingKey = new RoutingKey(channelName.Value);
        return this;
    }
    
    private Type? _dataType;

    public PostgresSubscriptionBuilder SetDataType(Type? dataType)
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

    public PostgresSubscriptionBuilder SetGetRequestType(Func<Message, Type>? getRequestType)
    {
        _getRequestType = getRequestType;
        return this;
    }

    private int _bufferSize = 1;

    public PostgresSubscriptionBuilder SetBufferSize(int bufferSize)
    {
        _bufferSize = bufferSize;
        return this;
    }
    
    private int _noOfPerformers  = 1;

    public PostgresSubscriptionBuilder SetNumberOfPerformers(int noOfPerformers)
    {
        _noOfPerformers = noOfPerformers;
        return this;
    }
    
    private TimeSpan? _timeOut;
    public PostgresSubscriptionBuilder SetTimeout(TimeSpan? timeout)
    {
        _timeOut = timeout;
        return this;
    }
    
    private int _requeueCount = -1;
    public PostgresSubscriptionBuilder SetRequeueCount(int requeueCount)
    {
        _requeueCount = requeueCount;
        return this;
    }
    
    private TimeSpan? _requeueDelay;
    public PostgresSubscriptionBuilder SetRequeueDelay(TimeSpan? timeout)
    {
        _requeueDelay = timeout;
        return this;
    }
    
    private int _unacceptableMessageLimit;
    public PostgresSubscriptionBuilder SetUnacceptableMessageLimit(int unacceptableMessageLimit)
    {
        _unacceptableMessageLimit = unacceptableMessageLimit ;
        return this;
    }
    
    private MessagePumpType _messagePumpType = MessagePumpType.Proactor;
    public PostgresSubscriptionBuilder SetMessagePumpType(MessagePumpType messagePumpType)
    {
        _messagePumpType = messagePumpType;
        return this;
    }

    private IAmAChannelFactory? _channelFactory;

    public PostgresSubscriptionBuilder SetChannelFactory(IAmAChannelFactory? channelFactory)
    {
        _channelFactory = channelFactory;
        return this;
    }
    
    private OnMissingChannel _onMissingChannel = OnMissingChannel.Create;
    public PostgresSubscriptionBuilder SetMakeChannels(OnMissingChannel onMissingChannel)
    {
        _onMissingChannel = onMissingChannel;
        return this;
    }

    private TimeSpan? _emptyChannelDelay;
    public PostgresSubscriptionBuilder SetEmptyChannelDelay(TimeSpan? emptyChannelDelay)
    {
        _emptyChannelDelay = emptyChannelDelay;
        return this;
    }

    private TimeSpan? _channelFailureDelay;
    public PostgresSubscriptionBuilder SetChannelFailureDelay(TimeSpan? channelFailureDelay)
    {
        _channelFailureDelay = channelFailureDelay;
        return this;
    }

    private string? _schemaName;
    public PostgresSubscriptionBuilder SetSchemaName(string? schemaName)
    {
        _schemaName = schemaName;
        return this;
    }

    private string? _queueStoreTable;
    public PostgresSubscriptionBuilder SetQueueStoreTable(string? queueStoreTable)
    {
        _queueStoreTable = queueStoreTable;
        return this;
    }

    private TimeSpan? _visibleTimeout;
    public PostgresSubscriptionBuilder SetVisibleTimeout(TimeSpan? visibleTimeout)
    {
        _visibleTimeout = visibleTimeout;
        return this;
    }

    private bool _tableWithLargeMessage;
    public PostgresSubscriptionBuilder SetTableWithLargeMessage(bool tableWithLargeMessage)
    {
        _tableWithLargeMessage = tableWithLargeMessage;
        return this;
    }

    private bool? _binaryMessagePayload;
    public PostgresSubscriptionBuilder SetBinaryMessagePayload(bool? binaryMessagePayload)
    {
        _binaryMessagePayload = binaryMessagePayload;
        return this;
    }
    
    internal PostgresSubscription Build()
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
        
        return new PostgresSubscription(
            subscriptionName: _subscriptionName,
            channelName: _channelName,
            routingKey: _routingKey,
            dataType: _dataType,
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
            channelFailureDelay: _channelFailureDelay,
            schemaName: _schemaName,
            queueStoreTable: _queueStoreTable,
            visibleTimeout: _visibleTimeout,
            tableWithLargeMessage: _tableWithLargeMessage,
            binaryMessagePayload: _binaryMessagePayload);
    }
}