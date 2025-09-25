using System;

using Amazon.DynamoDBv2;

using Paramore.Brighter;
using Paramore.Brighter.Locking.DynamoDb;
using Paramore.Brighter.MessagingGateway.AWSSQS;

namespace Fluent.Brighter.AWS;

/// <summary>
/// Builder class for fluently configuring a DynamoDB-based locking provider for Paramore.Brighter.
/// Provides methods to set the DynamoDB client, connection configuration, time provider,
/// and locking options for distributed locking using Amazon DynamoDB.
/// </summary>
public class DynamoDbLockingProviderBuilder
{
    private IAmazonDynamoDB? _client;

    /// <summary>
    /// Sets the pre-configured DynamoDB client instance for accessing Amazon DynamoDB.
    /// If not provided, a client will be created using the connection configuration.
    /// </summary>
    /// <param name="client">The pre-configured DynamoDB client</param>
    /// <returns>The builder instance for method chaining</returns>
    public DynamoDbLockingProviderBuilder SetClient(IAmazonDynamoDB client)
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
    public DynamoDbLockingProviderBuilder SetConnection(AWSMessagingGatewayConnection connection)
    {
        _connection = connection;
        return this;
    }

    private TimeProvider _timeProvider = TimeProvider.System;

    /// <summary>
    /// Sets the time provider for lock expiration calculations, allowing for
    /// custom time sources in testing scenarios.
    /// </summary>
    /// <param name="timeProvider">The time provider implementation</param>
    /// <returns>The builder instance for method chaining</returns>
    public DynamoDbLockingProviderBuilder SetTimeProvider(TimeProvider timeProvider)
    {
        _timeProvider = timeProvider;
        return this;
    }

    private DynamoDbLockingProviderOptions? _options;

    /// <summary>
    /// Sets the locking provider configuration options including table name,
    /// lock timeout settings, and other DynamoDB-specific parameters.
    /// </summary>
    /// <param name="options">The locking provider configuration options</param>
    /// <returns>The builder instance for method chaining</returns> 
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
            _client = new AmazonDynamoDBClient(_connection.Credentials, config);
        }

        return new DynamoDbLockingProvider(_client, _options, _timeProvider);
    }
}