using System;

using Paramore.Brighter;
using Paramore.Brighter.Inbox;
using Paramore.Brighter.Inbox.MongoDb;
using Paramore.Brighter.MongoDb;

namespace Fluent.Brighter.MongoDb;

/// <summary>
/// A fluent builder for creating instances of <see cref="InboxConfiguration"/>.
/// Provides a clean, readable API for configuring inbox behavior including de-duplication, scope, and PostgreSQL-specific settings.
/// </summary>
public class MongoDbInboxBuilder
{
    // Base class constructor parameters
    private InboxScope _scope = InboxScope.All;
    private bool _onceOnly = true;
    private OnceOnlyAction _actionOnExists = OnceOnlyAction.Throw;
    private Func<Type, string>? _context;


    /// <summary>
    /// Sets the scope of requests stored in the inbox.
    /// Defaults to <see cref="InboxScope.All"/> meaning all requests are tracked.
    /// </summary>
    /// <param name="scope">The inbox scope.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public MongoDbInboxBuilder Scope(InboxScope scope)
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
    public MongoDbInboxBuilder OnceOnly(bool onceOnly)
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
    public MongoDbInboxBuilder ActionOnExists(OnceOnlyAction actionOnExists)
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
    public MongoDbInboxBuilder Context(Func<Type, string>? context)
    {
        _context = context;
        return this;
    }
    
    
    private MongoDbConfiguration? _configuration;

    /// <summary>
    /// Sets the MongoDB-specific database configuration.
    /// </summary>
    /// <param name="configuration">An instance of <see cref="MongoDbConfiguration"/>.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public MongoDbInboxBuilder Connection(MongoDbConfiguration? configuration)
    {
        _configuration = configuration;
        return this;
    }
    
    /// <summary>
    /// Sets the MongoDB-specific relational database configuration.
    /// </summary>
    /// <param name="configuration">The configuration setup.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public MongoDbInboxBuilder Connection(Action<MongoDbConfigurationBuilder> configuration)
    {
        var builder = new MongoDbConfigurationBuilder();
        configuration(builder);
        _configuration = builder.Build();
        return this;
    }

    /// <summary>
    /// Sets the MongoDB-specific database configuration if it was not set.
    /// </summary>
    /// <param name="configuration">An instance of <see cref="MongoDbConfiguration "/>.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public MongoDbInboxBuilder SetConnectionIfIsMissing(MongoDbConfiguration? configuration)
    {
        _configuration ??= configuration;
        return this;
    }

    /// <summary>
    /// Builds and returns a new instance of <see cref="InboxConfiguration"/> with the specified settings.
    /// </summary>
    /// <returns>A configured instance of <see cref="InboxConfiguration"/>.</returns>
    public InboxConfiguration Build()
    {
        if (_configuration == null)
        {
            throw new ConfigurationException("Configuration not configured");
        }
        
        return new InboxConfiguration(
            new MongoDbInbox(_configuration),
            _scope,
            _onceOnly,
            _actionOnExists,
            _context
        );
    }
}