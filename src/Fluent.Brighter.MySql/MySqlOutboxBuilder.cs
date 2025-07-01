using Paramore.Brighter;
using Paramore.Brighter.MySql;
using Paramore.Brighter.Outbox.MySql;

namespace Fluent.Brighter.MySql;

/// <summary>
/// A fluent builder for creating instances of <see cref="BrighterOutboxConfiguration"/>.
/// Provides a clean, readable API for configuring inbox behavior including de-duplication, scope, and MySQL-specific settings.
/// </summary>
public class MySqlOutboxBuilder
{
    private int? _maxOutStandingMessages = -1;
    
    /// <summary>
    /// Sets the maximum number of outstanding messages allowed in the outbox before throwing an exception.
    /// -1 => No limit (older entries may be discarded based on implementation)  
    /// 0 => Disallow any messages in the outbox (fail immediately)  
    /// 1+ => Allow this number of messages to accumulate before failing fast
    /// </summary>
    /// <param name="maxOutStandingMessages">The max number of messages allowed in the outbox.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public MySqlOutboxBuilder MaxOutStandingMessages(int? maxOutStandingMessages)
    {
        _maxOutStandingMessages = maxOutStandingMessages;
        return this;
    }
    
    private TimeSpan _maxOutStandingCheckInterval = TimeSpan.Zero;

    /// <summary>
    /// Sets the interval at which to check if the outbox has exceeded the <see cref="MaxOutStandingMessages"/> limit.
    /// A background thread checks this when inserting new messages.  
    /// If <see cref="MaxOutStandingMessages"/> is -1 or 0, this setting is ignored.
    /// </summary>
    /// <param name="maxOutStandingCheckInterval">The interval between checks (e.g., every 5 seconds).</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public MySqlOutboxBuilder MaxOutStandingCheckInterval(TimeSpan maxOutStandingCheckInterval)
    {
        _maxOutStandingCheckInterval = maxOutStandingCheckInterval;
        return this;
    }

    private int? _outboxBulkChunkSize;
    
    /// <summary>
    /// Sets the maximum number of messages to deposit into the outbox in a single batch.
    /// This helps avoid oversized insert operations.
    /// </summary>
    /// <param name="outboxBulkChunkSize">The bulk insert size limit.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public MySqlOutboxBuilder BulkChunkSize(int? outboxBulkChunkSize)
    {
        _outboxBulkChunkSize = outboxBulkChunkSize;
        return this;
    }
    
    private Dictionary<string, object>? _outBoxBag; 

    /// <summary>
    /// Sets additional arguments required by the outbox implementation.
    /// For example, the DynamoDB outbox may require a "Topic" key in this bag.
    /// </summary>
    /// <param name="outBoxBag">A dictionary of additional arguments.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public MySqlOutboxBuilder Bag(Dictionary<string, object>? outBoxBag)
    {
        _outBoxBag = outBoxBag;
        return this;
    }

    private int? _outboxTimeout;
    
    /// <summary>
    /// Sets the timeout duration for writing to the outbox, in milliseconds.
    /// </summary>
    /// <param name="outboxTimeout">The timeout in milliseconds.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public MySqlOutboxBuilder Timeout(int? outboxTimeout)
    {
        _outboxTimeout = outboxTimeout;
        return this;
    }

    private RelationalDatabaseConfiguration? _configuration;
    
    /// <summary>
    /// Sets the MySQL-specific relational database configuration.
    /// </summary>
    /// <param name="configuration">An instance of <see cref="RelationalDatabaseConfiguration"/>.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public MySqlOutboxBuilder Configuration(RelationalDatabaseConfiguration? configuration)
    {
        _configuration = configuration;
        return this;
    }

    /// <summary>
    /// Sets the MySQL-specific relational database configuration.
    /// </summary>
    /// <param name="configuration">An instance of <see cref="RelationalDatabaseConfiguration"/>.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public MySqlOutboxBuilder Configuration(Action<RelationalDatabaseConfigurationBuilder> configuration)
    {
        var builder = new RelationalDatabaseConfigurationBuilder();
        configuration(builder);
        _configuration = builder.Build();
        return this;
    }
   
    /// <summary>
    /// Sets the MySQL-specific relational database configuration if it was not set.
    /// </summary>
    /// <param name="configuration">An instance of <see cref="RelationalDatabaseConfiguration"/>.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public MySqlOutboxBuilder ConfigurationIfIsMissing(RelationalDatabaseConfiguration configuration)
    {
        _configuration ??= configuration;
        return this;
    }

    private bool _useUnitOfWork;
    
    /// <summary>
    /// Sets whether to use a unit of work with the outbox.
    /// </summary>
    /// <param name="useUnitOfWork">True to enable unit of work; otherwise, false.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public MySqlOutboxBuilder UseUnitOfWork(bool useUnitOfWork)
    {
        _useUnitOfWork = useUnitOfWork;
        return this;
    }

    /// <summary>
    /// Enable to use a unit of work with the outbox.
    /// </summary>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public MySqlOutboxBuilder EnableUnitOfWork() => UseUnitOfWork(true);
    
    /// <summary>
    /// Disable to use a unit of work with the outbox.
    /// </summary>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public MySqlOutboxBuilder DisableUnitOfWork() => UseUnitOfWork(false);
    
    private IAmARelationalDbConnectionProvider _unitOfWork = null!;

    /// <summary>
    /// Sets the unit of work connection provider
    /// </summary>
    /// <param name="provider"></param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public MySqlOutboxBuilder UnitOfWorkConnectionProvider(IAmARelationalDbConnectionProvider provider)
    {
        _unitOfWork = provider;
        return this;
    }
        
    /// <summary>
    /// Create a new instance of <see cref="BrighterOutboxConfiguration" /> with provided data.
    /// </summary>
    /// <returns>A new instance of <see cref="BrighterOutboxConfiguration" />.</returns>
    public BrighterOutboxConfiguration Build()
    {
        var provider = _useUnitOfWork ? _unitOfWork : new MySqlConnectionProvider(_configuration!);
        return new BrighterOutboxConfiguration
        {
            MaxOutStandingMessages = _maxOutStandingMessages,
            MaxOutStandingCheckInterval = _maxOutStandingCheckInterval,
            OutboxBulkChunkSize = _outboxBulkChunkSize,
            OutboxTimeout = _outboxTimeout,
            OutBoxBag = _outBoxBag,
            Outbox = new MySqlOutbox(_configuration!, provider)
        };
    }
}