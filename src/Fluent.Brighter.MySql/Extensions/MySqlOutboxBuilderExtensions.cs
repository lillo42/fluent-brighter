using System;

using Fluent.Brighter.MySql;

namespace Fluent.Brighter;

public static class MySqlOutboxBuilderExtensions
{
    public static MySqlOutboxBuilder SetConfiguration(this MySqlOutboxBuilder builder,
        Action<RelationalDatabaseConfigurationBuilder> configure)
    {
        var configuration = new RelationalDatabaseConfigurationBuilder();
        configure(configuration);
        return builder.SetConfiguration(configuration.Build());
    }
}