using System;

using Amazon;
using Amazon.Runtime;

using Paramore.Brighter.MessagingGateway.AWSSQS;

namespace Fluent.Brighter.Aws;

public class AwsCredentialBuilder
{
    private AWSCredentials _credentials;
    private RegionEndpoint _regionEndpoint;
    private Action<ClientConfig>? _clientConfiguration;


    public AwsCredentialBuilder()
    {
        _credentials = FallbackCredentialsFactory.GetCredentials();
        _regionEndpoint = Amazon.Util.EC2InstanceMetadata.Region;
    }

    public AwsCredentialBuilder Credentials(AWSCredentials credentials)
    {
        _credentials = credentials;
        return this;
    }


    public AwsCredentialBuilder Region(RegionEndpoint regionEndpoint)
    {
        _regionEndpoint = regionEndpoint;
        return this;
    }

    public AwsCredentialBuilder ConfigureClient(Action<ClientConfig> clientConfiguration)
    {
        _clientConfiguration = clientConfiguration;
        return this;
    }

    internal AWSMessagingGatewayConnection Build()
    {
        return new AWSMessagingGatewayConnection(_credentials, _regionEndpoint, _clientConfiguration);
    }
}