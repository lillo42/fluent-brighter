using System;

using Fluent.Brighter.GoogleCloud;

namespace Fluent.Brighter;

/// <summary>
/// Extension methods for <see cref="FirestoreInboxBuilder"/> to provide convenient configuration
/// of Firestore settings using builder pattern callbacks.
/// </summary>
public static class FirestoreInboxBuilderExtensions
{
    /// <summary>
    /// Sets the Firestore configuration using a fluent configuration callback.
    /// </summary>
    /// <param name="builder">The Firestore inbox builder instance</param>
    /// <param name="configure">An action to configure the Firestore configuration builder</param>
    /// <returns>The Firestore inbox builder instance for method chaining</returns>
    public static FirestoreInboxBuilder SetConfiguration(this FirestoreInboxBuilder builder,
        Action<FirestoreConfigurationBuilder> configure)
    {
        var configuration = new FirestoreConfigurationBuilder();
        configure(configuration);
        return builder.SetConfiguration(configuration.Build());
    }
}