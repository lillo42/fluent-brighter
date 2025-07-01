using Paramore.Brighter;
using Paramore.Brighter.Inbox;
using Paramore.Brighter.Inbox.MySql;
using Paramore.Brighter.MySql;

namespace Fluent.Brighter.MySql;

/// <summary>
/// A fluent builder for creating instances of <see cref="InboxConfiguration"/>.
/// Provides a clean, readable API for configuring inbox behavior including de-duplication, scope, and MySQL-specific settings.
/// </summary>
public class MySqlInboxBuilder
{
    // Base class constructor parameters
    private InboxScope _scope = InboxScope.All;
    private bool _onceOnly = true;
    private OnceOnlyAction _actionOnExists = OnceOnlyAction.Throw;
    private Func<Type, string>? _context;

    // Derived class properties
    private RelationalDatabaseConfiguration? _configuration;
    private bool _useUnitOfWork;

    /// <summary>
    /// Sets the scope of requests stored in the inbox.
    /// Defaults to <see cref="InboxScope.All"/> meaning all requests are tracked.
    /// </summary>
    /// <param name="scope">The inbox scope.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public MySqlInboxBuilder Scope(InboxScope scope)
    {
        _scope = scope;
        return this;
    }

    /// <summary>
    /// Sets whether the inbox should de-duplicate requests.
    /// Defaults to true.
    /// </summary>
    /// <param name="onceOnly">True to enable de-duplication; false to disable.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public MySqlInboxBuilder OnceOnly(bool onceOnly)
    {
        _onceOnly = onceOnly;
        return this;
    }

    /// <summary>
    /// Sets the action to take when a duplicate request is detected.
    /// Defaults to <see cref="OnceOnlyAction.Throw"/>.
    /// </summary>
    /// <param name="actionOnExists">The action to take on duplicate requests.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public MySqlInboxBuilder ActionOnExists(OnceOnlyAction actionOnExists)
    {
        _actionOnExists = actionOnExists;
        return this;
    }

    /// <summary>
    /// Sets a custom function to generate context keys for de-duplication.
    /// If not provided, defaults to using the full name of the handler type.
    /// </summary>
    /// <param name="context">A function that maps handler types to context strings.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public MySqlInboxBuilder Context(Func<Type, string>? context)
    {
        _context = context;
        return this;
    }

    /// <summary>
    /// Sets the MySQL-specific relational database configuration.
    /// </summary>
    /// <param name="configuration">An instance of <see cref="RelationalDatabaseConfiguration"/>.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public MySqlInboxBuilder Configuration(RelationalDatabaseConfiguration? configuration)
    {
        _configuration = configuration;
        return this;
    }
    
    /// <summary>
    /// Sets the MySQL-specific relational database configuration.
    /// </summary>
    /// <param name="configuration">An instance of <see cref="RelationalDatabaseConfiguration"/>.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public MySqlInboxBuilder Configuration(Action<RelationalDatabaseConfigurationBuilder> configuration)
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
    public MySqlInboxBuilder ConfigurationIfIsMissing(RelationalDatabaseConfiguration configuration)
    {
        _configuration ??= configuration;
        return this;
    }

    /// <summary>
    /// Sets whether to use a unit of work with the inbox.
    /// </summary>
    /// <param name="useUnitOfWork">True to enable unit of work; otherwise, false.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public MySqlInboxBuilder UseUnitOfWork(bool useUnitOfWork)
    {
        _useUnitOfWork = useUnitOfWork;
        return this;
    }
    
    /// <summary>
    /// Enable to use a unit of work with the inbox.
    /// </summary>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public MySqlInboxBuilder EnableUnitOfWork() => UseUnitOfWork(true);
    
    /// <summary>
    /// Disable to use a unit of work with the inbox.
    /// </summary>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public MySqlInboxBuilder DisableUnitOfWork() => UseUnitOfWork(false);
    
    private IAmARelationalDbConnectionProvider _unitOfWork = null!;

    /// <summary>
    /// Sets the unit of work connection provider
    /// </summary>
    /// <param name="provider"></param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public MySqlInboxBuilder UnitOfWorkConnectionProvider(IAmARelationalDbConnectionProvider provider)
    {
        _unitOfWork = provider;
        return this;
    }

    /// <summary>
    /// Builds and returns a new instance of <see cref="InboxConfiguration"/> with the specified settings.
    /// </summary>
    /// <returns>A configured instance of <see cref="InboxConfiguration"/>.</returns>
    public InboxConfiguration Build()
    {
        var provider = _useUnitOfWork ? _unitOfWork : new MySqlConnectionProvider(_configuration!);
        return new InboxConfiguration(
            new MySqlInbox(_configuration, provider),
            _scope,
            _onceOnly,
            _actionOnExists,
            _context
        );
    }
}