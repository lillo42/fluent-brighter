using System.Collections.Generic;
using System.Linq;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.AWSSQS;

namespace Fluent.Brighter.Aws;

public sealed class SnsMessageProducerFactoryBuilder
{
    private AWSMessagingGatewayConnection? _connection;

    public SnsMessageProducerFactoryBuilder SetConfiguration(AWSMessagingGatewayConnection configuration)
    {
        _connection = configuration;
        return this;
    }

    private List<SnsPublication> _publications = [];

    public SnsMessageProducerFactoryBuilder SetPublications(params SnsPublication[] publications)
    {
        _publications = publications.ToList();
        return this;
    }

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