using Amazon.DynamoDBv2;

using Paramore.Brighter;
using Paramore.Brighter.Inbox.DynamoDB.V4;
using Paramore.Brighter.MessagingGateway.AWSSQS.V4;
using Paramore.Brighter.Observability;

namespace Fluent.Brighter.AWS.V4;

public sealed class DynamoDbInboxBuilder
{
    private IAmazonDynamoDB? _client;
    public DynamoDbInboxBuilder SetClient(IAmazonDynamoDB client)
    {
        _client = client;
        return this;
    }
    
    private AWSMessagingGatewayConnection? _connection;
    public DynamoDbInboxBuilder SetConnection(AWSMessagingGatewayConnection connection)
    {
        _connection = connection;
        return this;
    }
    
    private string _tableName = "brighter_inbox";
    public DynamoDbInboxBuilder SetTableName(string tableName)
    {
        _tableName = tableName;
        return this;
    }

    private InstrumentationOptions _instrumentation = InstrumentationOptions.All;
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
            var config  = new AmazonDynamoDBConfig { RegionEndpoint = _connection.Region };
            _connection.ClientConfigAction?.Invoke(config);
            _client = new AmazonDynamoDBClient(_connection.Credentials, config);
        }
        
        return new DynamoDbInbox(_client, new DynamoDbInboxConfiguration
        {
            TableName = _tableName
        }, _instrumentation);
    }
}