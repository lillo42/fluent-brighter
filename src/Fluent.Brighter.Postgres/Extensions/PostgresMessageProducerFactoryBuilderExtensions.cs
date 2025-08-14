using System;

using Fluent.Brighter.Postgres;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.Postgres;

namespace Fluent.Brighter;

public static class PostgresMessageProducerFactoryBuilderExtensions
{
    #region SetConnection
    public static PostgresMessageProducerFactoryBuilder SetConnection(
        this PostgresMessageProducerFactoryBuilder builder,
        RelationalDatabaseConfiguration configuration) 
        => builder.SetConnection(new PostgresMessagingGatewayConnection(configuration));
    
    public static PostgresMessageProducerFactoryBuilder SetConnection(
        this PostgresMessageProducerFactoryBuilder builder,
        Action<RelationalDatabaseConfigurationBuilder> configure)
    {
        var configuration = new RelationalDatabaseConfigurationBuilder();
        configure(configuration);
        return builder.SetConnection(new PostgresMessagingGatewayConnection(configuration.Build()));
    }
    #endregion
    
    #region AddPublication

    public static PostgresMessageProducerFactoryBuilder AddPublication(
        this PostgresMessageProducerFactoryBuilder builder,
        Action<PostgresPublicationBuilder> configure)
    {
        var publication = new PostgresPublicationBuilder();
        configure(publication);
        return builder.AddPublication(publication.Build());
    }
    
    public static PostgresMessageProducerFactoryBuilder AddPublication<TRequest>(
        this PostgresMessageProducerFactoryBuilder builder,
        Action<PostgresPublicationBuilder> configure)
        where TRequest: class, IRequest
    {
        var publication = new PostgresPublicationBuilder();
        publication.SetRequestType(typeof(TRequest));
        configure(publication);
        return builder.AddPublication(publication.Build());
    }
    #endregion
}