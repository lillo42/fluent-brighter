using Amazon.Scheduler.Model;

using Paramore.Brighter.MessageScheduler.Aws;

namespace Fluent.Brighter.AWS;

public static class SchedulerGroupBuilderExtensions
{
    public static SchedulerGroupBuilder SetTags(this SchedulerGroupBuilder builder, params Tag[] tags)
    {
        return builder.SetTags([.. tags]);
    }

    public static SchedulerGroupBuilder AddTag(this SchedulerGroupBuilder builder, string key, string value)
    {
        return builder.AddTag(new Tag { Key = key, Value = value });
    }

    public static SchedulerGroupBuilder CreateIfMissing(this SchedulerGroupBuilder builder)
    {
        return builder.SetMakeGroup(OnMissingSchedulerGroup.Create);
    }

    public static SchedulerGroupBuilder AssumeExists(this SchedulerGroupBuilder builder)
    {
        return builder.SetMakeGroup(OnMissingSchedulerGroup.Assume);
    }
}