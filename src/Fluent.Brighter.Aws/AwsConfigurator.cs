
using System;
using System.Collections.Generic;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.AWSSQS;

namespace Fluent.Brighter.Aws;

public class AwsConfigurator
{
    private readonly List<SqsSubscription> _subscriptions = [];
    private readonly List<SnsPublication> _publications = [];
    private AWSMessagingGatewayConnection _connection;

    public AwsConfigurator()
    {
        _connection = new AwsCredentialBuilder().Build();
    }

    public AwsConfigurator Connection(Action<AwsCredentialBuilder> configure)
    {
        var credentials = new AwsCredentialBuilder();
        configure(credentials);
        _connection = credentials.Build();
        return this;
    }

    public AwsConfigurator AddSubscription<T>(Action<SqsSubscriptionBuilder<T>> configure)
        where T : class, IRequest
    {
        var configurator = new SqsSubscriptionBuilder<T>();
        configure(configurator);

        _subscriptions.Add(configurator.Build());
        return this;
    }


    public AwsConfigurator AddPublication(Action<SnsPublicationBuilder> configure)
    {
        var builder = new SnsPublicationBuilder();
        configure(builder);
        _publications.Add(builder.Build());
        return this;
    }

    internal void Register(IBrighterConfigurator register)
    {

        
    }
}