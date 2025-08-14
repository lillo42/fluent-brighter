using System;

using MongoDB.Driver;

using Paramore.Brighter.MongoDb;

namespace Fluent.Brighter.MongoDb;

public sealed class MongoDbCollectionConfigurationBuilder
{
    private readonly MongoDbCollectionConfiguration _configuration = new();

    public MongoDbCollectionConfigurationBuilder SetName(string name)
    {
        _configuration.Name = name;
        return this;
    }

    public MongoDbCollectionConfigurationBuilder SetMakeCollection(OnResolvingACollection makeCollection)
    {
        _configuration.MakeCollection = makeCollection;
        return this;
    }

    public MongoDbCollectionConfigurationBuilder SetSettings(MongoCollectionSettings settings)
    {
        _configuration.Settings = settings;
        return this;
    }

    public MongoDbCollectionConfigurationBuilder SetCreateCollectionOptions(CreateCollectionOptions createOptions)
    {
        _configuration.CreateCollectionOptions = createOptions;
        return this;
    }

    public MongoDbCollectionConfigurationBuilder SetTimeToLive(TimeSpan? timeToLive)
    {
        _configuration.TimeToLive = timeToLive;
        return this;
    }

    internal MongoDbCollectionConfiguration Build()
    {
        return _configuration;
    }
}