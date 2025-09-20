using System;

using Microsoft.Extensions.DependencyInjection;

using Paramore.Brighter;
using Paramore.Brighter.Extensions.DependencyInjection;
using Paramore.Brighter.Outbox.Hosting;
using Paramore.Brighter.ServiceActivator.Extensions.DependencyInjection;

namespace Fluent.Brighter;

/// <summary>
/// Main entry point for fluently configuring Brighter components
/// </summary>
/// <remarks>
/// Provides a centralized fluent interface to configure all aspects of Brighter including:
/// - Subscriptions (message consumers)
/// - Request handlers
/// - Message mappers
/// - Message transformers
/// - Message producers
/// 
/// This builder coordinates configuration across specialized sub-builders.
/// </remarks>
public class FluentBrighterBuilder
{
    private readonly ConsumerBuilder _consumerBuilder = new();

    /// <summary>
    /// Configures message subscriptions and consumer settings
    /// </summary>
    /// <param name="configure">Configuration action for ConsumerBuilder</param>
    /// <returns>The FluentBrighterBuilder instance for fluent chaining</returns>
    public FluentBrighterBuilder Subscriptions(Action<ConsumerBuilder> configure)
    {
        configure(_consumerBuilder);
        return this;
    }
    
    private readonly RequestHandlerBuilder _requestHandlerBuilder = new();
    
    /// <summary>
    /// Configures request handlers (synchronous and asynchronous)
    /// </summary>
    /// <param name="configure">Configuration action for RequestHandlerBuilder</param>
    /// <returns>The FluentBrighterBuilder instance for fluent chaining</returns>
    public FluentBrighterBuilder RequestHandlers(Action<RequestHandlerBuilder> configure)
    {
        configure(_requestHandlerBuilder);
        return this;
    }
    
    private readonly MapperBuilder _mapperBuilder = new();

    /// <summary>
    /// Configures message mappers for request/message translation
    /// </summary>
    /// <param name="configure">Configuration action for MapperBuilder</param>
    /// <returns>The FluentBrighterBuilder instance for fluent chaining</returns>
    public FluentBrighterBuilder Mappers(Action<MapperBuilder> configure)
    {
        configure(_mapperBuilder);
        return this;
    }
    
    private TransformerBuilder _transformerBuilder = new();

    /// <summary>
    /// Configures message transformers for pipeline processing
    /// </summary>
    /// <param name="configure">Configuration action for TransformerBuilder</param>
    /// <returns>The FluentBrighterBuilder instance for fluent chaining</returns>
    public FluentBrighterBuilder Transformers(Action<TransformerBuilder> configure)
    {
        configure(_transformerBuilder);
        return this;
    }

    private ProducerBuilder _producerBuilder = new();

    /// <summary>
    /// Configures message producers and outbox settings
    /// </summary>
    /// <param name="configure">Configuration action for ProducerBuilder</param>
    /// <returns>The FluentBrighterBuilder instance for fluent chaining</returns>
    public FluentBrighterBuilder Producers(Action<ProducerBuilder> configure)
    {
        configure(_producerBuilder);
        return this;
    }

    private Action<TimedOutboxSweeperOptions>? _outboxSweeperOptions;
    
    /// <summary>
    /// Use the timed outbox sweeper with the specified configuration options
    /// </summary>
    /// <param name="options">The configuration action for TimedOutboxSweeperOptions. Allows configuring sweep interval, batch size, message age, and other outbox parameters.</param>
    /// <returns>The FluentBrighterBuilder instance for method chaining</returns>
    /// <remarks>
    /// This method configures the background message sweeper that periodically checks the outbox for unsent messages.
    /// Use this to customize:
    /// - Timer interval (in seconds)
    /// - Minimum message age (TimeSpan)
    /// - Batch size (number of messages per sweep)
    /// - Bulk dispatch mode
    /// - Custom outbox arguments via Args dictionary
    /// 
    /// Example usage:
    /// <code>
    /// .EnableSweeper(options => 
    /// {
    ///     options.TimerInterval = 10;
    ///     options.MinimumMessageAge = TimeSpan.FromSeconds(30);
    ///     options.BatchSize = 200;
    ///     options.WithArg("TableName", "Outbox");
    /// })
    /// </code>
    /// </remarks>
    public FluentBrighterBuilder UseOutboxSweeper(Action<TimedOutboxSweeperOptions> options)
    {
        _outboxSweeperOptions = options;
        return this;
    }

    private Action<IBrighterBuilder>? _archiverConfiguration;
    
    /// <summary>
    /// Configures and enables outbox archiving using a specified archive provider
    /// </summary>
    /// <typeparam name="TTransaction">The transaction type used by the underlying outbox</typeparam>
    /// <param name="archiveProvider">The archive provider implementation to use for storing archived messages</param>
    /// <param name="timedOutboxArchiverOptionsAction">Optional configuration action for customizing archiver behavior</param>
    /// <returns>The FluentBrighterBuilder instance for method chaining</returns>
    /// <remarks>
    /// <para>
    /// This method enables periodic archiving of messages from the outbox. Archived messages are moved from
    /// the active outbox to long-term storage using the provided archive provider.
    /// </para>
    /// <para>
    /// Example usage:
    /// <code>
    /// var archiveProvider = new SqlArchiveProvider(connectionString);
    /// 
    /// builder.UseOutboxArchiver&lt;SqlTransaction&gt;(
    ///     archiveProvider,
    ///     options => 
    ///     {
    ///         options.TimerInterval = 3600; // Run hourly
    ///         options.MinimumAge = TimeSpan.FromDays(7);
    ///         options.ArchiveBatchSize = 500;
    ///     });
    /// </code>
    /// </para>
    /// <para>
    /// The archiver runs on a timer with configurable:
    /// - <b>TimerInterval</b>: Execution frequency in seconds
    /// - <b>MinimumAge</b>: Messages must be this old to be archived
    /// - <b>ArchiveBatchSize</b>: Number of messages processed per execution
    /// </para>
    /// <para>
    /// When not provided, the archiver uses default options:
    /// - TimerInterval: 15 seconds
    /// - MinimumAge: 24 hours
    /// - ArchiveBatchSize: 100 messages
    /// </para>
    /// <para>
    /// Note: The actual archiving process will be started during service initialization
    /// </para>
    /// </remarks>
    public FluentBrighterBuilder UseOutboxArchiver<TTransaction>(IAmAnArchiveProvider archiveProvider,
        Action<TimedOutboxArchiverOptions>? timedOutboxArchiverOptionsAction = null)
    {
        _archiverConfiguration = builder => builder.UseOutboxArchiver<TTransaction>(archiveProvider, timedOutboxArchiverOptionsAction);
        return this;
    }
    
    private Action<IServiceCollection> _registerServices = _ =>{};

    /// <summary>
    /// Registers additional services with the dependency injection container
    /// </summary>
    /// <param name="configure">Action to configure the service collection</param>
    /// <returns>The FluentBrighterBuilder instance for method chaining</returns>
    public FluentBrighterBuilder RegisterServices(Action<IServiceCollection> configure)
    {
        _registerServices += configure;
        return this;
    }

    private readonly LuggageStoreBuilder _luggageStoreBuilder = new(); 
    
    
    /// <summary>
    /// Configures the luggage store for handling large messages that exceed normal message size limits
    /// </summary>
    /// <param name="configure">Action to configure the luggage store builder</param>
    /// <returns>The FluentBrighterBuilder instance for method chaining</returns>
    /// <remarks>
    /// The luggage store is used to handle message payloads that are too large for direct
    /// transmission through messaging systems. Large messages are stored externally (e.g., in S3)
    /// with only a reference included in the actual message.
    /// 
    /// Example usage:
    /// <code>
    /// .SetLuggageStore(store => store
    ///     .UseS3LuggageStore(cfg => cfg
    ///         .SetBucketName("my-bucket")
    ///         .SetRegion(RegionEndpoint.USWest2)))
    /// </code>
    /// </remarks>
    public FluentBrighterBuilder SetLuggageStore(Action<LuggageStoreBuilder> configure)
    {
        configure(_luggageStoreBuilder);
        return this;
    }
    
    internal void SetConsumerOptions(ConsumersOptions options)
        =>  _consumerBuilder.SetConsumerOptions(options);

    internal void SetBrighterBuilder(IBrighterBuilder builder)
    {
        _requestHandlerBuilder.SetRequestHandlers(builder);
        _mapperBuilder.SetMappers(builder);
        _transformerBuilder.SetTransforms(builder);
        _producerBuilder.SetProducer(builder);
        _luggageStoreBuilder.SetLuggageStore(builder);
        
        if (_outboxSweeperOptions != null)
        {
            builder.UseOutboxSweeper(opt => _outboxSweeperOptions(opt));
        }

        if (_archiverConfiguration != null)
        {
            _archiverConfiguration(builder);
        }

        _registerServices(builder.Services);
    }
}