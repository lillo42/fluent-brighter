using System;

using Amazon.DynamoDBv2.Model;

using Fluent.Brighter.AWS;

using Paramore.Brighter;

namespace Fluent.Brighter;

/// <summary>
/// Extension methods for FluentBrighterBuilder to provide AWS-specific configurations
/// and DynamoDB outbox archiving functionality.
/// </summary>
public static class FluentBrighterExtensions
{
    /// <summary>
    /// Configures AWS services (SNS, SQS, DynamoDB, S3) for use with Paramore.Brighter
    /// using a fluent configuration pattern.
    /// </summary>
    /// <param name="builder">The FluentBrighterBuilder instance</param>
    /// <param name="configure">Action to configure AWS services</param>
    /// <returns>The FluentBrighterBuilder instance for method chaining</returns>
    public static FluentBrighterBuilder UsingAws(this FluentBrighterBuilder builder,
        Action<AWSConfigurator> configure)
    {
        var configurator = new AWSConfigurator();
        configure(configurator);
        configurator.SetFluentBrighter(builder);
        return builder;
    }

    #region Outbox 

    /// <summary>
    /// Configures DynamoDB as the transaction outbox archive store using default settings.
    /// Note: Currently uses a null archive provider implementation.
    /// </summary>
    /// <param name="builder">The FluentBrighterBuilder instance</param>
    /// <returns>The FluentBrighterBuilder instance for method chaining</returns>
    public static FluentBrighterBuilder UseDynamoDbTransactionOutboxArchive(this FluentBrighterBuilder builder)
    {
        return builder.UseOutboxArchiver<TransactWriteItemsRequest>(new NullOutboxArchiveProvider());
    }

    /// <summary>
    /// Configures DynamoDB as the transaction outbox archive store with custom settings.
    /// Note: Currently uses a null archive provider implementation.
    /// </summary>
    /// <param name="builder">The FluentBrighterBuilder instance</param>
    /// <param name="configure">Action to configure outbox archiver options</param>
    /// <returns>The FluentBrighterBuilder instance for method chaining</returns>
    public static FluentBrighterBuilder UseDynamoDbTransactionOutboxArchive(this FluentBrighterBuilder builder, Action<TimedOutboxArchiverOptionsBuilder> configure)
    {
        var options = new TimedOutboxArchiverOptionsBuilder();
        configure(options);
        return builder.UseOutboxArchiver<TransactWriteItemsRequest>(new NullOutboxArchiveProvider(), options.Build());
    }

    #endregion
}