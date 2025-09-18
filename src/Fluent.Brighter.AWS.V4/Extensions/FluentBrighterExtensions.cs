using System;

using Amazon.DynamoDBv2.Model;

using Paramore.Brighter;

namespace Fluent.Brighter.AWS.V4.Extensions;

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

    #region outbox 
    public static FluentBrighterBuilder UseDynamoDbTransactionOutboxArchive(this FluentBrighterBuilder builder)
        => builder.UseOutboxArchiver<TransactWriteItemsRequest>(new NullOutboxArchiveProvider());
    
    public static FluentBrighterBuilder UseDynamoDbTransactionOutboxArchive(this FluentBrighterBuilder builder, Action<TimedOutboxArchiverOptionsBuilder> configure)
    {
        var options = new TimedOutboxArchiverOptionsBuilder();
        configure(options);
        return builder.UseOutboxArchiver<TransactWriteItemsRequest>(new NullOutboxArchiveProvider(), options.Build());
    }

    #endregion
}