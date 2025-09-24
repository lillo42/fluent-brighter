using System.Collections.Generic;
using System.Linq;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.AWSSQS;

namespace Fluent.Brighter.AWS;

/// <summary>
/// Builder class for creating and configuring an SQS message producer factory.
/// Provides methods to set AWS connection configuration and register SQS publications
/// for message routing to Amazon SQS queues in Paramore.Brighter.
/// </summary>
public sealed class SqsMessageProducerFactoryBuilder
{
    private AWSMessagingGatewayConnection? _connection;

    /// <summary>
    /// Sets the AWS messaging gateway connection configuration including
    /// credentials, region, and other AWS-specific settings for SQS access.
    /// </summary>
    /// <param name="configuration">The AWS connection configuration</param>
    /// <returns>The builder instance for method chaining</returns>
    public SqsMessageProducerFactoryBuilder SetConfiguration(AWSMessagingGatewayConnection configuration)
    {
        _connection = configuration;
        return this;
    }

    private List<SqsPublication> _publications = [];

    /// <summary>
    /// Sets the collection of SQS publications that define how messages
    /// should be routed to specific SQS queues.
    /// </summary>
    /// <param name="publications">Array of SQS publication configurations</param>
    /// <returns>The builder instance for method chaining</returns>
    public SqsMessageProducerFactoryBuilder SetPublications(params SqsPublication[] publications)
    {
        _publications = publications.ToList();
        return this;
    }

    /// <summary>
    /// Adds a single SQS publication configuration to the collection
    /// of registered publications.
    /// </summary>
    /// <param name="publication">SQS publication configuration to add</param>
    /// <returns>The builder instance for method chaining</returns>
    public SqsMessageProducerFactoryBuilder AddPublication(SqsPublication publication)
    {
        _publications.Add(publication);
        return this;
    }

    internal SqsMessageProducerFactory Build()
    {
        if (_connection == null)
        {
            throw new ConfigurationException("Connection is not configured");
        }

        return new SqsMessageProducerFactory(_connection, _publications);
    }
}