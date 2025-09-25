using System.Collections.Generic;

using Amazon.Scheduler.Model;

using Paramore.Brighter.MessageScheduler.AWS.V4;

namespace Fluent.Brighter.AWS.V4;

/// <summary>
/// Builder class for fluently configuring a scheduler group for AWS EventBridge Scheduler.
/// Provides methods to set group name, tags, and creation behavior for organizing
/// and managing related schedules in Paramore.Brighter.
/// </summary>
public class SchedulerGroupBuilder
{
    private string _name = "default";
    
    /// <summary>
    /// Sets the name of the scheduler group for organizing related schedules.
    /// </summary>
    /// <param name="name">The name of the scheduler group</param>
    /// <returns>The builder instance for method chaining</returns>
    public SchedulerGroupBuilder SetName(string name)
    {
        _name = name;
        return this;
    }

    private List<Tag> _tags = [new Tag { Key = "Source", Value = "Brighter" }];
    
    /// <summary>
    /// Sets the tags for the scheduler group, which help with organization,
    /// cost allocation, and access control in AWS.
    /// </summary>
    /// <param name="tags">List of tags to apply to the scheduler group</param>
    /// <returns>The builder instance for method chaining</returns>
    public SchedulerGroupBuilder SetTags(List<Tag> tags)
    {
        _tags = tags;
        return this;
    }

    /// <summary>
    /// Adds a single tag to the scheduler group.
    /// </summary>
    /// <param name="tag">The tag to add</param>
    /// <returns>The builder instance for method chaining</returns>
    public SchedulerGroupBuilder AddTag(Tag tag)
    {
        _tags.Add(tag);
        return this;
    }

    private OnMissingSchedulerGroup _makeSchedulerGroup = OnMissingSchedulerGroup.Assume;
    
    /// <summary>
    /// Sets the behavior when the scheduler group doesn't exist.
    /// </summary>
    /// <param name="makeSchedulerGroup">The missing group handling strategy</param>
    /// <returns>The builder instance for method chaining</returns>
    public SchedulerGroupBuilder SetMakeGroup(OnMissingSchedulerGroup makeSchedulerGroup)
    {
        _makeSchedulerGroup = makeSchedulerGroup;
        return this;
    }

    internal SchedulerGroup Build()
    {
        return new SchedulerGroup
        {
            Name = _name,
            Tags = _tags,
            MakeSchedulerGroup = _makeSchedulerGroup
        };
    }
}