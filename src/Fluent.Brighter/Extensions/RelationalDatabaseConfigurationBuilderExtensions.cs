namespace Fluent.Brighter;

/// <summary>
/// Provides extension methods for fluently configuring binary message payload options in relational database configurations
/// </summary>
public static class RelationalDatabaseConfigurationBuilderExtensions
{
    /// <summary>
    /// Enables a binary storage format for message payloads in the database
    /// </summary>
    /// <remarks>
    /// Configures the system to store message payloads in binary format (e.g., BLOB or varbinary columns).
    /// This is typically more space-efficient than text storage for certain payload types.
    /// </remarks>
    /// <param name="builder">The relational database configuration builder</param>
    /// <returns>The configuration builder for fluent chaining</returns>
    public static RelationalDatabaseConfigurationBuilder EnableBinaryMessagePayload(
        this RelationalDatabaseConfigurationBuilder builder)
    {
        return builder.SetBinaryMessagePayload(true);
    }

    /// <summary>
    /// Disables a binary storage format for message payloads in the database
    /// </summary>
    /// <remarks>
    /// Configures the system to store message payloads in text format (e.g., CLOB, nvarchar or text columns).
    /// This is typically more human-readable and may be preferable for debugging purposes.
    /// </remarks>
    /// <param name="builder">The relational database configuration builder</param>
    /// <returns>The configuration builder for fluent chaining</returns>
    public static RelationalDatabaseConfigurationBuilder DisableBinaryMessagePayload(
        this RelationalDatabaseConfigurationBuilder builder)
    {
        return builder.SetBinaryMessagePayload(false);
    }
}