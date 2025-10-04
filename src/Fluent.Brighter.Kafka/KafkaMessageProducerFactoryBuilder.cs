using System.Collections.Generic;
using System.Linq;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.Kafka;

namespace Fluent.Brighter.Kafka;

/// <summary>
/// Provides a fluent builder for configuring a Kafka message producer factory
/// in a Brighter integration with Apache Kafka, allowing setup of connection settings
/// and publication profiles.
/// </summary>
public sealed class KafkaMessageProducerFactoryBuilder
{
    private KafkaMessagingGatewayConfiguration? _configuration;

    /// <summary>
    /// Sets the Kafka messaging gateway configuration used by the producer factory.
    /// </summary>
    /// <param name="configuration">The Kafka connection configuration.</param>
    /// <returns>The current builder instance for chaining.</returns>
    public KafkaMessageProducerFactoryBuilder SetConfiguration(KafkaMessagingGatewayConfiguration configuration)
    {
        _configuration = configuration;
        return this;
    }

    private List<KafkaPublication> _publications = [];

    /// <summary>
    /// Replaces the current list of publications with the provided set.
    /// </summary>
    /// <param name="publications">One or more Kafka publication configurations.</param>
    /// <returns>The current builder instance for chaining.</returns>
    public KafkaMessageProducerFactoryBuilder SetPublications(params KafkaPublication[] publications)
    {
        _publications = publications.ToList();
        return this;
    }
    
    /// <summary>
    /// Adds a single Kafka publication configuration to the factory.
    /// </summary>
    /// <param name="publications">The publication to add.</param>
    /// <returns>The current builder instance for chaining.</returns>
    public KafkaMessageProducerFactoryBuilder AddPublication(KafkaPublication publications)
    {
        _publications.Add(publications);
        return this;
    }

    /// <summary>
    /// Builds a new instance of <see cref="KafkaMessageProducerFactory"/> with the configured settings.
    /// </summary>
    /// <returns>A fully configured Kafka message producer factory.</returns>
    /// <exception cref="ConfigurationException">Thrown if no Kafka configuration has been set.</exception>
    internal KafkaMessageProducerFactory Build()
    {
        if (_configuration == null)
        {
            throw new ConfigurationException("The configuration was not set");
        }
        
        return new KafkaMessageProducerFactory(_configuration, _publications);
    }
}