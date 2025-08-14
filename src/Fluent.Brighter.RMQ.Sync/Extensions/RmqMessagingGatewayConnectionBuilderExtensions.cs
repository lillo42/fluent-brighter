using System;

using Fluent.Brighter.RMQ.Sync;

namespace Fluent.Brighter;

public static class RmqMessagingGatewayConnectionBuilderExtensions
{
    public static RmqMessagingGatewayConnectionBuilder SetAmpq(this RmqMessagingGatewayConnectionBuilder builder,
        Action<AmqpUriSpecificationBuilder> configure)
    {
        var specification = new AmqpUriSpecificationBuilder();
        configure(specification);
        return builder.SetAmpq(specification.Build());
    }

    public static RmqMessagingGatewayConnectionBuilder SetExchange(this RmqMessagingGatewayConnectionBuilder builder,
        Action<ExchangeBuilder> configure)
    {
        var exchangeBuilder = new ExchangeBuilder();
        configure(exchangeBuilder);
        return builder.SetExchange(exchangeBuilder.Build());
    }
    
    public static RmqMessagingGatewayConnectionBuilder SetDeadLetterExchange(this RmqMessagingGatewayConnectionBuilder builder,
        Action<ExchangeBuilder> configure)
    {
        var exchangeBuilder = new ExchangeBuilder();
        configure(exchangeBuilder);
        return builder.SetDeadLetterExchange(exchangeBuilder.Build());
    }
}