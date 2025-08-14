using System;

using Fluent.Brighter.MongoDb;

namespace Fluent.Brighter;

public static class FluentBrighterExtensions
{
    public static FluentBrighterBuilder UsingMongoDb(this FluentBrighterBuilder builder,
        Action<MongoDbConfigurator> configure)
    {
        var configurator = new MongoDbConfigurator();
        configure(configurator);
        configurator.SetFluentBrighter(builder);
        return builder;
    }
}