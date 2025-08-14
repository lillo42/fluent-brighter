using Paramore.Brighter;

namespace Fluent.Brighter;

public sealed class RelationalDatabaseConfigurationBuilder
{
    private string? _connectionString;

    public RelationalDatabaseConfigurationBuilder SetConnectionString(string connectionString)
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