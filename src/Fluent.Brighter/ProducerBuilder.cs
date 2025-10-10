using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Extensions.DependencyInjection;

using Paramore.Brighter;
using Paramore.Brighter.Extensions.DependencyInjection;
using Paramore.Brighter.Observability;

namespace Fluent.Brighter;

/// <summary>
/// Fluent builder for configuring Brighter message producers and outbox settings
/// </summary>
/// <remarks>
/// Provides a fluent interface to configure message production, outbox behavior, 
/// reply handling, and instrumentation for Brighter-based systems.
/// </remarks>
public sealed class ProducerBuilder
{
    private int? _archiveBatchSize;

    /// <summary>
    /// Sets the batch size for archiving messages (optional)
    /// </summary>
    /// <param name="archiveBatchSize">Number of messages to archive in a single batch</param>
    /// <returns>The builder instance for fluent chaining</returns>
    public ProducerBuilder SetArchiveBatchSize(int archiveBatchSize)
    {
        _archiveBatchSize = archiveBatchSize;
        return this;
    }
    
    private IAmAnArchiveProvider? _archiveProvider;
    
    /// <summary>
    /// Sets the archive provider for message storage (optional)
    /// </summary>
    /// <param name="archiveProvider">The archive provider implementation</param>
    /// <returns>The builder instance for fluent chaining</returns>
    public ProducerBuilder SetArchiveProvider(IAmAnArchiveProvider archiveProvider)
    {
        _archiveProvider = archiveProvider;
        return this;
    }
    
    private Type? _connectionProvider;

    /// <summary>
    /// Sets the connection provider type for producer connections (optional)
    /// </summary>
    /// <param name="connectionProvider">Type of connection provider</param>
    /// <returns>The builder instance for fluent chaining</returns>
    public ProducerBuilder SetConnectionProvider(Type connectionProvider)
    {
        _connectionProvider = connectionProvider;
        return this;
    }
    
    private Type? _transactionProvider ;

    /// <summary>
    /// Sets the transaction provider type for managing transactions (optional)
    /// </summary>
    /// <param name="transactionProvider">Type of transaction provider</param>
    /// <returns>The builder instance for fluent chaining</returns>
    public ProducerBuilder SetTransactionProvider(Type transactionProvider)
    {
        _transactionProvider = transactionProvider;
        return this;
    }
    
    private IDistributedLock? _distributedLock;

    /// <summary>
    /// Sets the distributed lock implementation for concurrency control (optional)
    /// </summary>
    /// <param name="distributedLock">Distributed lock implementation</param>
    /// <returns>The builder instance for fluent chaining</returns>
    public ProducerBuilder SetDistributedLock(IDistributedLock distributedLock)
    {
        _distributedLock = distributedLock;
        return this;
    }
    
    private IAmAnOutbox? _outbox;

    /// <summary>
    /// Sets the outbox implementation for reliable messaging (optional)
    /// </summary>
    /// <param name="outbox">Outbox implementation</param>
    /// <returns>The builder instance for fluent chaining</returns>
    public ProducerBuilder SetOutbox(IAmAnOutbox outbox)
    {
        _outbox = outbox;
        return this;
    }

    private int? _outboxBulkChunkSize;

    /// <summary>
    /// Sets the chunk size for bulk outbox operations (optional)
    /// </summary>
    /// <param name="outboxBulkChunkSize">Number of messages per bulk operation</param>
    /// <returns>The builder instance for fluent chaining</returns>
    public ProducerBuilder SetOutboxBulkChunkSize(int outboxBulkChunkSize)
    {
        _outboxBulkChunkSize = outboxBulkChunkSize;
        return this;
    }

    private int? _outboxTimeout;
    
    /// <summary>
    /// Sets the timeout for outbox operations (optional)
    /// </summary>
    /// <param name="outboxTimeout">Timeout duration in milliseconds</param>
    /// <returns>The builder instance for fluent chaining</returns>
    public ProducerBuilder SetOutboxTimeout(int outboxTimeout)
    {
        _outboxTimeout = outboxTimeout;
        return this;
    }

    private Dictionary<string, object>? _outboxBag;

    /// <summary>
    /// Sets additional context data for outbox operations (optional)
    /// </summary>
    /// <param name="outboxBag">Dictionary of contextual data</param>
    /// <returns>The builder instance for fluent chaining</returns>
    public ProducerBuilder SetOutboxBag(Dictionary<string, object> outboxBag)
    {
        _outboxBag = outboxBag;
        return this;
    }

    private int _maxOutStandingMessages = -1;

    /// <summary>
    /// Sets the maximum number of outstanding messages before throttling (optional)
    /// </summary>
    /// <param name="maxOutStandingMessages">Maximum allowed unacknowledged messages</param>
    /// <returns>The builder instance for fluent chaining</returns>
    public ProducerBuilder SetMaxOutStandingMessages(int maxOutStandingMessages)
    {
        _maxOutStandingMessages = maxOutStandingMessages;
        return this;
    }

    private TimeSpan _maxOutStandingCheckInterval = TimeSpan.Zero;

    /// <summary>
    /// Sets the interval for checking outstanding message counts (optional)
    /// </summary>
    /// <param name="maxOutStandingCheckInterval">Check interval duration</param>
    /// <returns>The builder instance for fluent chaining</returns>
    public ProducerBuilder SetMaxOutStandingCheckInterval(TimeSpan maxOutStandingCheckInterval)
    {
        _maxOutStandingCheckInterval = maxOutStandingCheckInterval;
        return this;
    }

    private IAmAProducerRegistry? _producerRegistry;

    /// <summary>
    /// Sets the producer registry implementation (optional)
    /// </summary>
    /// <param name="producerRegistry">Producer registry instance</param>
    /// <returns>The builder instance for fluent chaining</returns>
    public ProducerBuilder SetProducerRegistry(IAmAProducerRegistry producerRegistry)
    {
        _producerRegistry =  producerRegistry;
        return this;
    }

    private List<IAmAMessageProducerFactory> _messageProducerFactories = [];

    /// <summary>
    /// Adds a message producer factory to the configuration (optional)
    /// </summary>
    /// <param name="factory">Producer factory implementation</param>
    /// <returns>The builder instance for fluent chaining</returns>
    public ProducerBuilder AddMessageProducerFactory(IAmAMessageProducerFactory factory)
    {
        _messageProducerFactories.Add(factory);
        return this;
    }

    private List<Subscription>? _replySubscriptions;

    /// <summary>
    /// Sets the collection of reply subscriptions (optional)
    /// </summary>
    /// <param name="replySubscriptions">Enumerable collection of reply subscriptions</param>
    /// <returns>The builder instance for fluent chaining</returns>
    public ProducerBuilder SetReplySubscription(IEnumerable<Subscription> replySubscriptions)
    {
        _replySubscriptions = replySubscriptions.ToList();
        return this;
    }

    /// <summary>
    /// Adds a single reply subscription to the configuration (optional)
    /// </summary>
    /// <param name="replySubscription">Reply subscription to add</param>
    /// <returns>The builder instance for fluent chaining</returns>
    public ProducerBuilder AddReplySubscription(Subscription replySubscription)
    {
        _replySubscriptions ??= [];
        _replySubscriptions.Add(replySubscription);
        return this;
    }

    private IAmAChannelFactory? _defaultReplyChannelFactory;

    /// <summary>
    /// Sets the default channel factory for reply messages (optional)
    /// </summary>
    /// <param name="defaultReplyChannelFactory">Channel factory implementation</param>
    /// <returns>The builder instance for fluent chaining</returns>
    public ProducerBuilder SetDefaultReplyChannelFactory(IAmAChannelFactory defaultReplyChannelFactory)
    {
        _defaultReplyChannelFactory = defaultReplyChannelFactory;
        return this;
    }
    
    private IAmARequestContextFactory? _defaultRequestContextFactory;

    /// <summary>
    /// Sets the default request context factory (optional)
    /// </summary>
    /// <param name="defaultRequestContextFactory">Request context factory implementation</param>
    /// <returns>The builder instance for fluent chaining</returns>
    public ProducerBuilder SetDefaultRequestContextFactory(IAmARequestContextFactory defaultRequestContextFactory)
    {
        _defaultRequestContextFactory = defaultRequestContextFactory;
        return this;
    }

    private IAmAMessageSchedulerFactory? _messageSchedulerFactory;

    /// <summary>
    /// Sets the message scheduler factory (optional)
    /// </summary>
    /// <param name="messageSchedulerFactory">Scheduler factory implementation</param>
    /// <returns>The builder instance for fluent chaining</returns>
    public ProducerBuilder SetMessageSchedulerFactory(IAmAMessageSchedulerFactory messageSchedulerFactory)
    {
        _messageSchedulerFactory = messageSchedulerFactory;
        return this;
    }

    private InstrumentationOptions _instrumentation = InstrumentationOptions.All;

    /// <summary>
    /// Configures instrumentation options for monitoring (optional)
    /// </summary>
    /// <param name="instrumentation">Instrumentation configuration flags</param>
    /// <returns>The builder instance for fluent chaining</returns>
    public ProducerBuilder SetInstrumentation(InstrumentationOptions instrumentation)
    {
        _instrumentation = instrumentation;
        return this;
    }
    
    private Action<ProducersConfiguration>? _configuration;

    /// <summary>
    /// Sets additional configuration via ProducersConfiguration action (optional)
    /// </summary>
    /// <param name="configuration">Configuration action</param>
    /// <returns>The builder instance for fluent chaining</returns>
    public ProducerBuilder SetConfiguration(Action<ProducersConfiguration> configuration)
    {
        _configuration = configuration;
        return this;
    }

    internal void SetProducer(IBrighterBuilder builder)
    {
        builder.AddProducers(producer =>
        {
            producer.ArchiveBatchSize = _archiveBatchSize;
            producer.ArchiveProvider = _archiveProvider;
            
            producer.ConnectionProvider = _connectionProvider;
            producer.TransactionProvider =  _transactionProvider;
            producer.DistributedLock = _distributedLock;
            
            producer.Outbox = _outbox;
            producer.OutboxBulkChunkSize = _outboxBulkChunkSize;
            producer.OutboxTimeout = _outboxTimeout;
            producer.OutBoxBag = _outboxBag;

            if (producer.Outbox != null)
            {
                builder.Services.AddSingleton(producer.Outbox);
            }
            
            producer.MaxOutStandingMessages = _maxOutStandingMessages;
            producer.MaxOutStandingCheckInterval = _maxOutStandingCheckInterval;

            if (_producerRegistry == null && _messageProducerFactories.Count > 0 )
            {
                _producerRegistry = new CombinedProducerRegistryFactory(_messageProducerFactories.ToArray()).Create();
            }
            
            producer.ProducerRegistry = _producerRegistry;

            producer.ReplyQueueSubscriptions = _replySubscriptions;
            producer.ResponseChannelFactory = _defaultReplyChannelFactory;
            producer.RequestContextFactory = _defaultRequestContextFactory;
            producer.MessageSchedulerFactory = _messageSchedulerFactory;

            producer.InstrumentationOptions = _instrumentation;
            
            _configuration?.Invoke(producer);
        });
    }
}