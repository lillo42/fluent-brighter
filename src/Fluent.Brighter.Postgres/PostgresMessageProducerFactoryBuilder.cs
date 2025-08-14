using System.Collections.Generic;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.Postgres;

namespace Fluent.Brighter.Postgres;

public sealed class PostgresMessageProducerFactoryBuilder
{
    private PostgresMessagingGatewayConnection? _connection;

    public PostgresMessageProducerFactoryBuilder SetConnection(PostgresMessagingGatewayConnection connection)
    {
        _connection = connection;
        return this;
    }
    
    private readonly List<PostgresPublication> _publications = [];
    public PostgresMessageProducerFactoryBuilder AddPublication(PostgresPublication publication)
    {
        _publications.Add(publication);
        return this;
    }

    internal PostgresMessageProducerFactory Build()
    {
        if (_connection == null)
        {
            throw new ConfigurationException("No connection configured");
        }
        
        return new PostgresMessageProducerFactory(_connection, _publications);
    }
}