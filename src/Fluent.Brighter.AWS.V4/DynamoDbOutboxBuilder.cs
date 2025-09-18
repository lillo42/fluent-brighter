using System;

using Amazon.DynamoDBv2;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.AWSSQS.V4;
using Paramore.Brighter.Observability;
using Paramore.Brighter.Outbox.DynamoDB;
using Paramore.Brighter.Outbox.DynamoDB.V4;

namespace Fluent.Brighter.AWS.V4;

public sealed class DynamoDbOutboxBuilder
{
    private IAmazonDynamoDB? _client;
    public DynamoDbOutboxBuilder SetClient(IAmazonDynamoDB client)
    {
        _client = client;
        return this;
    }
    
    private AWSMessagingGatewayConnection? _connection;
    public DynamoDbOutboxBuilder SetConnection(AWSMessagingGatewayConnection connection)
    {
        _connection = connection;
        return this;
    }
    
    private InstrumentationOptions _instrumentation = InstrumentationOptions.All;
    public DynamoDbOutboxBuilder SetInstrumentation(InstrumentationOptions instrumentation)
    {
        _instrumentation = instrumentation;
        return this;
    }
    
    private TimeProvider _timeProvider = TimeProvider.System;
    public DynamoDbOutboxBuilder SetTimeProvider(TimeProvider timeProvider)
    {
        _timeProvider = timeProvider;
        return this;
    }

    private DynamoDbConfiguration _configuration = new();

    public DynamoDbOutboxBuilder SetConfiguration(DynamoDbConfiguration configuration)
    {
        _configuration = configuration;
        return this;
    }

    internal DynamoDbOutbox Build()
    {
        if (_connection == null)
        {
            throw new ConfigurationException("Connection is not set");
        }

        if (_client == null)
        {
            var config = new AmazonDynamoDBConfig { RegionEndpoint = _connection.Region };
            _connection.ClientConfigAction?.Invoke(config);
            
            _client = new AmazonDynamoDBClient(_connection.Credentials,  config);
        }
        
        return new DynamoDbOutbox(_client, _configuration, _timeProvider, _instrumentation);
    }
}