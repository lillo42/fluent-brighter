using Paramore.Brighter;

namespace Fluent.Brighter;

/// <summary>
/// A fluent builder for creating instances of <see cref="RelationalDatabaseConfiguration"/>.
/// </summary>
public class RelationalDatabaseConfigurationBuilder 
{
    private string? _connectionString;

    /// <summary>
    /// Sets the connection string
    /// </summary>
    /// <param name="connectionString">The database connection strings</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public RelationalDatabaseConfigurationBuilder ConnectionString(string connectionString)
    {
        _connectionString = connectionString;
        return this;
    }
    private string? _databaseName = "Brighter";
    
    /// <summary>
    /// Sets the name of the database containing the tables.
    /// If not provided, the default value "Brighter" will be used.
    /// </summary>
    /// <param name="databaseName">The name of the database.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public RelationalDatabaseConfigurationBuilder DatabaseName(string? databaseName)
    {
        _databaseName = databaseName;
        return this;
    }
    
    private string? _outBoxTableName = "Outbox";

    /// <summary>
    /// Sets the name of the outbox table.
    /// If not provided, the default value "Outbox" will be used.
    /// </summary>
    /// <param name="outBoxTableName">The name of the outbox table.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public RelationalDatabaseConfigurationBuilder OutBoxTableName(string? outBoxTableName)
    {
        _outBoxTableName = outBoxTableName;
        return this;
    }
    
    private string? _inBoxTableName = "Inbox";
   
    /// <summary>
    /// Sets the name of the inbox table.
    /// If not provided, the default value "Inbox" will be used.
    /// </summary>
    /// <param name="inboxTableName">The name of the inbox table.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public RelationalDatabaseConfigurationBuilder InBoxTableName(string? inboxTableName)
    {
        _inBoxTableName = inboxTableName;
        return this;
    }

    private string? _queueStoreTable = "Queue";

    /// <summary>
    /// Sets the name of the queue store table.
    /// If not provided, the default value "Queue" will be used.
    /// </summary>
    /// <param name="queueStoreTable">The name of the queue store table.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public RelationalDatabaseConfigurationBuilder QueueStoreTable(string? queueStoreTable)
    {
        _queueStoreTable = queueStoreTable;
        return this;
    }
    
    private string? _schemaName;

    /// <summary>
    /// Sets the schema name for the database objects.
    /// If not provided, no schema name will be used.
    /// </summary>
    /// <param name="schemaName">The schema name.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public RelationalDatabaseConfigurationBuilder SchemaName(string? schemaName)
    {
        _schemaName = schemaName;
        return this;
    }
    
    private bool _binaryMessagePayload;

    /// <summary>
    /// Sets whether the message payload should be stored as binary or UTF-8 string (in some case it will
    /// JSON or JSONB).
    /// The default is false 
    /// </summary>
    /// <param name="binaryMessagePayload">True to store as binary, false to store as UTF-8 string.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public RelationalDatabaseConfigurationBuilder BinaryMessagePayload(bool binaryMessagePayload)
    {
        _binaryMessagePayload = binaryMessagePayload;
        return this;
    }

    /// <summary>
    /// Enable binary message payload
    /// </summary>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public RelationalDatabaseConfigurationBuilder EnableBinaryMessagePayload()
        => BinaryMessagePayload(true);
    
    /// <summary>
    /// Disable binary message payload
    /// </summary>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public RelationalDatabaseConfigurationBuilder DisableBinaryMessagePayload()
        => BinaryMessagePayload(false);

    /// <summary>
    /// Builds and returns the configured <see cref="RelationalDatabaseConfiguration"/> instance.
    /// </summary>
    /// <returns>A new instance of <see cref="RelationalDatabaseConfiguration"/>.</returns>
    public RelationalDatabaseConfiguration Build()
    {
        if (string.IsNullOrEmpty(_connectionString))
        {
            throw new ConfigurationException("Connection string not set");
        }
        
        return new RelationalDatabaseConfiguration(
            _connectionString!,
            _databaseName,
            _outBoxTableName,
            _inBoxTableName,
            _queueStoreTable,
            _schemaName,
            _binaryMessagePayload
        );
    }
}