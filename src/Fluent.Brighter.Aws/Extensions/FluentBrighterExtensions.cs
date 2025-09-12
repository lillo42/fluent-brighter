using System;

using Fluent.Brighter.Aws;

namespace Fluent.Brighter;

public static class FluentBrighterExtensions
{
    public static FluentBrighterBuilder UsingAws(this FluentBrighterBuilder builder,
        Action<AwsConfigurator> configure)
    {
        var configurator = new AwsConfigurator();
        configure(configurator);
        configurator.SetFluentBrighter(builder);
        return builder;
    }
}