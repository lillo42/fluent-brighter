using System;

using Fluent.Brighter.Aws;

namespace Fluent.Brighter;

public static class ProducerExtensions
{
    public static ProducerBuilder AddSnsPublication(this ProducerBuilder builder,
        Action<SnsMessageProducerFactoryBuilder> configure)
    {
        var factory = new SnsMessageProducerFactoryBuilder();
        configure(factory);
        return builder.AddMessageProducerFactory(factory.Build());
    }
    
    public static ProducerBuilder AddSqsPublication(this ProducerBuilder builder,
        Action<SqsMessageProducerFactoryBuilder> configure)
    {
        var factory = new SqsMessageProducerFactoryBuilder();
        configure(factory);
        return builder.AddMessageProducerFactory(factory.Build());
    }  
}