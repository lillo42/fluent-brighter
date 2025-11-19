using System;
using System.Collections.Generic;
using System.Linq;

using Paramore.Brighter.MessagingGateway.GcpPubSub;
using Paramore.Brighter.Observability;

namespace Fluent.Brighter.GoogleCloud;

/// <summary>
/// Builder class for configuring and creating a <see cref="GcpPubSubMessageProducerFactory"/>.
/// Provides a fluent interface for setting up Google Cloud Pub/Sub message producer configurations.
/// </summary>
public class GcpPublicationFactoryBuilder
{
    private GcpMessagingGatewayConnection? _connection;
    
    /// <summary>
    /// Sets the connection details for the Google Cloud Pub/Sub gateway.
    /// </summary>
    /// <param name="connection">The connection configuration including credentials and project settings.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public GcpPublicationFactoryBuilder SetConnection(GcpMessagingGatewayConnection connection)
    {
        _connection = connection;
        return this;
    }

    private List<GcpPublication> _publications = new();
    
    /// <summary>
    /// Sets the collection of Google Cloud Pub/Sub specific publication configurations.
    /// Replaces any previously configured publications.
    /// </summary>
    /// <param name="publications">The publications to configure.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public GcpPublicationFactoryBuilder SetPublications(params GcpPublication[] publications)
    {
        _publications = publications.ToList();
        return this;
    }

    /// <summary>
    /// Adds a single publication to the collection.
    /// Can be called multiple times to add multiple publications.
    /// </summary>
    /// <param name="publication">The publication to add.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public GcpPublicationFactoryBuilder AddPublication(GcpPublication publication)
    {
        _publications.Add(publication);
        return this;
    }

    private InstrumentationOptions? _instrumentation;
    
    /// <summary>
    /// Sets the instrumentation options for tracing and metrics.
    /// </summary>
    /// <param name="instrumentation">The instrumentation options to enable observability features.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public GcpPublicationFactoryBuilder SetInstrumentation(InstrumentationOptions instrumentation)
    {
        _instrumentation = instrumentation;
        return this;
    }

    /// <summary>
    /// Builds the <see cref="GcpPubSubMessageProducerFactory"/> with the configured settings.
    /// </summary>
    /// <returns>A new instance of <see cref="GcpPubSubMessageProducerFactory"/>.</returns>
    /// <exception cref="InvalidOperationException">Thrown if connection is not set.</exception>
    internal GcpPubSubMessageProducerFactory Build()
    {
        if (_connection == null)
        {
            throw new InvalidOperationException("Connection must be set before building the factory.");
        }

        return new GcpPubSubMessageProducerFactory(_connection, _publications, _instrumentation);
    }
}