using System.Collections.Generic;
using System.Linq;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.RMQ.Sync;

namespace Fluent.Brighter.RMQ.Sync;

public sealed class RmqMessageProducerFactoryBuilder
{
    private RmqMessagingGatewayConnection? _connection;

    public RmqMessageProducerFactoryBuilder SetConnection(RmqMessagingGatewayConnection connection)
    {
        _connection = connection;
        return this;
    }
    
    private List<RmqPublication> _publications = [];

    public RmqMessageProducerFactoryBuilder SetPublications(params RmqPublication[] publications)
    {
        _publications = publications.ToList();
        return this;
    }

    public RmqMessageProducerFactoryBuilder AddPublication(RmqPublication publication)
    {
        _publications.Add(publication);
        return this;
    }

    internal RmqMessageProducerFactory Build()
    {
        if (_connection == null)
        {
            throw new ConfigurationException("Connection is null");
        }
        
        return new RmqMessageProducerFactory(_connection, _publications);
    }
}