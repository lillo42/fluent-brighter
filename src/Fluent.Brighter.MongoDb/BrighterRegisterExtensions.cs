using System;

namespace Fluent.Brighter.MongoDb;

/// <summary>
/// Provides extension methods for integrating MongoDB-based infrastructure with Brighter service configuration.
/// Enables fluent configuration of MongoDB as a messaging gateway and persistence store.
/// </summary>
/// <remarks>
/// This class simplifies MongoDB integration with Brighter's pipeline by providing a fluent API for configuring:
/// - MongoDB connections
/// - Outbox pattern implementation
/// - Inbox pattern implementation
/// - Distributed locking
/// - Luggage store for message attachments
/// </remarks>
public static class BrighterRegisterExtensions
{
    /// <summary>
    /// Configures MongoDB integration for Brighter messaging using a fluent <see cref="MongoDbConfigurator"/> setup.
    /// Applies the provided configuration to set up MongoDB-based infrastructure services.
    /// </summary>
    /// <param name="configurator">The Brighter configurator to extend.</param>
    /// <param name="configure">An action to customize the <see cref="MongoDbConfigurator"/>.</param>
    /// <returns>The updated <see cref="IBrighterConfigurator"/> instance with MongoDB services registered.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="configurator"/> or <paramref name="configure"/> is null.</exception>
    /// <example>
    /// <code>
    /// services.AddBrighter(config =>
    /// {
    ///     config.UsingMongoDb(mongo =>
    ///     {
    ///         mongo.Connection(builder => builder
    ///             .ConnectionString("mongodb://localhost:27017")
    ///             .Database("MyDatabase")
    ///             .BucketName("MyBucket"));
    ///
    ///         mongo.UsingOutbox();
    ///         mongo.UsingInbox();
    ///     });
    /// });
    /// </code>
    /// </example>
    public static IBrighterConfigurator UsingMongoDb(this IBrighterConfigurator configurator, Action<MongoDbConfigurator> configure)
    {
        if (configurator == null)
        {
            throw new ArgumentNullException(nameof(configurator));
        }

        if (configure == null)
        {
            throw new ArgumentNullException(nameof(configure));
        }

        var postgresConfigurator = new MongoDbConfigurator();
        configure(postgresConfigurator);
        return postgresConfigurator.AddMongoDb(configurator);
    }
}