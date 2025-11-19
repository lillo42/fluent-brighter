using System;

using Fluent.Brighter.GoogleCloud;

namespace Fluent.Brighter;

/// <summary>
/// Extension methods for <see cref="GcpPublicationFactoryBuilder"/> to provide a fluent configuration API.
/// </summary>
public static class GcpPublicationFactoryBuilderExtensions
{
    /// <summary>
    /// Sets the Google Cloud Pub/Sub messaging gateway connection using a builder action.
    /// This overload allows configuring the connection inline without creating a separate connection object.
    /// </summary>
    /// <param name="builder">The <see cref="GcpPublicationFactoryBuilder"/> instance.</param>
    /// <param name="configure">An action to configure the <see cref="GcpMessagingGatewayConnectionBuilder"/>.</param>
    /// <returns>The <see cref="GcpPublicationFactoryBuilder"/> instance for method chaining.</returns>
    /// <example>
    /// <code>
    /// builder.SetConnection(conn => conn
    ///     .SetProjectId("my-gcp-project")
    ///     .SetCredential(GoogleCredential.GetApplicationDefault()));
    /// </code>
    /// </example>
    public static GcpPublicationFactoryBuilder SetConnection(this GcpPublicationFactoryBuilder builder,
        Action<GcpMessagingGatewayConnectionBuilder> configure)
    {
        var connection = new GcpMessagingGatewayConnectionBuilder();
        configure(connection);
        return builder.SetConnection(connection.Build());
    }
}