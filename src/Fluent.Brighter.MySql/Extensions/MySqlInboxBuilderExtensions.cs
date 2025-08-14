using System;

using Fluent.Brighter.MySql;

namespace Fluent.Brighter;

public static class MySqlInboxBuilderExtensions
{
    public static MySqlInboxBuilder SetConfiguration(this MySqlInboxBuilder  builder,
        Action<RelationalDatabaseConfigurationBuilder> configure)
    {
        var configuration = new RelationalDatabaseConfigurationBuilder();
        configure(configuration);
        return builder.SetConfiguration(configuration.Build());
    }
}