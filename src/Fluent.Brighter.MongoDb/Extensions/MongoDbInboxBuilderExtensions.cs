using System;

using Fluent.Brighter.MongoDb;

using Paramore.Brighter.MongoDb;

namespace Fluent.Brighter;

public static class MongoDbInboxBuilderExtensions
{
    public static MongoDbInboxBuilder SetConfiguration(this MongoDbInboxBuilder builder,
        Action<MongoDbConfigurationBuilder> configure)
    {
        var configuration = new  MongoDbConfigurationBuilder();
        configure(configuration);
        return builder.SetConfiguration(configuration.Build());
    }

    public static MongoDbInboxBuilder SetCollection(this MongoDbInboxBuilder builder, string collectionName)
        => builder.SetCollection(new MongoDbCollectionConfiguration { Name = collectionName });

    public static MongoDbInboxBuilder SetCollection(this MongoDbInboxBuilder builder,
        Action<MongoDbCollectionConfigurationBuilder> configure)
    {
        var collection =  new MongoDbCollectionConfigurationBuilder();
        configure(collection);
        return builder.SetCollection(collection.Build());
    }
}