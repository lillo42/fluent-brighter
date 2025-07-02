using System;

using MongoDB.Driver;

using Paramore.Brighter.MongoDb;

namespace Fluent.Brighter.MongoDb;

/// <summary>
/// A builder class for creating instances of <see cref="MongoDbConfiguration"/> with a fluent API.
/// </summary>
public class MongoDbConfigurationBuilder
{
    private MongoClient? _client;
    
    /// <summary>
    /// Sets the <see cref="MongoClient"/> for the configuration.
    /// </summary>
    /// <param name="client">The mongo client.</param>
    /// <returns>The current builder instance.</returns>
    public MongoDbConfigurationBuilder Client(MongoClient client)
    {
        _client = client;
        return this;
    }
    
    /// <summary>
    /// Sets the connection string for the configuration.
    /// </summary>
    /// <param name="connectionString">The MongoDB connection string.</param>
    /// <returns>The current builder instance.</returns>
    public MongoDbConfigurationBuilder ConnectionString(string connectionString)
    {
        _client = new MongoClient(connectionString);
        return this;
    }

    private string? _databaseName;
    
    /// <summary>
    /// Sets the database name for the configuration.
    /// </summary>
    /// <param name="databaseName">The name of the database.</param>
    /// <returns>The current builder instance.</returns>
    public MongoDbConfigurationBuilder DatabaseName(string databaseName)
    {
        _databaseName = databaseName;
        return this;
    }
    
    private string? _collectionName;
    
    /// <summary>
    /// Sets the collection name for the configuration.
    /// </summary>
    /// <param name="collectionName">The name of the collection.</param>
    /// <returns>The current builder instance.</returns>
    public MongoDbConfigurationBuilder CollectionName(string collectionName)
    {
        _collectionName = collectionName;
        return this;
    }
    
    private TimeProvider _timeProvider = System.TimeProvider.System;
        
    /// <summary>
    /// Sets the <see cref="System.TimeProvider"/> for the configuration.
    /// </summary>
    /// <param name="timeProvider">The time provider instance.</param>
    /// <returns>The current builder instance.</returns>
    public MongoDbConfigurationBuilder TimeProvider(TimeProvider timeProvider)
    {
        _timeProvider = timeProvider;
        return this;
    }

    private MongoDatabaseSettings? _databaseSettings;
    
    /// <summary>
    /// Sets the <see cref="MongoDatabaseSettings"/> used when accessing the database.
    /// </summary>
    /// <param name="databaseSettings">The database settings.</param>
    /// <returns>The current builder instance.</returns>
    public MongoDbConfigurationBuilder DatabaseSettings(MongoDatabaseSettings? databaseSettings)
    {
        _databaseSettings = databaseSettings;
        return this;
    }

    private MongoCollectionSettings? _collectionSettings;
    
    /// <summary>
    /// Sets the <see cref="MongoCollectionSettings"/> used to get the collection.
    /// </summary>
    /// <param name="collectionSettings">The collection settings.</param>
    /// <returns>The current builder instance.</returns>
    public MongoDbConfigurationBuilder CollectionSettings(MongoCollectionSettings? collectionSettings)
    {
        _collectionSettings = collectionSettings;
        return this;
    }

    private CreateCollectionOptions? _createCollectionOptions;

    /// <summary>
    /// Sets the <see cref="CreateCollectionOptions"/> for creating the collection.
    /// </summary>
    /// <param name="createCollectionOptions">The create collection options.</param>
    /// <returns>The current builder instance.</returns>
    public MongoDbConfigurationBuilder CreateCollectionOptions(CreateCollectionOptions? createCollectionOptions)
    {
        _createCollectionOptions = createCollectionOptions;
        return this;
    }

    private TimeSpan? _timeToLive;
    
    /// <summary>
    /// Sets the optional time to live for messages in the outbox.
    /// </summary>
    /// <param name="timeToLive">The time to live duration. Null means messages will not expire.</param>
    /// <returns>The current builder instance.</returns>
    public MongoDbConfigurationBuilder TimeToLive(TimeSpan? timeToLive)
    {
        _timeToLive = timeToLive;
        return this;
    } 
    
    private OnResolvingACollection _makeCollection = OnResolvingACollection.CreateIfNotExists;
    
    /// <summary>
    /// Sets the action to be performed when resolving a collection.
    /// </summary>
    /// <param name="makeCollection">The collection resolution action.</param>
    /// <returns>The current builder instance.</returns>
    public MongoDbConfigurationBuilder MakeCollection(OnResolvingACollection makeCollection)
    {
        _makeCollection = makeCollection;
        return this;
    }
 
    /// <summary>
    /// Set the make collection as <see cref="OnResolvingACollection.CreateIfNotExists"/>
    /// </summary>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public MongoDbConfigurationBuilder CreateCollectionIfMissing() => MakeCollection(OnResolvingACollection.CreateIfNotExists);
    
    /// <summary>
    /// Set the make collection as <see cref="OnResolvingACollection.Validate"/>
    /// </summary>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public MongoDbConfigurationBuilder ValidateIfCollectionExists() => MakeCollection(OnResolvingACollection.Validate);
    
    /// <summary>
    /// Set the make collection as <see cref="OnResolvingACollection.Assume"/>
    /// </summary>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public MongoDbConfigurationBuilder AssumeCollectionExists() => MakeCollection(OnResolvingACollection.Assume);
    
    /// <summary>
    /// Builds and returns a new instance of <see cref="MongoDbConfiguration"/> with the configured properties.
    /// </summary>
    /// <returns>A new <see cref="MongoDbConfiguration"/> instance.</returns>
    /// <exception cref="InvalidOperationException">Thrown if mandatory parameters (Client, DatabaseName, CollectionName) are not set.</exception>
    public MongoDbConfiguration Build()
    {
        if (_client == null || _databaseName == null || _collectionName == null)
        {
            throw new InvalidOperationException("Client, DatabaseName, and CollectionName must be provided before building MongoDbConfiguration.");
        }

        var config = new MongoDbConfiguration(_client, _databaseName, _collectionName)
        {
            TimeProvider = _timeProvider,
            MakeCollection = _makeCollection,
            DatabaseSettings = _databaseSettings,
            CollectionSettings = _collectionSettings,
            CreateCollectionOptions = _createCollectionOptions,
            TimeToLive = _timeToLive
        };

        return config;
    }
}