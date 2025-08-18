using System;
using System.Data.Common;

using Paramore.Brighter;

namespace Fluent.Brighter;


/// <summary>
/// Provides extension methods for configuring Brighter's outbox features using a fluent API
/// </summary>
public static class FluentBrighterExtensions
{
    /// <summary>
    /// Enables the outbox sweeper with default configuration
    /// </summary>
    /// <param name="builder">The FluentBrighterBuilder instance</param>
    /// <returns>The FluentBrighterBuilder instance for method chaining</returns>
    /// <remarks>
    /// Uses default sweeper configuration:
    /// - TimerInterval: 5 seconds
    /// - MinimumMessageAge: 5 seconds
    /// - BatchSize: 100 messages
    /// - UseBulk: false
    /// </remarks>
    public static FluentBrighterBuilder UseOutboxSweeper(this FluentBrighterBuilder builder) 
        => builder.UseOutboxSweeper(_ => { });

    /// <summary>
    /// Configures and enables the outbox sweeper using a fluent builder
    /// </summary>
    /// <param name="builder">The FluentBrighterBuilder instance</param>
    /// <param name="configure">Configuration action for the outbox sweeper options</param>
    /// <returns>The FluentBrighterBuilder instance for method chaining</returns>
    /// <remarks>
    /// <para>
    /// This extension provides a fluent interface for configuring the TimedOutboxSweeper. It wraps
    /// the configuration process using the TimedOutboxSweeperOptionsBuilder for a more readable syntax.
    /// </para>
    /// <para>
    /// Example usage:
    /// <code>
    /// services.AddFluentBrighter(brighter => brighter
    ///     .UseOutboxSweeper(sweeper => 
    ///         sweeper.SetTimerInterval(30)
    ///             .SetMinimumMessageAge(TimeSpan.FromMinutes(1))
    ///             .SetBatchSize(500)
    ///             .SetBulk(true)
    ///             .SetArg("TableName", "OutboxMessages"))
    ///     );
    /// </code>
    /// </para>
    /// <para>
    /// The configuration action will:
    /// 1. Create a new TimedOutboxSweeperOptionsBuilder
    /// 2. Apply all configured options through the action
    /// 3. Build the final configuration delegate
    /// 4. Enable the sweeper in Brighter
    /// </para>
    /// </remarks>
    public static FluentBrighterBuilder UseOutboxSweeper(
        this FluentBrighterBuilder builder,
        Action<TimedOutboxSweeperOptionsBuilder> configure)
    {
        var options = new TimedOutboxSweeperOptionsBuilder();
        configure(options);
        return builder.UseOutboxSweeper(options.Build());
    }

    /// <summary>
    /// Enables outbox archiving with a null provider (no-op) and default configuration
    /// </summary>
    /// <typeparam name="TTransaction">The transaction type used by the outbox</typeparam>
    /// <param name="builder">The FluentBrighterBuilder instance</param>
    /// <returns>The FluentBrighterBuilder instance for method chaining</returns>
    /// <remarks>
    /// This method registers archiving with a null provider, effectively disabling actual archiving.
    /// Useful for development environments or when archiving isn't required.
    /// 
    /// Uses default archiver configuration:
    /// - TimerInterval: 15 seconds
    /// - MinimumAge: 24 hours
    /// - ArchiveBatchSize: 100 messages
    /// </remarks>
    public static FluentBrighterBuilder UseOutboxArchive<TTransaction>(this FluentBrighterBuilder builder) 
        => builder.UseOutboxArchiver<TTransaction>(new NullOutboxArchiveProvider());
    
    /// <summary>
    /// Enables outbox archiving with a null provider (no-op) and custom configuration
    /// </summary>
    /// <typeparam name="TTransaction">The transaction type used by the outbox</typeparam>
    /// <param name="builder">The FluentBrighterBuilder instance</param>
    /// <param name="configure">Configuration action for the outbox archiver options</param>
    /// <returns>The FluentBrighterBuilder instance for method chaining</returns>
    /// <remarks>
    /// Configures archiving behavior while using a null provider that doesn't actually archive messages.
    /// Useful for testing configuration or when you need to satisfy architectural requirements without persistence.
    /// 
    /// Example:
    /// <code>
    /// builder.UseOutboxArchive&lt;SqlTransaction&gt;(archiver => 
    ///     archiver.SetTimerInterval(3600)
    ///         .SetMinimumAgeInHours(48)
    ///         .SetArchiveBatchSize(500));
    /// </code>
    /// </remarks>
    public static FluentBrighterBuilder UseOutboxArchive<TTransaction>(
        this FluentBrighterBuilder builder,
        Action<TimedOutboxArchiverOptionsBuilder> configure)
    {
        var options = new TimedOutboxArchiverOptionsBuilder();
        configure(options);
        return builder.UseOutboxArchiver<TTransaction>(new NullOutboxArchiveProvider(), options.Build());
    }

    /// <summary>
    /// Enables outbox archiving for DbTransaction with a null provider (no-op) and default configuration
    /// </summary>
    /// <param name="builder">The FluentBrighterBuilder instance</param>
    /// <returns>The FluentBrighterBuilder instance for method chaining</returns>
    /// <remarks>
    /// Convenience method for systems using System.Data.Common.DbTransaction.
    /// Registers archiving with a null provider and default configuration.
    /// </remarks>
    public static FluentBrighterBuilder UseDbTransactionOutboxArchive(this FluentBrighterBuilder builder) 
        => builder.UseOutboxArchiver<DbTransaction>(new NullOutboxArchiveProvider());
    
    /// <summary>
    /// Enables outbox archiving for DbTransaction with a null provider (no-op) and custom configuration
    /// </summary>
    /// <param name="builder">The FluentBrighterBuilder instance</param>
    /// <param name="configure">Configuration action for the outbox archiver options</param>
    /// <returns>The FluentBrighterBuilder instance for method chaining</returns>
    /// <remarks>
    /// Convenience method for DbTransaction-based systems that need to configure archiving behavior
    /// without actually persisting archived messages.
    /// 
    /// Example:
    /// <code>
    /// builder.UseDbTransactionOutboxArchive(archiver => 
    ///     archiver.SetTimerInterval(86400)
    ///         .SetMinimumAgeInHours(168));
    /// </code>
    /// </remarks>
    public static FluentBrighterBuilder UseDbTransactionOutboxArchive(
        this FluentBrighterBuilder builder,
        Action<TimedOutboxArchiverOptionsBuilder> configure)
    {
        var options = new TimedOutboxArchiverOptionsBuilder();
        configure(options);
        return builder.UseOutboxArchiver<DbTransaction>(new NullOutboxArchiveProvider(), options.Build());
    }
}