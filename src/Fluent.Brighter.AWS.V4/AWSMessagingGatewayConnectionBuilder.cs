using System;

using Amazon;
using Amazon.Runtime;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.AWSSQS.V4;

namespace Fluent.Brighter.AWS.V4;

/// <summary>
/// Builder class for fluently configuring an AWS messaging gateway connection.
/// Provides methods to set AWS credentials, region, and client configuration for
/// establishing connections to AWS services like SQS and SNS.
/// </summary>
public sealed class AWSMessagingGatewayConnectionBuilder
{
    private AWSCredentials _credentials = new AnonymousAWSCredentials();

    /// <summary>
    /// Sets the AWS credentials for authenticating with AWS services.
    /// </summary>
    /// <param name="credentials">The AWS credentials (e.g., BasicAWSCredentials, SessionAWSCredentials)</param>
    /// <returns>The builder instance for method chaining</returns>
    public AWSMessagingGatewayConnectionBuilder SetCredentials(AWSCredentials credentials)
    {
        _credentials = credentials;
        return this;
    }

    private RegionEndpoint _region = RegionEndpoint.USEast1;

    /// <summary>
    /// Sets the AWS region endpoint where the messaging services are located.
    /// </summary>
    /// <param name="region">The AWS region endpoint</param>
    /// <returns>The builder instance for method chaining</returns>
    public AWSMessagingGatewayConnectionBuilder SetRegion(RegionEndpoint region)
    {
        _region = region;
        return this;
    }

    private Action<ClientConfig>? _clientConfigAction;

    /// <summary>
    /// Sets a custom client configuration action for fine-grained control over
    /// AWS service client settings like timeout, retry policy, and endpoint configuration.
    /// </summary>
    /// <param name="clientConfigAction">Action to configure AWS client settings</param>
    /// <returns>The builder instance for method chaining</returns>
    public AWSMessagingGatewayConnectionBuilder SetClientConfigAction(Action<ClientConfig> clientConfigAction)
    {
        _clientConfigAction = clientConfigAction;
        return this;
    }

    internal AWSMessagingGatewayConnection Build()
    {
        if (_credentials == null)
        {
            throw new ConfigurationException("Credentials must be set.");
        }

        return new AWSMessagingGatewayConnection(_credentials, _region, _clientConfigAction);
    }
}