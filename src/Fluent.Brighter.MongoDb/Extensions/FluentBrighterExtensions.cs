using System;

using Fluent.Brighter.MongoDb;

namespace Fluent.Brighter;

/// <summary>
/// Provides extension methods for <see cref="FluentBrighterBuilder"/> to enable fluent configuration
/// of MongoDB-backed Brighter features (inbox, outbox, distributed locking) via a dedicated configurator.
/// </summary>
public static class FluentBrighterExtensions
{
    /// <summary>
    /// Configures MongoDB integration for Brighter using a fluent configurator that supports inbox, outbox,
    /// and distributed locking setup with shared MongoDB connection settings.
    /// </summary>
    /// <param name="builder">The <see cref="FluentBrighterBuilder"/> to extend.</param>
    /// <param name="configure">An action that configures a <see cref="MongoDbConfigurator"/>.</param>
    /// <returns>The same <see cref="FluentBrighterBuilder"/> instance, enabling further chaining.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="builder"/> or <paramref name="configure"/> is null.
    /// </exception>
    /// <remarks>
    /// This method applies all configured MongoDB features (e.g., <c>UseInbox()</c>, <c>UseOutbox()</c>) to the builder
    /// in a single, cohesive step using a shared MongoDB configuration.
    /// </remarks>
    public static FluentBrighterBuilder UsingMongoDb(this FluentBrighterBuilder builder,
        Action<MongoDbConfigurator> configure)
    {
        if (configure == null)
        {
            throw new ArgumentNullException(nameof(configure));
        }
        
        var configurator = new MongoDbConfigurator();
        configure(configurator);
        configurator.SetFluentBrighter(builder);
        return builder;
    }
}