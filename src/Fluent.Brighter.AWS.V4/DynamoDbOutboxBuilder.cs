using System;

using Amazon.DynamoDBv2;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.AWSSQS.V4;
using Paramore.Brighter.Observability;
using Paramore.Brighter.Outbox.DynamoDB.V4;

namespace Fluent.Brighter.AWS.V4;

/// <summary>
/// Builder class for fluently configuring a DynamoDB outbox for Paramore.Brighter's message store.
/// Provides methods to set the DynamoDB client, connection configuration, instrumentation options,
/// time provider, and outbox configuration for storing and retrieving messages from Amazon DynamoDB.
/// </summary>
public sealed class DynamoDbOutboxBuilder
{
    private IAmazonDynamoDB? _client;

    /// <summary>
    /// Sets the pre-configured DynamoDB client instance for accessing Amazon DynamoDB.
    /// If not provided, a client will be created using the connection configuration.
    /// </summary>
    /// <param name="client">The pre-configured DynamoDB client</param>
    /// <returns>The builder instance for method chaining</returns>
    public DynamoDbOutboxBuilder SetClient(IAmazonDynamoDB client)
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
    public DynamoDbOutboxBuilder SetConnection(AWSMessagingGatewayConnection connection)
    {
        _connection = connection;
        return this;
    }

    private InstrumentationOptions _instrumentation = InstrumentationOptions.All;

    /// <summary>
    /// Sets the instrumentation options for monitoring and tracing the DynamoDB outbox operations.
    /// </summary>
    /// <param name="instrumentation">The instrumentation options</param>
    /// <returns>The builder instance for method chaining</returns>
    public DynamoDbOutboxBuilder SetInstrumentation(InstrumentationOptions instrumentation)
    {
        _instrumentation = instrumentation;
        return this;
    }

    private TimeProvider _timeProvider = TimeProvider.System;

    /// <summary>
    /// Sets the time provider for timestamp generation, allowing for custom time sources
    /// in testing scenarios or for time synchronization requirements.
    /// </summary>
    /// <param name="timeProvider">The time provider implementation</param>
    /// <returns>The builder instance for method chaining</returns>
    public DynamoDbOutboxBuilder SetTimeProvider(TimeProvider timeProvider)
    {
        _timeProvider = timeProvider;
        return this;
    }

    private DynamoDbConfiguration _configuration = new();

    /// <summary>
    /// Sets the DynamoDB outbox configuration including table name, key structure,
    /// and other DynamoDB-specific parameters.
    /// </summary>
    /// <param name="configuration">The DynamoDB configuration</param>
    /// <returns>The builder instance for method chaining</returns>
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

            _client = new AmazonDynamoDBClient(_connection.Credentials, config);
        }

        return new DynamoDbOutbox(_client, _configuration, _timeProvider, _instrumentation);
    }
}