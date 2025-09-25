using Amazon.DynamoDBv2;

using Paramore.Brighter;
using Paramore.Brighter.Inbox.DynamoDB;
using Paramore.Brighter.MessagingGateway.AWSSQS;
using Paramore.Brighter.Observability;

namespace Fluent.Brighter.AWS;

/// <summary>
/// Builder class for fluently configuring a DynamoDB inbox for Paramore.Brighter's message store.
/// Provides methods to set AWS DynamoDB client, connection configuration, table name,
/// and instrumentation options for storing and retrieving messages from Amazon DynamoDB.
/// </summary>
public sealed class DynamoDbInboxBuilder
{
    private IAmazonDynamoDB? _client;

    /// <summary>
    /// Sets the pre-configured DynamoDB client instance for accessing Amazon DynamoDB.
    /// If not provided, a client will be created using the connection configuration.
    /// </summary>
    /// <param name="client">The pre-configured DynamoDB client</param>
    /// <returns>The builder instance for method chaining</returns>
    public DynamoDbInboxBuilder SetClient(IAmazonDynamoDB client)
    {
        _client = client;
        return this;
    }

    private AWSMessagingGatewayConnection? _connection;

    /// <summary>
    /// Sets the AWS messaging gateway connection configuration for creating
    /// a DynamoDB client, including credentials, region, and client configuration.
    /// </summary>
    /// <param name="connection">The AWS connection configuration</param>
    /// <returns>The builder instance for method chaining</returns>
    public DynamoDbInboxBuilder SetConnection(AWSMessagingGatewayConnection connection)
    {
        _connection = connection;
        return this;
    }

    private string _tableName = "brighter_inbox";

    /// <summary>
    /// Sets the name of the DynamoDB table used for storing inbox messages.
    /// Defaults to "brighter_inbox" if not specified.
    /// </summary>
    /// <param name="tableName">The name of the DynamoDB table</param>
    /// <returns>The builder instance for method chaining</returns>
    public DynamoDbInboxBuilder SetTableName(string tableName)
    {
        _tableName = tableName;
        return this;
    }

    private InstrumentationOptions _instrumentation = InstrumentationOptions.All;

    /// <summary>
    /// Sets the instrumentation options for monitoring and tracing the DynamoDB inbox operations.
    /// </summary>
    /// <param name="instrumentation">The instrumentation options</param>
    /// <returns>The builder instance for method chaining</returns>
    public DynamoDbInboxBuilder SetInstrumentation(InstrumentationOptions instrumentation)
    {
        _instrumentation = instrumentation;
        return this;
    }

    internal DynamoDbInbox Build()
    {
        if (_connection == null)
        {
            throw new ConfigurationException("Connection not set");
        }

        if (_client == null)
        {
            var config = new AmazonDynamoDBConfig { RegionEndpoint = _connection.Region };
            _connection.ClientConfigAction?.Invoke(config);
            _client = new AmazonDynamoDBClient(_connection.Credentials, config);
        }

        return new DynamoDbInbox(_client, new DynamoDbInboxConfiguration
        {
            TableName = _tableName
        }, _instrumentation);
    }
}