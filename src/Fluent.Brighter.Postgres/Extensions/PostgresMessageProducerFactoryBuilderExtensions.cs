using System;

using Fluent.Brighter.Postgres;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.Postgres;

namespace Fluent.Brighter;

/// <summary>
/// Provides extension methods for <see cref="PostgresMessageProducerFactoryBuilder"/> to simplify configuration.
/// These extensions enable fluent configuration of PostgreSQL message producer factories, including
/// connection settings and publication mappings.
/// </summary>
public static class PostgresMessageProducerFactoryBuilderExtensions
{
    #region SetConnection
    /// <summary>
    /// Sets the PostgreSQL messaging gateway connection using a pre-configured database configuration.
    /// This method creates a <see cref="PostgresMessagingGatewayConnection"/> from the provided configuration
    /// and sets it on the builder.
    /// </summary>
    /// <param name="builder">The <see cref="PostgresMessageProducerFactoryBuilder"/> instance to configure.</param>
    /// <param name="configuration">The pre-configured relational database configuration containing connection details.</param>
    /// <returns>The <see cref="PostgresMessageProducerFactoryBuilder"/> instance for method chaining.</returns>
    public static PostgresMessageProducerFactoryBuilder SetConnection(
        this PostgresMessageProducerFactoryBuilder builder,
        RelationalDatabaseConfiguration configuration) 
        => builder.SetConnection(new PostgresMessagingGatewayConnection(configuration));
    
    /// <summary>
    /// Sets the PostgreSQL messaging gateway connection using a fluent configuration builder.
    /// This extension method creates a <see cref="RelationalDatabaseConfigurationBuilder"/>, applies the provided configuration,
    /// and creates a <see cref="PostgresMessagingGatewayConnection"/> from the result.
    /// </summary>
    /// <param name="builder">The <see cref="PostgresMessageProducerFactoryBuilder"/> instance to configure.</param>
    /// <param name="configure">An action that configures the <see cref="RelationalDatabaseConfigurationBuilder"/> with connection details and settings.</param>
    /// <returns>The <see cref="PostgresMessageProducerFactoryBuilder"/> instance for method chaining.</returns>
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

    /// <summary>
    /// Adds a PostgreSQL publication to the producer factory using a configuration builder.
    /// Publications define how messages are published to PostgreSQL, including topic mappings and message metadata.
    /// </summary>
    /// <param name="builder">The <see cref="PostgresMessageProducerFactoryBuilder"/> instance to configure.</param>
    /// <param name="configure">An action that configures the <see cref="PostgresPublicationBuilder"/> with publication settings.</param>
    /// <returns>The <see cref="PostgresMessageProducerFactoryBuilder"/> instance for method chaining.</returns>
    public static PostgresMessageProducerFactoryBuilder AddPublication(
        this PostgresMessageProducerFactoryBuilder builder,
        Action<PostgresPublicationBuilder> configure)
    {
        var publication = new PostgresPublicationBuilder();
        configure(publication);
        return builder.AddPublication(publication.Build());
    }
    
    /// <summary>
    /// Adds a PostgreSQL publication for a specific request type to the producer factory.
    /// This method automatically configures the publication with the specified <typeparamref name="TRequest"/> type
    /// and then applies additional configuration provided by the action.
    /// </summary>
    /// <typeparam name="TRequest">The type of request/message that this publication will handle. Must implement <see cref="IRequest"/>.</typeparam>
    /// <param name="builder">The <see cref="PostgresMessageProducerFactoryBuilder"/> instance to configure.</param>
    /// <param name="configure">An action that configures the <see cref="PostgresPublicationBuilder"/> with additional publication settings.</param>
    /// <returns>The <see cref="PostgresMessageProducerFactoryBuilder"/> instance for method chaining.</returns>
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