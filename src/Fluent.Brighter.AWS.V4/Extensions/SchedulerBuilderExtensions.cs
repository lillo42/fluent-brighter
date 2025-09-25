using System;

using Fluent.Brighter.AWS.V4;

using Paramore.Brighter.MessageScheduler.AWS.V4;

namespace Fluent.Brighter;

public static class SchedulerBuilderExtensions
{
    public static SchedulerBuilder UseAWSScheduler(this SchedulerBuilder builder, Action<SchedulerFactoryBuilder> configure)
    {
        var factory = new SchedulerFactoryBuilder();
        configure(factory);
        return builder.UseAWSScheduler(factory.Build());
    }

    public static SchedulerBuilder UseAWSScheduler(this SchedulerBuilder builder, AwsSchedulerFactory factory)
    {
        return builder.UseScheduler(factory);
    }
}