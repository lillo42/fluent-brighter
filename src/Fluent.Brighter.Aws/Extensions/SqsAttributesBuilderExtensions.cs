using System;

using Fluent.Brighter.Aws;

using Paramore.Brighter.MessagingGateway.AWSSQS;

namespace Fluent.Brighter;

public static class SqsAttributesBuilderExtensions
{
    #region Raw Nessage Delivery
    public static SqsAttributesBuilder EnableRawMessageDelivery(this SqsAttributesBuilder builder)
        => builder.SetRawMessageDelivery(true);
    
    public static SqsAttributesBuilder DisableRawMessageDelivery(this SqsAttributesBuilder builder)
        => builder.SetRawMessageDelivery(false);
    #endregion
    
    #region Content Based Deduplication
    public static SqsAttributesBuilder EnableContentBasedDeduplication(this SqsAttributesBuilder builder)
        => builder.SetContentBasedDeduplication(true);
    
    public static SqsAttributesBuilder DisableContentBasedDeduplication(this SqsAttributesBuilder builder)
        => builder.SetContentBasedDeduplication(false);
    #endregion
    
    #region Sqs Type
    public static SqsAttributesBuilder UseStandardQueue(this SqsAttributesBuilder builder)
        => builder.SetType(SqsType.Standard);
    
    public static SqsAttributesBuilder UseFifoQueue(this SqsAttributesBuilder builder)
        => builder.SetType(SqsType.Standard);
    #endregion
    
    #region Deduplication Scope
    public static SqsAttributesBuilder UseMessageGroupDeduplicationScope(this SqsAttributesBuilder builder)
        => builder.SetDeduplicationScope(DeduplicationScope.MessageGroup);
    
    public static SqsAttributesBuilder UseQueueDeduplicationScope(this SqsAttributesBuilder builder)
        => builder.SetDeduplicationScope(DeduplicationScope.Queue);
    #endregion
    
    #region Fifo Throughput Limit
    public static SqsAttributesBuilder UsePerQueueThroughputLimit(this SqsAttributesBuilder builder)
        => builder.SetFifoThroughputLimit(FifoThroughputLimit.PerQueue);
    
    public static SqsAttributesBuilder UsePerMessageGroupIdThroughputLimit(this SqsAttributesBuilder builder)
        => builder.SetFifoThroughputLimit(FifoThroughputLimit.PerMessageGroupId);
    #endregion
    
    #region Redrive Policy

    public static SqsAttributesBuilder SetRedrivePolicy(this SqsAttributesBuilder builder,
        Action<RedrivePolicyBuilder> configure)
    {
        var redrivePolicyBuilder = new RedrivePolicyBuilder();
        configure(redrivePolicyBuilder);
        return builder.SetRedrivePolicy(redrivePolicyBuilder.Build());
    }
    
    #endregion
}