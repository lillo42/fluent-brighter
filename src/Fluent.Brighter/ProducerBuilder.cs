using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Extensions.DependencyInjection;

using Paramore.Brighter;
using Paramore.Brighter.Extensions.DependencyInjection;
using Paramore.Brighter.Observability;

namespace Fluent.Brighter;

public sealed class ProducerBuilder
{
    private int? _archiveBatchSize;

    public ProducerBuilder SetArchiveBatchSize(int archiveBatchSize)
    {
        _archiveBatchSize = archiveBatchSize;
        return this;
    }
    
    private IAmAnArchiveProvider? _archiveProvider;
    public ProducerBuilder SetArchiveProvider(IAmAnArchiveProvider archiveProvider)
    {
        _archiveProvider = archiveProvider;
        return this;
    }
    
    private Type? _connectionProvider;

    public ProducerBuilder SetConnectionProvider(Type connectionProvider)
    {
        _connectionProvider = connectionProvider;
        return this;
    }
    
    private Type? _transactionProvider ;

    public ProducerBuilder SetTransactionProvider(Type transactionProvider)
    {
        _transactionProvider = transactionProvider;
        return this;
    }
    
    private IDistributedLock? _distributedLock;

    public ProducerBuilder SetDistributedLock(IDistributedLock distributedLock)
    {
        _distributedLock = distributedLock;
        return this;
    }
    
    private IAmAnOutbox? _outbox;

    public ProducerBuilder SetOutbox(IAmAnOutbox outbox)
    {
        _outbox = outbox;
        return this;
    }

    private int? _outboxBulkChunkSize;

    public ProducerBuilder SetOutboxBulkChunkSize(int outboxBulkChunkSize)
    {
        _outboxBulkChunkSize = outboxBulkChunkSize;
        return this;
    }

    private int? _outboxTimeout;

    public ProducerBuilder SetOutboxTimeout(int outboxTimeout)
    {
        _outboxTimeout = outboxTimeout;
        return this;
    }

    private Dictionary<string, object>? _outboxBag;

    public ProducerBuilder SetOutboxBag(Dictionary<string, object> outboxBag)
    {
        _outboxBag = outboxBag;
        return this;
    }

    private int? _maxOutStandingMessages;

    public ProducerBuilder SetMaxOutStandingMessages(int maxOutStandingMessages)
    {
        _maxOutStandingMessages = maxOutStandingMessages;
        return this;
    }

    private TimeSpan _maxOutStandingCheckInterval = TimeSpan.Zero;

    public ProducerBuilder SetMaxOutStandingCheckInterval(TimeSpan maxOutStandingCheckInterval)
    {
        _maxOutStandingCheckInterval = maxOutStandingCheckInterval;
        return this;
    }

    private IAmAProducerRegistry? _producerRegistry;

    public ProducerBuilder SetProducerRegistry(IAmAProducerRegistry producerRegistry)
    {
        _producerRegistry =  producerRegistry;
        return this;
    }

    private List<IAmAMessageProducerFactory> _messageProducerFactories = [];

    public ProducerBuilder AddMessageProducerFactory(IAmAMessageProducerFactory factory)
    {
        _messageProducerFactories.Add(factory);
        return this;
    }

    private List<Subscription>? _replySubscriptions;

    public ProducerBuilder SetReplySubscription(IEnumerable<Subscription> replySubscriptions)
    {
        _replySubscriptions = replySubscriptions.ToList();
        return this;
    }

    public ProducerBuilder AddReplySubscription(Subscription replySubscription)
    {
        _replySubscriptions ??= [];
        _replySubscriptions.Add(replySubscription);
        return this;
    }

    private IAmAChannelFactory? _defaultReplyChannelFactory;

    public ProducerBuilder SetDefaultReplyChannelFactory(IAmAChannelFactory defaultReplyChannelFactory)
    {
        _defaultReplyChannelFactory = defaultReplyChannelFactory;
        return this;
    }
    
    private IAmARequestContextFactory? _defaultRequestContextFactory;

    public ProducerBuilder SetDefaultRequestContextFactory(IAmARequestContextFactory defaultRequestContextFactory)
    {
        _defaultRequestContextFactory = defaultRequestContextFactory;
        return this;
    }

    private IAmAMessageSchedulerFactory? _messageSchedulerFactory;

    public ProducerBuilder SetMessageSchedulerFactory(IAmAMessageSchedulerFactory messageSchedulerFactory)
    {
        _messageSchedulerFactory = messageSchedulerFactory;
        return this;
    }

    private InstrumentationOptions _instrumentation = InstrumentationOptions.All;

    public ProducerBuilder SetInstrumentation(InstrumentationOptions instrumentation)
    {
        _instrumentation = instrumentation;
        return this;
    }
    
    private Action<ProducersConfiguration>? _configuration;

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
                _producerRegistry = new CombinedProducerRegistryFactory(_messageProducerFactories.ToArray())
                    .Create();
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