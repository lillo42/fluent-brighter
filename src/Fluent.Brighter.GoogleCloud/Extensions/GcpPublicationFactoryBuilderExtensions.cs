using System;

using Fluent.Brighter.GoogleCloud;

using Paramore.Brighter;

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
    
    /// <summary>
    /// Adds a message publication configuration to the Google Cloud Pub/Sub message producer factory.
    /// This overload allows configuring publication settings using a builder action without specifying a request type.
    /// </summary>
    /// <param name="builder">The <see cref="GcpPublicationFactoryBuilder"/> instance.</param>
    /// <param name="configure">An action to configure the <see cref="GcpPublicationBuilder"/>.</param>
    /// <returns>The <see cref="GcpPublicationFactoryBuilder"/> instance for method chaining.</returns>
    /// <example>
    /// <code>
    /// builder.AddPublication(pub => pub
    ///     .SetTopicAttributes(attrs => attrs.SetName("my-topic"))
    ///     .SetSource("https://example.com/events"));
    /// </code>
    /// </example>
    public static GcpPublicationFactoryBuilder AddPublication(this GcpPublicationFactoryBuilder builder,
        Action<GcpPublicationBuilder> configure)
    {
        var connection = new GcpPublicationBuilder();
        configure(connection);
        return builder.AddPublication(connection.Build());
    }
    
    /// <summary>
    /// Adds a message publication configuration to the Google Cloud Pub/Sub message producer factory for a specific request type.
    /// This overload automatically associates the publication with the specified request type using generics.
    /// </summary>
    /// <typeparam name="T">The type of request (command or event) this publication handles. Must implement <see cref="IRequest"/>.</typeparam>
    /// <param name="builder">The <see cref="GcpPublicationFactoryBuilder"/> instance.</param>
    /// <param name="configure">An action to configure the <see cref="GcpPublicationBuilder"/>.</param>
    /// <returns>The <see cref="GcpPublicationFactoryBuilder"/> instance for method chaining.</returns>
    /// <example>
    /// <code>
    /// builder.AddPublication&lt;MyCommand&gt;(pub => pub
    ///     .SetTopicAttributes(attrs => attrs
    ///         .SetName("my-command-topic")
    ///         .SetLabels(new Dictionary&lt;string, string&gt; { ["env"] = "production" }))
    ///     .SetSource("https://example.com/commands")
    ///     .SetSubject("MyCommand"));
    /// </code>
    /// </example>
    public static GcpPublicationFactoryBuilder AddPublication<T>(this GcpPublicationFactoryBuilder builder,
        Action<GcpPublicationBuilder> configure)
    where T : class, IRequest
    {
        var connection = new GcpPublicationBuilder();
        connection.SetRequestType(typeof(T));
        configure(connection);
        return builder.AddPublication(connection.Build());
    }
}