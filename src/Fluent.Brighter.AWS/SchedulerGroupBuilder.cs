
using System.Collections.Generic;

using Amazon.Scheduler.Model;

using Paramore.Brighter.MessageScheduler.Aws;

namespace Fluent.Brighter.AWS;

public class SchedulerGroupBuilder
{
    private string _name = "default";
    public SchedulerGroupBuilder SetName(string name)
    {
        _name = name;
        return this;
    }

    private List<Tag> _tags = [new Tag { Key = "Source", Value = "Brighter" }];
    public SchedulerGroupBuilder SetTags(List<Tag> tags)
    {
        _tags = tags;
        return this;
    }

    public SchedulerGroupBuilder AddTag(Tag tag)
    {
        _tags.Add(tag);
        return this;
    }

    private OnMissingSchedulerGroup _makeSchedulerGroup = OnMissingSchedulerGroup.Assume;
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