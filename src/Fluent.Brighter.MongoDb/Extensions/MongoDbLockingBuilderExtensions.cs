using System;

using Fluent.Brighter.MongoDb;

using Paramore.Brighter.MongoDb;

namespace Fluent.Brighter;

public static class MongoDbLockingBuilderExtensions
{
    public static MongoDbLockingBuilder SetConfiguration(this MongoDbLockingBuilder builder,
        Action<MongoDbConfigurationBuilder> configure)
    {
        var configuration = new  MongoDbConfigurationBuilder();
        configure(configuration);
        return builder.SetConfiguration(configuration.Build());
    }

    public static MongoDbLockingBuilder SetCollection(this MongoDbLockingBuilder builder, string collectionName)
        => builder.SetCollection(new MongoDbCollectionConfiguration { Name = collectionName });

    public static MongoDbLockingBuilder SetCollection(this MongoDbLockingBuilder builder,
        Action<MongoDbCollectionConfigurationBuilder> configure)
    {
        var collection =  new MongoDbCollectionConfigurationBuilder();
        configure(collection);
        return builder.SetCollection(collection.Build());
    }
}