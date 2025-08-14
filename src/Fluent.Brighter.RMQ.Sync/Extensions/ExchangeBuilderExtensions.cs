using Fluent.Brighter.RMQ.Sync;

using RabbitMQ.Client;

namespace Fluent.Brighter;

public static class ExchangeBuilderExtensions
{
    #region Durable 
    public static ExchangeBuilder EnableDurable(this ExchangeBuilder builder) 
        => builder.SetDurable(true);
    
    public static ExchangeBuilder DisableDurable(this ExchangeBuilder builder)
        => builder.SetDurable(false);
    #endregion

    #region Support delay
    public static ExchangeBuilder EnableSupportDelay(this ExchangeBuilder builder) 
        => builder.SetSupportDelay(true);
    
    public static ExchangeBuilder DisableSupportDelay(this ExchangeBuilder builder) 
        => builder.SetSupportDelay(false);
    #endregion

    #region Exchagen type

    public static ExchangeBuilder DirectType(this ExchangeBuilder builder)
        => builder.SetType(ExchangeType.Direct);
    
    public static ExchangeBuilder FanoutType(this ExchangeBuilder builder)
        => builder.SetType(ExchangeType.Fanout);
    
    public static ExchangeBuilder HeadersType(this ExchangeBuilder builder)
        => builder.SetType(ExchangeType.Headers);
    
    public static ExchangeBuilder TopicType(this ExchangeBuilder builder)
        => builder.SetType(ExchangeType.Topic);
    #endregion
}