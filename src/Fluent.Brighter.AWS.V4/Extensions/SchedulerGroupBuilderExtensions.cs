using Amazon.Scheduler.Model;

using Fluent.Brighter.AWS.V4;

using Paramore.Brighter.MessageScheduler.AWS.V4;

namespace Fluent.Brighter;

/// <summary>
/// Extension methods for SchedulerGroupBuilder to provide additional convenience methods
/// for fluent configuration of AWS EventBridge Scheduler groups.
/// </summary>

public static class SchedulerGroupBuilderExtensions
{
    /// <summary>
    /// Sets tags for the scheduler group using a parameter array of tags.
    /// </summary>
    /// <param name="builder">The scheduler group builder instance</param>
    /// <param name="tags">Array of tags to apply to the scheduler group</param>
    /// <returns>The builder instance for method chaining</returns>
    public static SchedulerGroupBuilder SetTags(this SchedulerGroupBuilder builder, params Tag[] tags)
    {
        return builder.SetTags([.. tags]);
    }

    /// <summary>
    /// Adds a single tag to the scheduler group using a key-value pair.
    /// </summary>
    /// <param name="builder">The scheduler group builder instance</param>
    /// <param name="key">The tag key</param>
    /// <param name="value">The tag value</param>
    /// <returns>The builder instance for method chaining</returns>
    public static SchedulerGroupBuilder AddTag(this SchedulerGroupBuilder builder, string key, string value)
    {
        return builder.AddTag(new Tag { Key = key, Value = value });
    }

    /// <summary>
    /// Configures the scheduler group to be created if it doesn't exist.
    /// </summary>
    /// <param name="builder">The scheduler group builder instance</param>
    /// <returns>The builder instance for method chaining</returns>
    public static SchedulerGroupBuilder CreateIfMissing(this SchedulerGroupBuilder builder)
    {
        return builder.SetMakeGroup(OnMissingSchedulerGroup.Create);
    }

    /// <summary>
    /// Configures the scheduler group to be assumed to exist (no validation or creation).
    /// </summary>
    /// <param name="builder">The scheduler group builder instance</param>
    /// <returns>The builder instance for method chaining</returns>
    public static SchedulerGroupBuilder AssumeExists(this SchedulerGroupBuilder builder)
    {
        return builder.SetMakeGroup(OnMissingSchedulerGroup.Assume);
    }
}