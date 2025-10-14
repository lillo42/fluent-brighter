using System;

using Fluent.Brighter.SqlServer;

using Paramore.Brighter;

namespace Fluent.Brighter;

/// <summary>
/// Provides extension methods for the <see cref="SqlServerMessageProducerFactoryBuilder"/> to enable fluent configuration
/// of SQL Server-based message producer factories in the Fluent Brighter library.
/// </summary>
/// <remarks>
/// These extensions simplify setting up database connections and publications using inline builder actions,
/// improving readability and reducing boilerplate. Part of the Fluent Brighter library
/// (<see href="https://github.com/lillo42/fluent-brighter/"/>).
/// </remarks>
public static class SqlServerMessageProducerFactoryBuilderExtensions
{
    #region SetConnection

    /// <summary>
    /// Configures the SQL Server connection for the message producer factory using a fluent builder action.
    /// </summary>
    /// <param name="builder">The <see cref="SqlServerMessageProducerFactoryBuilder"/> to extend.</param>
    /// <param name="configure">An action that customizes the <see cref="RelationalDatabaseConfigurationBuilder"/> to define connection settings (e.g., connection string, schema).</param>
    /// <returns>The updated <see cref="SqlServerMessageProducerFactoryBuilder"/> to allow method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="configure"/> is null.</exception>
    public static SqlServerMessageProducerFactoryBuilder SetConnection(
        this SqlServerMessageProducerFactoryBuilder builder,
        Action<RelationalDatabaseConfigurationBuilder> configure)
    {
        if (configure == null)
        {
            throw new ArgumentNullException(nameof(configure));
        }
        
        var configuration = new RelationalDatabaseConfigurationBuilder();
        configure(configuration);
        return builder.SetConnection(configuration.Build());
    }

    #endregion

    #region AddPublication

    /// <summary>
    /// Adds a publication by configuring it fluently using a <see cref="SqlServerPublicationBuilder"/>.
    /// </summary>
    /// <param name="builder">The <see cref="SqlServerMessageProducerFactoryBuilder"/> to extend.</param>
    /// <param name="configure">An action that customizes the publication builder (e.g., topic, source, CloudEvents metadata).</param>
    /// <returns>The updated <see cref="SqlServerMessageProducerFactoryBuilder"/> to allow method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="configure"/> is null.</exception>
    public static SqlServerMessageProducerFactoryBuilder AddPublication(
        this SqlServerMessageProducerFactoryBuilder builder,
        Action<SqlServerPublicationBuilder> configure)
    {
        if (configure == null)
        {
            throw new ArgumentNullException(nameof(configure));
        }
        
        var publication = new SqlServerPublicationBuilder();
        configure(publication);
        return builder.AddPublication(publication.Build());
    }

    /// <summary>
    /// Adds a publication for a specific request type <typeparamref name="TRequest"/>,
    /// automatically setting the request type and allowing further customization via a builder action.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request/command being published. Must implement <see cref="IRequest"/>.</typeparam>
    /// <param name="builder">The <see cref="SqlServerMessageProducerFactoryBuilder"/> to extend.</param>
    /// <param name="configure">An action to further configure the publication builder (e.g., routing, CloudEvents attributes).</param>
    /// <returns>The updated <see cref="SqlServerMessageProducerFactoryBuilder"/> to allow method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="configure"/> is null.</exception>
    public static SqlServerMessageProducerFactoryBuilder AddPublication<TRequest>(
        this SqlServerMessageProducerFactoryBuilder builder,
        Action<SqlServerPublicationBuilder> configure)
        where TRequest : class, IRequest
    {
        if (configure == null)
        {
            throw new ArgumentNullException(nameof(configure));
        }
        
        var publication = new SqlServerPublicationBuilder();
        publication.SetRequestType(typeof(TRequest));
        configure(publication);
        return builder.AddPublication(publication.Build());
    }

    #endregion
}