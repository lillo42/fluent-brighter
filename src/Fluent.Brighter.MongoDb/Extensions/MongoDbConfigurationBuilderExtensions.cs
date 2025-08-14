using System;

using Fluent.Brighter.MongoDb;

using Paramore.Brighter.MongoDb;

namespace Fluent.Brighter;

public static class MongoDbConfigurationBuilderExtensions
{
    public static MongoDbConfigurationBuilder SetOutbox(this MongoDbConfigurationBuilder builder, string collectionName)
        => builder.SetOutbox(new MongoDbCollectionConfiguration { Name = collectionName });
    
    public static MongoDbConfigurationBuilder SetOutbox(this MongoDbConfigurationBuilder builder,
        Action<MongoDbCollectionConfigurationBuilder> configure)
    {
        var collection = new MongoDbCollectionConfigurationBuilder();
        configure(collection);
        return builder.SetOutbox(collection.Build());
    }
    
    public static MongoDbConfigurationBuilder SetInbox(this MongoDbConfigurationBuilder builder, string collectionName)
        => builder.SetInbox(new MongoDbCollectionConfiguration { Name = collectionName });
    
    public static MongoDbConfigurationBuilder SetInbox(this MongoDbConfigurationBuilder builder,
        Action<MongoDbCollectionConfigurationBuilder> configure)
    {
        var collection = new MongoDbCollectionConfigurationBuilder();
        configure(collection);
        return builder.SetInbox(collection.Build());
    }
    
    public static MongoDbConfigurationBuilder SetLocking(this MongoDbConfigurationBuilder builder, string collectionName)
        => builder.SetLocking(new MongoDbCollectionConfiguration { Name = collectionName });
    
    public static MongoDbConfigurationBuilder SetLocking(this MongoDbConfigurationBuilder builder,
        Action<MongoDbCollectionConfigurationBuilder> configure)
    {
        var collection = new MongoDbCollectionConfigurationBuilder();
        configure(collection);
        return builder.SetLocking(collection.Build());
    }
}