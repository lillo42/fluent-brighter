using System;

using MongoDB.Driver;

using Paramore.Brighter;
using Paramore.Brighter.MongoDb;

namespace Fluent.Brighter.MongoDb;

/// <summary>
/// A high-level configurator that integrates MongoDB-based Brighter features (inbox, outbox, distributed locking)
/// into a <see cref="FluentBrighterBuilder"/> using a shared MongoDB configuration.
/// </summary>
public sealed class MongoDbConfigurator
{
    private string? _connectionString;
    private IAmAMongoDbConfiguration? _configuration;
    private Action<FluentBrighterBuilder> _action = _ => { };

    /// <summary>
    /// Configures the MongoDB connection using a fluent builder delegate.
    /// </summary>
    /// <param name="configuration">An action that configures a <see cref="MongoDbConfigurationBuilder"/>.</param>
    /// <returns>The current <see cref="MongoDbConfigurator"/> instance for method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="configuration"/> is null.</exception>
    /// <exception cref="ConfigurationException">Thrown if the builder delegate fails to produce a valid configuration.</exception>
    public MongoDbConfigurator SetConnection(Action<MongoDbConfigurationBuilder> configuration)
    {
        if (configuration == null)
        {
            throw new ArgumentNullException(nameof(configuration));
        }
        
        var builder = new MongoDbConfigurationBuilder();
        configuration(builder);
        _connectionString = builder.ConnectionString;
        return SetConnection(builder.Build());
    }

    /// <summary>
    /// Sets an existing MongoDB configuration to be used by all MongoDB-based Brighter components.
    /// </summary>
    /// <param name="configuration">The pre-built MongoDB configuration implementing <see cref="IAmAMongoDbConfiguration"/>.</param>
    /// <returns>The current <see cref="MongoDbConfigurator"/> instance for method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="configuration"/> is null.</exception>
    public MongoDbConfigurator SetConnection(IAmAMongoDbConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        return this;
    }

    /// <summary>
    /// Enables the inbox pattern using the shared MongoDB configuration.
    /// The inbox ensures idempotent handling of incoming messages.
    /// </summary>
    /// <returns>The current <see cref="MongoDbConfigurator"/> instance for method chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown if called before <see cref="SetConnection(Paramore.Brighter.MongoDb.IAmAMongoDbConfiguration)"/>.</exception>
    public MongoDbConfigurator UseInbox()
    {
        UseInbox("inbox");
        return this;
    }

    /// <summary>
    /// Enables the inbox pattern with a custom collection name, using the shared MongoDB configuration.
    /// </summary>
    /// <param name="collectionName">The name of the MongoDB collection to use for the inbox.</param>
    /// <returns>The current <see cref="MongoDbConfigurator"/> instance for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown if <paramref name="collectionName"/> is null or empty.</exception>
    /// <exception cref="InvalidOperationException">Thrown if called before <see cref="SetConnection(Paramore.Brighter.MongoDb.IAmAMongoDbConfiguration)"/>.</exception>
    public MongoDbConfigurator UseInbox(string collectionName)
    {
        if (string.IsNullOrEmpty(collectionName))
        {
            throw new ArgumentException("Collection name cannot be null or empty.", nameof(collectionName));
        }

        _action += fluent => fluent.Subscriptions(x => x
            .UseMongoDbInbox(cfg => cfg
                .SetCollection(collectionName)
                .SetConfiguration(_configuration!)));
        return this;
    }

    /// <summary>
    /// Enables the inbox pattern with advanced collection configuration via a builder delegate,
    /// while using the shared MongoDB configuration for connection and database settings.
    /// </summary>
    /// <param name="configure">An action that configures a <see cref="MongoDbCollectionConfigurationBuilder"/>.</param>
    /// <returns>The current <see cref="MongoDbConfigurator"/> instance for method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="configure"/> is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown if called before <see cref="SetConnection(Paramore.Brighter.MongoDb.IAmAMongoDbConfiguration)"/>.</exception>
    public MongoDbConfigurator UseInbox(Action<MongoDbCollectionConfigurationBuilder> configure)
    {
        if (configure == null)
        {
            throw new ArgumentNullException(nameof(configure));
        }

        _action += fluent => fluent.Subscriptions(x => x
            .UseMongoDbInbox(cfg => cfg
                .SetCollection(configure)
                .SetConfiguration(_configuration!)));
        return this;
    }

    /// <summary>
    /// Enables the outbox pattern using the shared MongoDB configuration.
    /// The outbox ensures reliable message publishing by storing messages in MongoDB before dispatch.
    /// </summary>
    /// <returns>The current <see cref="MongoDbConfigurator"/> instance for method chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown if called before <see cref="SetConnection(Paramore.Brighter.MongoDb.IAmAMongoDbConfiguration)"/>.</exception>
    public MongoDbConfigurator UseOutbox()
    {
        UseOutbox("outbox");
        return this;
    }

    /// <summary>
    /// Enables the outbox pattern with a custom collection name, using the shared MongoDB configuration.
    /// </summary>
    /// <param name="collectionName">The name of the MongoDB collection to use for the outbox.</param>
    /// <returns>The current <see cref="MongoDbConfigurator"/> instance for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown if <paramref name="collectionName"/> is null or empty.</exception>
    /// <exception cref="InvalidOperationException">Thrown if called before <see cref="SetConnection(Paramore.Brighter.MongoDb.IAmAMongoDbConfiguration)"/>.</exception>
    public MongoDbConfigurator UseOutbox(string collectionName)
    {
        if (string.IsNullOrEmpty(collectionName))
            throw new ArgumentException("Collection name cannot be null or empty.", nameof(collectionName));

        _action += fluent => fluent.Producers(x => x
            .UseMongoDbOutbox(cfg => cfg
                .SetCollection(collectionName)
                .SetConfiguration(_configuration!)));
        return this;
    }

    /// <summary>
    /// Enables the outbox pattern with advanced collection configuration via a builder delegate,
    /// while using the shared MongoDB configuration for connection and database settings.
    /// </summary>
    /// <param name="configure">An action that configures a <see cref="MongoDbCollectionConfigurationBuilder"/>.</param>
    /// <returns>The current <see cref="MongoDbConfigurator"/> instance for method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="configure"/> is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown if called before <see cref="SetConnection(Paramore.Brighter.MongoDb.IAmAMongoDbConfiguration)"/>.</exception>
    public MongoDbConfigurator UseOutbox(Action<MongoDbCollectionConfigurationBuilder> configure)
    {
        if (configure == null)
        {
            throw new ArgumentNullException(nameof(configure));
        }

        _action += fluent => fluent.Producers(x => x
            .UseMongoDbOutbox(cfg => cfg
                .SetCollection(configure)
                .SetConfiguration(_configuration!)));
        return this;
    }

    /// <summary>
    /// Enables distributed locking using the shared MongoDB configuration.
    /// This prevents duplicate message processing in scaled-out environments.
    /// </summary>
    /// <returns>The current <see cref="MongoDbConfigurator"/> instance for method chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown if called before <see cref="SetConnection(Paramore.Brighter.MongoDb.IAmAMongoDbConfiguration)"/>.</exception>
    public MongoDbConfigurator UseDistributedLock()
    {
        UseDistributedLock("distributedLock");
        return this;
    }

    /// <summary>
    /// Enables distributed locking with a custom collection name, using the shared MongoDB configuration.
    /// </summary>
    /// <param name="collectionName">The name of the MongoDB collection to use for distributed locks.</param>
    /// <returns>The current <see cref="MongoDbConfigurator"/> instance for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown if <paramref name="collectionName"/> is null or empty.</exception>
    /// <exception cref="InvalidOperationException">Thrown if called before <see cref="SetConnection(Paramore.Brighter.MongoDb.IAmAMongoDbConfiguration)"/>.</exception>
    public MongoDbConfigurator UseDistributedLock(string collectionName)
    {
        if (string.IsNullOrEmpty(collectionName))
            throw new ArgumentException("Collection name cannot be null or empty.", nameof(collectionName));

        _action += fluent => fluent.Producers(x => x
            .UseMongoDbDistributedLock(cfg => cfg
                .SetCollection(collectionName)
                .SetConfiguration(_configuration!)));
        return this;
    }

    /// <summary>
    /// Enables distributed locking with advanced collection configuration via a builder delegate,
    /// while using the shared MongoDB configuration for connection and database settings.
    /// </summary>
    /// <param name="configure">An action that configures a <see cref="MongoDbCollectionConfigurationBuilder"/>.</param>
    /// <returns>The current <see cref="MongoDbConfigurator"/> instance for method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="configure"/> is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown if called before <see cref="SetConnection(Paramore.Brighter.MongoDb.IAmAMongoDbConfiguration)"/>.</exception>
    public MongoDbConfigurator UseDistributedLock(Action<MongoDbCollectionConfigurationBuilder> configure)
    {
        if (configure == null)
        {
            throw new ArgumentNullException(nameof(configure));
        }

        _action += fluent => fluent.Producers(x => x
            .UseMongoDbDistributedLock(cfg => cfg
                .SetCollection(configure)
                .SetConfiguration(_configuration!)));
        return this;
    }
    
    /// <summary>
    /// Configures a MongoDB GridFS luggage store using a custom bucket name.
    /// The database and connection are inherited from the shared MongoDB configuration.
    /// </summary>
    /// <param name="bucketName">The name of the GridFS bucket to use for storing large message payloads.</param>
    /// <returns>The current <see cref="MongoDbConfigurator"/> instance for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown if <paramref name="bucketName"/> is null or empty.</exception>
    public MongoDbConfigurator UseLuggageStore(string bucketName)
    {
        return UseLuggageStore(cfg => cfg.SetBucketName(bucketName));
    }

    /// <summary>
    /// Configures a MongoDB GridFS luggage store using a fluent builder delegate.
    /// The database name and connection string are automatically inherited from the shared MongoDB configuration.
    /// </summary>
    /// <param name="configure">An action that configures a <see cref="MongoDbLuggageStoreBuilder"/>.</param>
    /// <returns>The current <see cref="MongoDbConfigurator"/> instance for method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="configure"/> is null.</exception>
    public MongoDbConfigurator UseLuggageStore(Action<MongoDbLuggageStoreBuilder> configure)
    {
        _action += fluent => fluent
            .SetLuggageStore(store => store
                .UseMongoGridFsLuggageStore(cfg =>
                {
                    if (!string.IsNullOrEmpty(_connectionString))
                    {
                        cfg.SetConnectionString(_connectionString!);
                    }
                    else
                    {
                        var settings = _configuration!.Client.Settings;
                        var builder = new MongoUrlBuilder
                        {
                            AllowInsecureTls = settings.AllowInsecureTls, 
                            ApplicationName = settings.ApplicationName,
                            AuthenticationMechanism = settings.Credential.Mechanism,
                            AuthenticationSource = settings.Credential.Source,
                            Compressors = settings.Compressors,
                            ConnectTimeout = settings.ConnectTimeout,
                            DirectConnection = settings.DirectConnection,
                            HeartbeatInterval = settings.HeartbeatInterval,
                            HeartbeatTimeout = settings.HeartbeatTimeout,
                            IPv6 = settings.IPv6,
                            LoadBalanced = settings.LoadBalanced,
                            LocalThreshold = settings.LocalThreshold,
                            MaxConnecting = settings.MaxConnecting,
                            MaxConnectionIdleTime = settings.MaxConnectionIdleTime,
                            MaxConnectionLifeTime = settings.MaxConnectionLifeTime,
                            MaxConnectionPoolSize = settings.MaxConnectionPoolSize,
                            MinConnectionPoolSize = settings.MinConnectionPoolSize,
                            Username = settings.Credential.Username,
                            Password = settings.Credential.Password,
                            ReadConcernLevel = settings.ReadConcern.Level,
                            ReadPreference = settings.ReadPreference,
                            ReplicaSetName = settings.ReplicaSetName,
                            RetryReads = settings.RetryReads,
                            RetryWrites = settings.RetryWrites,
                            Scheme = settings.Scheme,
                            Server = settings.Server,
                            Servers = settings.Servers,
                            ServerMonitoringMode = settings.ServerMonitoringMode,
                            ServerSelectionTimeout = settings.ServerSelectionTimeout,
                            SocketTimeout = settings.SocketTimeout,
                            SrvMaxHosts = settings.SrvMaxHosts,
                            SrvServiceName = settings.SrvServiceName,
                            ProxyHost = settings.Socks5ProxySettings.Host,
                            ProxyPort = settings.Socks5ProxySettings.Port,
                            // Isn't possible to get username & password from setting
                            // builder.ProxyUsername = settings.Socks5ProxySettings.Authentication.ToString()
                            // builder.ProxyPassword = settings.Socks5ProxySettings.Authentication.ToString()
                            TlsDisableCertificateRevocationCheck = settings.SslSettings.CheckCertificateRevocation,
                            UseTls = settings.UseTls,
                            // builder.ComputedWaitQueueSize = settings.WaitQueueTimeout;
                            WaitQueueTimeout = settings.WaitQueueTimeout
                        };
                        cfg.SetConnectionString(builder.ToString());
                    }
                    
                    cfg.SetDatabaseName(_configuration!.DatabaseName);
                    configure(cfg);
                }));
        return this;
    }

    /// <summary>
    /// Applies the configured MongoDB features (inbox, outbox, locking) to the provided <see cref="FluentBrighterBuilder"/>.
    /// </summary>
    /// <param name="fluentBrighter">The Brighter builder to configure.</param>
    /// <exception cref="ConfigurationException">
    /// Thrown if <see cref="SetConnection(Paramore.Brighter.MongoDb.IAmAMongoDbConfiguration)"/> was not called before this method.
    /// </exception>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="fluentBrighter"/> is null.</exception>
    internal void SetFluentBrighter(FluentBrighterBuilder fluentBrighter)
    {
        if (fluentBrighter == null)
            throw new ArgumentNullException(nameof(fluentBrighter));

        if (_configuration == null)
        {
            throw new ConfigurationException("No MongoDB configuration was set. Call SetConnection before applying configuration.");
        }

        _action(fluentBrighter);
    }
}