using System.Collections.Generic;
using System.Linq;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.AWSSQS.V4;

namespace Fluent.Brighter.AWS.V4;

public sealed class SqsMessageProducerFactoryBuilder
{
    private AWSMessagingGatewayConnection? _connection;

    public SqsMessageProducerFactoryBuilder SetConfiguration(AWSMessagingGatewayConnection configuration)
    {
        _connection = configuration;
        return this;
    }

    private List<SqsPublication> _publications = [];

    public SqsMessageProducerFactoryBuilder SetPublications(params SqsPublication[] publications)
    {
        _publications = publications.ToList();
        return this;
    }

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