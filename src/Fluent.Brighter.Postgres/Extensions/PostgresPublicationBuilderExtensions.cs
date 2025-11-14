using Fluent.Brighter.Postgres;

using Paramore.Brighter;

namespace Fluent.Brighter;

/// <summary>
/// Provides extension methods for <see cref="PostgresPublicationBuilder"/> to simplify configuration with convenient helper methods.
/// These extensions provide readable, intention-revealing methods for configuring channel creation behavior and message payload formats.
/// </summary>
public static class PostgresPublicationBuilderExtensions
{
    #region MakeChannels
    /// <summary>
    /// Configures the publication to create PostgreSQL queues/topics if they don't exist.
    /// When a publication attempts to send to a non-existent queue, it will be automatically created.
    /// </summary>
    /// <param name="builder">The <see cref="PostgresPublicationBuilder"/> instance to configure.</param>
    /// <returns>The <see cref="PostgresPublicationBuilder"/> instance for method chaining.</returns>
    public static PostgresPublicationBuilder CreateQueueIfMissing(this PostgresPublicationBuilder builder)
    {
        return builder.SetMakeChannels(OnMissingChannel.Create);
    }

    /// <summary>
    /// Configures the publication to validate that PostgreSQL queues/topics exist before publishing.
    /// If a queue doesn't exist, an error will be raised rather than creating it automatically.
    /// </summary>
    /// <param name="builder">The <see cref="PostgresPublicationBuilder"/> instance to configure.</param>
    /// <returns>The <see cref="PostgresPublicationBuilder"/> instance for method chaining.</returns>
    public static PostgresPublicationBuilder ValidIfQueueExists(this PostgresPublicationBuilder builder)
    {
        return builder.SetMakeChannels(OnMissingChannel.Validate);
    }

    /// <summary>
    /// Configures the publication to assume PostgreSQL queues/topics exist without validation.
    /// No checks will be performed, and the publication will attempt to use the queue regardless.
    /// This is the most performant option but requires queues to be pre-created.
    /// </summary>
    /// <param name="builder">The <see cref="PostgresPublicationBuilder"/> instance to configure.</param>
    /// <returns>The <see cref="PostgresPublicationBuilder"/> instance for method chaining.</returns>
    public static PostgresPublicationBuilder AssumeQueueExists(this PostgresPublicationBuilder builder)
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
    /// <param name="builder">The <see cref="PostgresPublicationBuilder"/> instance to configure.</param>
    /// <returns>The <see cref="PostgresPublicationBuilder"/> instance for method chaining.</returns>
    public static PostgresPublicationBuilder EnableBinaryMessagePayload(this PostgresPublicationBuilder builder)
    {
        return builder.SetBinaryMessagePayload(true);
    }

    /// <summary>
    /// Disables binary message payload storage in PostgreSQL.
    /// Message payloads will be stored as text format instead of binary data.
    /// This is useful when you need human-readable messages in the database for debugging or monitoring.
    /// </summary>
    /// <param name="builder">The <see cref="PostgresPublicationBuilder"/> instance to configure.</param>
    /// <returns>The <see cref="PostgresPublicationBuilder"/> instance for method chaining.</returns>
    public static PostgresPublicationBuilder DisableBinaryMessagePayload(this PostgresPublicationBuilder builder)
    {
        return builder.SetBinaryMessagePayload(false);
    }

    #endregion
}