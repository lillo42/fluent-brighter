using System;

using Fluent.Brighter.GoogleCloud;

namespace Fluent.Brighter;

/// <summary>
/// Extension methods for <see cref="FirestoreOutboxBuilder"/> to provide convenient configuration
/// of Firestore settings using builder pattern callbacks.
/// </summary>
public static class FirestoreOutboxBuilderExtensions
{
    /// <summary>
    /// Sets the Firestore configuration using a fluent configuration callback.
    /// </summary>
    /// <param name="builder">The Firestore outbox builder instance</param>
    /// <param name="configure">An action to configure the Firestore configuration builder</param>
    /// <returns>The Firestore outbox builder instance for method chaining</returns>
    public static FirestoreOutboxBuilder SetConfiguration(this FirestoreOutboxBuilder builder,
        Action<FirestoreConfigurationBuilder> configure)
    {
        var configuration = new FirestoreConfigurationBuilder();
        configure(configuration);
        return builder.SetConfiguration(configuration.Build());
    }
}