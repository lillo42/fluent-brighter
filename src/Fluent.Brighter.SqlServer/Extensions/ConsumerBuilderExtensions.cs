using System;

using Fluent.Brighter.SqlServer;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.MsSql;

namespace Fluent.Brighter;

/// <summary>
/// Provides extension methods for the <see cref="ConsumerBuilder"/> to configure SQL Server-based
/// subscriptions, inbox integration, and channel factories within the Fluent Brighter messaging framework.
/// </summary>
public static class ConsumerBuilderExtensions
{
    /// <summary>
    /// Adds a pre-configured SQL Server subscription to the consumer.
    /// </summary>
    /// <param name="builder">The consumer builder to extend.</param>
    /// <param name="subscription">The subscription configuration.</param>
    /// <returns>The updated <see cref="ConsumerBuilder"/> to allow method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="subscription"/> is null.</exception>
    public static ConsumerBuilder AddMicrosoftSqlServerSubscription(this ConsumerBuilder builder, Subscription subscription)
    {
        return builder.AddSubscription(subscription ?? throw new ArgumentNullException(nameof(subscription)));
    }

    /// <summary>
    /// Adds a SQL Server subscription by configuring it fluently using a <see cref="SqlServerSubscriptionBuilder"/>.
    /// </summary>
    /// <param name="builder">The consumer builder to extend.</param>
    /// <param name="configure">An action that customizes the subscription builder.</param>
    /// <returns>The updated <see cref="ConsumerBuilder"/> to allow method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="configure"/> is null.</exception>
    public static ConsumerBuilder AddMicrosoftSqlServerSubscription(this ConsumerBuilder builder,
        Action<SqlServerSubscriptionBuilder> configure)
    {
        if (configure == null)
        { 
            throw new ArgumentNullException(nameof(configure));
        }
        
        var sub = new SqlServerSubscriptionBuilder();
        configure(sub);
        return builder.AddSubscription(sub.Build());
    }
    
    /// <summary>
    /// Adds a SQL Server subscription for a specific request type <typeparamref name="TRequest"/>,
    /// automatically setting the data type and allowing further customization.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request/command being consumed. Must implement <see cref="IRequest"/>.</typeparam>
    /// <param name="builder">The consumer builder to extend.</param>
    /// <param name="configure">An action to further configure the subscription builder.</param>
    /// <returns>The updated <see cref="ConsumerBuilder"/> to allow method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="configure"/> is null.</exception>
    public static ConsumerBuilder AddMicrosoftSqlServerSubscription<TRequest>(this ConsumerBuilder builder,
        Action<SqlServerSubscriptionBuilder> configure)
        where TRequest : class, IRequest
    {
        if (configure == null)
        { 
            throw new ArgumentNullException(nameof(configure));
        }
        
        var sub = new SqlServerSubscriptionBuilder();
        sub.SetDataType(typeof(TRequest));
        configure(sub);
        return builder.AddSubscription(sub.Build());
    }
    
    /// <summary>
    /// Configures the consumer to use a SQL Server-based inbox for message deduplication,
    /// using a fluent configuration action for database settings (e.g., connection string, schema).
    /// </summary>
    /// <param name="builder">The consumer builder to extend.</param>
    /// <param name="configure">An action that configures the relational database connection.</param>
    /// <returns>The updated <see cref="ConsumerBuilder"/> to allow method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="configure"/> is null.</exception>
    public static ConsumerBuilder UseMicrosoftSqlServerInbox(this ConsumerBuilder builder, 
        Action<RelationalDatabaseConfigurationBuilder> configure)
    {
        if (configure == null)
        { 
            throw new ArgumentNullException(nameof(configure));
        }
        
        var configuration = new RelationalDatabaseConfigurationBuilder();
        configure(configuration);
        return builder.UseMicrosoftSqlServerInbox(configuration.Build());
    }

    /// <summary>
    /// Configures the consumer to use a SQL Server-based inbox with an explicit database configuration.
    /// </summary>
    /// <param name="builder">The consumer builder to extend.</param>
    /// <param name="configuration">The relational database configuration.</param>
    /// <returns>The updated <see cref="ConsumerBuilder"/> to allow method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="configuration"/> is null.</exception>
    public static ConsumerBuilder UseMicrosoftSqlServerInbox(this ConsumerBuilder builder, IAmARelationalDatabaseConfiguration configuration)
    {
        return builder.UseMicrosoftSqlServerInbox(cfg => cfg.SetConfiguration(configuration ?? throw new ArgumentNullException(nameof(configuration))));
    }

    /// <summary>
    /// Configures the consumer to use a SQL Server-based inbox via a fully customized <see cref="SqlServerInboxBuilder"/>.
    /// </summary>
    /// <param name="builder">The consumer builder to extend.</param>
    /// <param name="configure">An action that configures the inbox builder (e.g., connection provider, configuration).</param>
    /// <returns>The updated <see cref="ConsumerBuilder"/> to allow method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="configure"/> is null.</exception>
    public static ConsumerBuilder UseMicrosoftSqlServerInbox(this ConsumerBuilder builder, Action<SqlServerInboxBuilder> configure)
    {
        if (configure == null)
        {
            throw new ArgumentNullException(nameof(configure));
        }
        
        var inbox = new SqlServerInboxBuilder();
        configure(inbox);
        return builder.SetInbox(cfg => cfg.SetInbox(inbox.Build()));
    }

    /// <summary>
    /// Adds a SQL Server channel factory using a fluent configuration action for database settings.
    /// This enables message consumption from SQL Server-backed queues (e.g., via outbox/inbox tables).
    /// </summary>
    /// <param name="builder">The consumer builder to extend.</param>
    /// <param name="configure">An action that configures the relational database connection.</param>
    /// <returns>The updated <see cref="ConsumerBuilder"/> to allow method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="configure"/> is null.</exception>
    public static ConsumerBuilder AddMicrosoftSqlServerChannelFactory(this ConsumerBuilder builder,
        Action<RelationalDatabaseConfigurationBuilder> configure)
    {
        var configuration = new RelationalDatabaseConfigurationBuilder();
        configure(configuration);
        return builder.AddMicrosoftSqlServerChannelFactory(configuration.Build());
    }

    /// <summary>
    /// Adds a SQL Server channel factory using an explicit database configuration.
    /// </summary>
    /// <param name="builder">The consumer builder to extend.</param>
    /// <param name="configuration">The relational database configuration containing connection details.</param>
    /// <returns>The updated <see cref="ConsumerBuilder"/> to allow method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="configuration"/> is null.</exception>
    public static ConsumerBuilder AddMicrosoftSqlServerChannelFactory(this ConsumerBuilder builder, RelationalDatabaseConfiguration configuration)
    {
        return builder
            .AddChannelFactory(new ChannelFactory(new MsSqlMessageConsumerFactory(configuration ?? throw new ArgumentNullException(nameof(configuration)))));
    }
}