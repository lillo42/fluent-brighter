using System;

using Fluent.Brighter.MongoDb;

using Paramore.Brighter.MongoDb;

namespace Fluent.Brighter;

public static class ConsumerBuilderExtensions
{
    public static ConsumerBuilder UseMongoDbInbox(this ConsumerBuilder builder,
        Action<MongoDbConfigurationBuilder> configure)
    {
        var configuration = new MongoDbConfigurationBuilder();
        configure(configuration);
        return builder.UseMongoDbInbox(configuration.Build());
    }
    public static ConsumerBuilder UseMongoDbInbox(this ConsumerBuilder builder, IAmAMongoDbConfiguration configuration)
        => builder.UseMongoDbInbox(cfg => cfg.SetConfiguration(configuration));

    public static ConsumerBuilder UseMongoDbInbox(this ConsumerBuilder builder, Action<MongoDbInboxBuilder> configure)
    {
        var inbox = new MongoDbInboxBuilder();
        configure(inbox);
        return builder.SetInbox(cfg => cfg.SetInbox(inbox.Build()));
    }
}