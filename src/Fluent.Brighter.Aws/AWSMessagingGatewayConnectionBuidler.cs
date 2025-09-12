using System;
using Amazon;
using Amazon.Runtime;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.AWSSQS;

namespace Fluent.Brighter.Aws;

public sealed class AWSMessagingGatewayConnectionBuidler
{
    private AWSCredentials _credentials = new AnonymousAWSCredentials();
    public AWSMessagingGatewayConnectionBuidler SetCredentials(AWSCredentials credentials)
    {
        _credentials = credentials;
        return this;
    }

    private RegionEndpoint _region = RegionEndpoint.USEast1;
    public AWSMessagingGatewayConnectionBuidler SetRegion(RegionEndpoint region)
    {
        _region = region;
        return this;
    }

    private Action<ClientConfig>? _clientConfigAction;
    public AWSMessagingGatewayConnectionBuidler SetClientConfigAction(Action<ClientConfig> clientConfigAction)
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