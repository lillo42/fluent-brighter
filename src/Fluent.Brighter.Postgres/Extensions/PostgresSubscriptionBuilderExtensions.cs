using Fluent.Brighter.Postgres;

using Paramore.Brighter;

namespace Fluent.Brighter;

/// <summary>
/// Provides extension methods for <see cref="PostgresSubscriptionBuilder"/> to simplify configuration with convenient helper methods.
/// These extensions provide readable, intention-revealing methods for configuring message pump types, channel creation behavior,
/// message payload formats, and large message support.
/// </summary>
public static class PostgresSubscriptionBuilderExtensions
{
    #region Message Pump
    /// <summary>
    /// Configures the subscription to use the Proactor message pump pattern.
    /// The Proactor pattern uses async/await for non-blocking I/O operations, making it ideal for
    /// high-throughput scenarios and when handling many concurrent operations.
    /// </summary>
    /// <param name="builder">The <see cref="PostgresSubscriptionBuilder"/> instance to configure.</param>
    /// <returns>The <see cref="PostgresSubscriptionBuilder"/> instance for method chaining.</returns>
    public static PostgresSubscriptionBuilder UseProactor(this PostgresSubscriptionBuilder builder)
    {
        return builder.SetMessagePumpType(MessagePumpType.Proactor);
    }

    /// <summary>
    /// Configures the subscription to use the Reactor message pump pattern.
    /// The Reactor pattern uses synchronous processing, which can be simpler to reason about
    /// and may be preferred for CPU-bound operations or simpler workflows.
    /// </summary>
    /// <param name="builder">The <see cref="PostgresSubscriptionBuilder"/> instance to configure.</param>
    /// <returns>The <see cref="PostgresSubscriptionBuilder"/> instance for method chaining.</returns>
    public static PostgresSubscriptionBuilder UseReactor(this PostgresSubscriptionBuilder builder)
    {
        return builder.SetMessagePumpType(MessagePumpType.Reactor);
    }

    #endregion
    
    #region MakeChannels
    /// <summary>
    /// Configures the subscription to create PostgreSQL queues/topics if they don't exist.
    /// When a subscription attempts to consume from a non-existent queue, it will be automatically created.
    /// </summary>
    /// <param name="builder">The <see cref="PostgresSubscriptionBuilder"/> instance to configure.</param>
    /// <returns>The <see cref="PostgresSubscriptionBuilder"/> instance for method chaining.</returns>
    public static PostgresSubscriptionBuilder CreateQueueIfMissing(this PostgresSubscriptionBuilder builder)
    {
        return builder.SetMakeChannels(OnMissingChannel.Create);
    }

    /// <summary>
    /// Configures the subscription to validate that PostgreSQL queues/topics exist before consuming.
    /// If a queue doesn't exist, an error will be raised rather than creating it automatically.
    /// </summary>
    /// <param name="builder">The <see cref="PostgresSubscriptionBuilder"/> instance to configure.</param>
    /// <returns>The <see cref="PostgresSubscriptionBuilder"/> instance for method chaining.</returns>
    public static PostgresSubscriptionBuilder ValidIfQueueExists(this PostgresSubscriptionBuilder builder)
    {
        return builder.SetMakeChannels(OnMissingChannel.Validate);
    }

    /// <summary>
    /// Configures the subscription to assume PostgreSQL queues/topics exist without validation.
    /// No checks will be performed, and the subscription will attempt to consume from the queue regardless.
    /// This is the most performant option but requires queues to be pre-created.
    /// </summary>
    /// <param name="builder">The <see cref="PostgresSubscriptionBuilder"/> instance to configure.</param>
    /// <returns>The <see cref="PostgresSubscriptionBuilder"/> instance for method chaining.</returns>
    public static PostgresSubscriptionBuilder AssumeQueueExists(this PostgresSubscriptionBuilder builder)
    {
        return builder.SetMakeChannels(OnMissingChannel.Assume);
    }

    #endregion
    
    #region Binary Message Payload
    /// <summary>
    /// Enables binary message payload storage in PostgreSQL.
    /// Message payloads will be stored as binary data (bytea) instead of text, which can be more efficient
    /// for certain message types and reduces storage overhead for binary content.
    /// </summary>
    /// <param name="builder">The <see cref="PostgresSubscriptionBuilder"/> instance to configure.</param>
    /// <returns>The <see cref="PostgresSubscriptionBuilder"/> instance for method chaining.</returns>
    public static PostgresSubscriptionBuilder EnableBinaryMessagePayload(this PostgresSubscriptionBuilder builder)
    {
        return builder.SetBinaryMessagePayload(true);
    }

    /// <summary>
    /// Disables binary message payload storage in PostgreSQL.
    /// Message payloads will be stored as text format instead of binary data.
    /// This is useful when you need human-readable messages in the database for debugging or monitoring.
    /// </summary>
    /// <param name="builder">The <see cref="PostgresSubscriptionBuilder"/> instance to configure.</param>
    /// <returns>The <see cref="PostgresSubscriptionBuilder"/> instance for method chaining.</returns>
    public static PostgresSubscriptionBuilder DisableBinaryMessagePayload(this PostgresSubscriptionBuilder builder)
    {
        return builder.SetBinaryMessagePayload(false);
    }

    #endregion
    
    #region Table with large message
    /// <summary>
    /// Enables large message support for the PostgreSQL queue table.
    /// When enabled, the table schema is optimized for storing larger message payloads that exceed normal size limits.
    /// This may impact performance but allows handling of messages with substantial content.
    /// </summary>
    /// <param name="builder">The <see cref="PostgresSubscriptionBuilder"/> instance to configure.</param>
    /// <returns>The <see cref="PostgresSubscriptionBuilder"/> instance for method chaining.</returns>
    public static PostgresSubscriptionBuilder EnableTableWithLargeMessage(this PostgresSubscriptionBuilder builder)
    {
        return builder.SetTableWithLargeMessage(true);
    }

    /// <summary>
    /// Disables large message support for the PostgreSQL queue table.
    /// The table will use a standard schema optimized for normal-sized messages.
    /// This provides better performance but may not accommodate very large message payloads.
    /// </summary>
    /// <param name="builder">The <see cref="PostgresSubscriptionBuilder"/> instance to configure.</param>
    /// <returns>The <see cref="PostgresSubscriptionBuilder"/> instance for method chaining.</returns>
    public static PostgresSubscriptionBuilder DisableTableWithLargeMessage(this PostgresSubscriptionBuilder builder)
    {
        return builder.SetTableWithLargeMessage(false);
    }

    #endregion
}