using Paramore.Brighter;

namespace Fluent.Brighter;

/// <summary>
/// Builder for configuring relational database settings used by Brighter's outbox/inbox
/// </summary>
/// <remarks>
/// Provides a fluent interface to configure database connection settings, table names, 
/// and message storage options for Brighter's persistence components.
/// </remarks>
public sealed class RelationalDatabaseConfigurationBuilder
{
    private string? _connectionString;

    /// <summary>
    /// Sets the database connection string (required)
    /// </summary>
    /// <param name="connectionString">The connection string for the database</param>
    /// <returns>The builder instance for fluent chaining</returns>
    public RelationalDatabaseConfigurationBuilder SetConnectionString(string connectionString)
    {
        _connectionString = connectionString;
        return this;
    }

    private string? _databaseName;
    
    /// <summary>
    /// Sets the database name (optional)
    /// </summary>
    /// <param name="databaseName">Name of the database</param>
    /// <returns>The builder instance for fluent chaining</returns>
    public RelationalDatabaseConfigurationBuilder SetDatabaseName(string? databaseName)
    {
        _databaseName = databaseName;
        return this;
    }

    
    private string? _outBoxTableName;
    
    /// <summary>
    /// Sets the outbox table name (optional)
    /// </summary>
    /// <param name="outBoxTableName">Name of the outbox table</param>
    /// <returns>The builder instance for fluent chaining</returns>
    public RelationalDatabaseConfigurationBuilder SetOutboxTableName(string? outBoxTableName)
    {
        _outBoxTableName = outBoxTableName;
        return this;
    }

   
    private string? _inboxTableName;
    
    /// <summary>
    /// Sets the inbox table name (optional)
    /// </summary>
    /// <param name="inboxTableName">Name of the inbox table</param>
    /// <returns>The builder instance for fluent chaining</returns>
    public RelationalDatabaseConfigurationBuilder SetInboxTableName(string? inboxTableName)
    {
        _inboxTableName = inboxTableName;
        return this;
    }

    private string? _queueStoreTable;
    
    /// <summary>
    /// Sets the queue store table name (optional)
    /// </summary>
    /// <param name="queueStoreTable">Name of the queue store table</param>
    /// <returns>The builder instance for fluent chaining</returns>
    public RelationalDatabaseConfigurationBuilder SetQueueStoreTable(string? queueStoreTable)
    {
        _queueStoreTable = queueStoreTable;
        return this;
    }

    private string? _schemaName;
    
    /// <summary>
    /// Sets the database schema name (optional)
    /// </summary>
    /// <param name="schemaName">Name of the database schema</param>
    /// <returns>The builder instance for fluent chaining</returns>
    public RelationalDatabaseConfigurationBuilder SetSchemaName(string? schemaName)
    {
        _schemaName = schemaName;
        return this;
    }

    private bool _binaryMessagePayload;
    
    /// <summary>
    /// Configures whether message payloads should be stored as binary (optional)
    /// </summary>
    /// <param name="binaryMessagePayload">
    /// True to store as binary, false for text (default: false)
    /// </param>
    /// <returns>The builder instance for fluent chaining</returns>
    public RelationalDatabaseConfigurationBuilder SetBinaryMessagePayload(bool binaryMessagePayload)
    {
        _binaryMessagePayload = binaryMessagePayload;
        return this;
    }

    public RelationalDatabaseConfiguration Build()
    {
        if (string.IsNullOrEmpty(_connectionString))
        {
            throw new ConfigurationException(
                "Database connection string is required. " +
                "Please provide a valid connection string using SetConnectionString() before building the configuration.");
        }

        return new RelationalDatabaseConfiguration(
            connectionString: _connectionString!, 
            databaseName: _databaseName,
            outBoxTableName: _outBoxTableName,
            inboxTableName: _inboxTableName,
            queueStoreTable: _queueStoreTable,
            schemaName: _schemaName,
            binaryMessagePayload: _binaryMessagePayload
        );
    } 
}