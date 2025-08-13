using Paramore.Brighter;

namespace Fluent.Brighter;

public sealed class RelationalDatabaseConfigurationBuilder
{
    private string? _connectionString;

    /// <summary>
    /// Sets the connection string for the database. This is a required parameter.
    /// </summary>
    /// <param name="connectionString">The database connection string.</param>
    /// <returns>The current builder instance.</returns>
    public RelationalDatabaseConfigurationBuilder WithConnectionString(string connectionString)
    {
        _connectionString = connectionString;
        return this;
    }

    private string? _databaseName;
    public RelationalDatabaseConfigurationBuilder SetDatabaseName(string? databaseName)
    {
        _databaseName = databaseName;
        return this;
    }

    
    private string? _outBoxTableName;
    public RelationalDatabaseConfigurationBuilder SetOutboxTableName(string? outBoxTableName)
    {
        _outBoxTableName = outBoxTableName;
        return this;
    }

   
    private string? _inboxTableName;
    public RelationalDatabaseConfigurationBuilder SetInboxTableName(string? inboxTableName)
    {
        _inboxTableName = inboxTableName;
        return this;
    }

    
    private string? _queueStoreTable;
    public RelationalDatabaseConfigurationBuilder SetQueueStoreTable(string? queueStoreTable)
    {
        _queueStoreTable = queueStoreTable;
        return this;
    }

    
    private string? _schemaName;
    public RelationalDatabaseConfigurationBuilder SetSchemaName(string? schemaName)
    {
        _schemaName = schemaName;
        return this;
    }

    private bool _binaryMessagePayload;
    public RelationalDatabaseConfigurationBuilder SetBinaryMessagePayload(bool binaryMessagePayload)
    {
        _binaryMessagePayload = binaryMessagePayload;
        return this;
    }


    public RelationalDatabaseConfiguration Build()
    {
        if (string.IsNullOrEmpty(_connectionString))
        {
            throw new ConfigurationException("ConnectionString must be set before building RelationalDatabaseConfiguration.");
        }

        return new RelationalDatabaseConfiguration(
            connectionString: _connectionString!, // Use ! to assert non-null after check
            databaseName: _databaseName,
            outBoxTableName: _outBoxTableName,
            inboxTableName: _inboxTableName,
            queueStoreTable: _queueStoreTable,
            schemaName: _schemaName,
            binaryMessagePayload: _binaryMessagePayload
        );
    } 
}