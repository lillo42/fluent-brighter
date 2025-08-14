using System.Collections.Generic;
using System.Linq;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.Kafka;

namespace Fluent.Brighter.Kafka;

public sealed class KafkaMessageProducerFactoryBuilder
{
    private KafkaMessagingGatewayConfiguration? _configuration;

    public KafkaMessageProducerFactoryBuilder SetConfiguration(KafkaMessagingGatewayConfiguration configuration)
    {
        _configuration = configuration;
        return this;
    }

    private List<KafkaPublication> _publications = [];

    public KafkaMessageProducerFactoryBuilder SetPublications(params KafkaPublication[] publications)
    {
        _publications = publications.ToList();
        return this;
    }
    
    public KafkaMessageProducerFactoryBuilder AddPublication(KafkaPublication publications)
    {
        _publications.Add(publications);
        return this;
    }

    internal KafkaMessageProducerFactory Build()
    {
        if (_configuration == null)
        {
            throw new ConfigurationException("The configureation was not set");
        }
        
        return new KafkaMessageProducerFactory(_configuration, _publications);
    }
}