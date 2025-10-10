using System;

using Fluent.Brighter.MongoDb;

using Paramore.Brighter.MongoDb;

namespace Fluent.Brighter;

/// <summary>
/// Provides extension methods for <see cref="ConsumerBuilder"/> to configure MongoDB-based inbox support
/// using a fluent, composable API.
/// </summary>
public static class ConsumerBuilderExtensions
{
    /// <summary>
    /// Configures the MongoDB inbox using a fluent configuration delegate that builds a MongoDB connection and database settings.
    /// This overload creates a new <see cref="MongoDbConfiguration"/> internally and uses it to instantiate the inbox.
    /// </summary>
    /// <param name="builder">The <see cref="ConsumerBuilder"/> to extend.</param>
    /// <param name="configure">An action that configures a <see cref="MongoDbConfigurationBuilder"/>.</param>
    /// <returns>The updated <see cref="ConsumerBuilder"/> with MongoDB inbox support enabled.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="builder"/> or <paramref name="configure"/> is null.</exception>
    public static ConsumerBuilder UseMongoDbInbox(this ConsumerBuilder builder,
        Action<MongoDbConfigurationBuilder> configure)
    {
        if (configure == null)
        {
            throw new ArgumentNullException(nameof(configure));
        }
        
        var configuration = new MongoDbConfigurationBuilder();
        configure(configuration);
        return builder.UseMongoDbInbox(configuration.Build());
    }

    /// <summary>
    /// Configures the MongoDB inbox using a pre-built MongoDB configuration.
    /// This is useful when reusing a shared configuration across multiple Brighter components.
    /// </summary>
    /// <param name="builder">The <see cref="ConsumerBuilder"/> to extend.</param>
    /// <param name="configuration">An existing MongoDB configuration implementing <see cref="IAmAMongoDbConfiguration"/>.</param>
    /// <returns>The updated <see cref="ConsumerBuilder"/> with MongoDB inbox support enabled.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="builder"/> or <paramref name="configuration"/> is null.</exception>
    public static ConsumerBuilder UseMongoDbInbox(this ConsumerBuilder builder, IAmAMongoDbConfiguration configuration)
    {
        return builder.UseMongoDbInbox(cfg => cfg.SetConfiguration(configuration));
    }

    /// <summary>
    /// Configures the MongoDB inbox using a dedicated inbox builder, allowing fine-grained control
    /// over collection settings, connection providers, and other inbox-specific options.
    /// </summary>
    /// <param name="builder">The <see cref="ConsumerBuilder"/> to extend.</param>
    /// <param name="configure">An action that configures a <see cref="MongoDbInboxBuilder"/>.</param>
    /// <returns>The updated <see cref="ConsumerBuilder"/> with MongoDB inbox support enabled.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="builder"/> or <paramref name="configure"/> is null.</exception>
    public static ConsumerBuilder UseMongoDbInbox(this ConsumerBuilder builder, Action<MongoDbInboxBuilder> configure)
    {
        if (configure == null)
        {
            throw new ArgumentNullException(nameof(configure));
        }
        
        var inbox = new MongoDbInboxBuilder();
        configure(inbox);
        return builder.SetInbox(cfg => cfg.SetInbox(inbox.Build()));
    }
}
