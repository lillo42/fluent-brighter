using System;

using Fluent.Brighter.GoogleCloud;

namespace Fluent.Brighter;

/// <summary>
/// Extension methods for <see cref="FirestoreLockingBuilder"/> to provide fluent configuration options.
/// </summary>
public static class FirestoreLockingBuilderExtensions
{
    /// <summary>
    /// Sets the Firestore configuration using a fluent configuration builder.
    /// </summary>
    /// <param name="builder">The <see cref="FirestoreLockingBuilder"/> instance.</param>
    /// <param name="configure">An action to configure the <see cref="FirestoreConfigurationBuilder"/>.</param>
    /// <returns>The builder instance for method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="builder"/> or <paramref name="configure"/> is null.</exception>
    /// <example>
    /// <code>
    /// builder.SetConfiguration(config => config
    ///     .SetProjectId("my-project")
    ///     .SetDatabase("(default)")
    ///     .SetLocking(locking => locking
    ///         .SetName("locks")
    ///         .SetTtl(TimeSpan.FromMinutes(5))));
    /// </code>
    /// </example>
    public static FirestoreLockingBuilder SetConfiguration(this FirestoreLockingBuilder builder,
        Action<FirestoreConfigurationBuilder> configure)
    {
        var configuration = new FirestoreConfigurationBuilder();
        configure(configuration);
        return builder.SetConfiguration(configuration.Build());
    }
}