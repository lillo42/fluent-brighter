using System;

using Fluent.Brighter.MongoDb;

using Paramore.Brighter.MongoDb;

namespace Fluent.Brighter;

public static class MongoDbOutboxBuilderExtensions
{
    public static MongoDbOutboxBuilder SetConfiguration(this MongoDbOutboxBuilder builder,
        Action<MongoDbConfigurationBuilder> configure)
    {
        var configuration = new  MongoDbConfigurationBuilder();
        configure(configuration);
        return builder.SetConfiguration(configuration.Build());
    }

    public static MongoDbOutboxBuilder SetCollection(this MongoDbOutboxBuilder builder, string collectionName)
        => builder.SetCollection(new MongoDbCollectionConfiguration { Name = collectionName });

    public static MongoDbOutboxBuilder SetCollection(this MongoDbOutboxBuilder builder,
        Action<MongoDbCollectionConfigurationBuilder> configure)
    {
        var collection =  new MongoDbCollectionConfigurationBuilder();
        configure(collection);
        return builder.SetCollection(collection.Build());
    }
}