using Fluent.Brighter.Aws;

using Paramore.Brighter.MessagingGateway.AWSSQS;

namespace Fluent.Brighter;

public static class SnsAttributesBuilderExtensions
{
    #region Content Based Deduplication
    public static SnsAttributesBuilder EnableContentBasedDeduplication(this SnsAttributesBuilder builder)
        => builder.SetContentBasedDeduplication(true);

    public static SnsAttributesBuilder DisableContentBasedDeduplication(this SnsAttributesBuilder builder)
        => builder.SetContentBasedDeduplication(false);
    #endregion

    #region Set Type

    public static SnsAttributesBuilder UseStandardSns(this SnsAttributesBuilder builder)
        => builder.SetType(SqsType.Standard);
    
    public static SnsAttributesBuilder UseFifoSns(this SnsAttributesBuilder builder)
        => builder.SetType(SqsType.Fifo);
    #endregion
}