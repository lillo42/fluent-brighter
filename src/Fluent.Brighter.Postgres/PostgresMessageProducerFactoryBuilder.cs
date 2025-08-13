using System;
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
    public PostgresMessageProducerFactoryBuilder AddPostgresPublication(PostgresPublication publication)
    {
        _publications.Add(publication);
        return this;
    }
    
    public PostgresMessageProducerFactoryBuilder AddPostgresPublication(Action<PostgresPublicationBuilder> configuration)
    {
        var builder = new PostgresPublicationBuilder();
        configuration(builder);
        return AddPostgresPublication(builder.Build());
    }
    
    public PostgresMessageProducerFactoryBuilder AddPostgresPublication<TRequest>(Action<PostgresPublicationBuilder> configuration)
        where TRequest: class, IRequest
    {
        var builder = new PostgresPublicationBuilder();
        builder.SetRequestType(typeof(TRequest));
        configuration(builder);
        return AddPostgresPublication(builder.Build());
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