using System;

using Amazon.DynamoDBv2;

using Paramore.Brighter;
using Paramore.Brighter.Locking.DynamoDB.V4;
using Paramore.Brighter.MessagingGateway.AWSSQS.V4;

namespace Fluent.Brighter.AWS.V4;

public class DynamoDbLockingProviderBuilder
{
    private IAmazonDynamoDB? _client;

    public DynamoDbLockingProviderBuilder SetClient(IAmazonDynamoDB client)
    {
        _client = client;
        return this;
    }
    
    private AWSMessagingGatewayConnection? _connection;

    public DynamoDbLockingProviderBuilder SetConnection(AWSMessagingGatewayConnection connection)
    {
        _connection = connection;
        return this;
    }
    
    private TimeProvider _timeProvider = TimeProvider.System;

    public DynamoDbLockingProviderBuilder SetTimeProvider(TimeProvider timeProvider)
    {
        _timeProvider = timeProvider;
        return this;
    }

    private DynamoDbLockingProviderOptions? _options;

    public DynamoDbLockingProviderBuilder SetConfiguration(DynamoDbLockingProviderOptions options)
    {
        _options = options;
        return this;
    }

    internal DynamoDbLockingProvider Build()
    {
        if (_connection == null)
        {
            throw new ConfigurationException("Lock Connection is not set");
        }

        if (_options == null)
        {
            throw new ConfigurationException("Lock configuration is not set");
        }

        if (_client == null)
        {
            var config = new AmazonDynamoDBConfig { RegionEndpoint = _connection.Region };
            _connection.ClientConfigAction?.Invoke(config);
            _client = new AmazonDynamoDBClient(_connection.Credentials,  config);
        }

        return new DynamoDbLockingProvider(_client, _options, _timeProvider);
    }
}