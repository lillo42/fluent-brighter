using System.Collections.Generic;
using System.Linq;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.AWSSQS;

namespace Fluent.Brighter.AWS;

/// <summary>
/// Builder class for creating and configuring an SNS message producer factory.
/// Provides methods to set AWS connection configuration and register SNS publications
/// for message routing in Paramore.Brighter.
/// </summary>
public sealed class SnsMessageProducerFactoryBuilder
{
    private AWSMessagingGatewayConnection? _connection;

    /// <summary>
    /// Sets the AWS messaging gateway connection configuration including
    /// credentials, region, and other AWS-specific settings.
    /// </summary>
    /// <param name="configuration">The AWS connection configuration</param>
    /// <returns>The builder instance for method chaining</returns>
    public SnsMessageProducerFactoryBuilder SetConfiguration(AWSMessagingGatewayConnection configuration)
    {
        _connection = configuration;
        return this;
    }

    private List<SnsPublication> _publications = [];

    /// <summary>
    /// Sets the collection of SNS publications that define how messages
    /// should be routed to specific SNS topics.
    /// </summary>
    /// <param name="publications">Array of SNS publication configurations</param>
    /// <returns>The builder instance for method chaining</returns>
    public SnsMessageProducerFactoryBuilder SetPublications(params SnsPublication[] publications)
    {
        _publications = publications.ToList();
        return this;
    }

    /// <summary>
    /// Adds a single SNS publication configuration to the collection
    /// of registered publications.
    /// </summary>
    /// <param name="publication">SNS publication configuration to add</param>
    /// <returns>The builder instance for method chaining</returns>
    public SnsMessageProducerFactoryBuilder AddPublication(SnsPublication publication)
    {
        _publications.Add(publication);
        return this;
    }

    internal SnsMessageProducerFactory Build()
    {
        if (_connection == null)
        {
            throw new ConfigurationException("Connection is not configured");
        }

        return new SnsMessageProducerFactory(_connection, _publications);
    }
}