
using System;

using Fluent.Brighter.AWS;

using Paramore.Brighter;
using Paramore.Brighter.MessageScheduler.Aws;

namespace Fluent.Brighter;

public static class SchedulerFactoryBuilderExtensions
{
    public static SchedulerFactoryBuilder SetConnection(this SchedulerFactoryBuilder builder,
            Action<AWSMessagingGatewayConnectionBuilder> configure)
    {
        var connection = new AWSMessagingGatewayConnectionBuilder();
        configure(connection);
        return builder.SetConnection(connection.Build());
    }

    #region Group 

    public static SchedulerFactoryBuilder SetGroup(this SchedulerFactoryBuilder builder,
        Action<SchedulerGroupBuilder> configure)
    {
        var groupBuilder = new SchedulerGroupBuilder();
        configure(groupBuilder);
        return builder.SetGroup(groupBuilder.Build());
    }

    public static SchedulerFactoryBuilder SetGroup(this SchedulerFactoryBuilder builder, string name)
    {
        return builder.SetGroup(x => x.SetName(name));
    }

    #endregion


    #region Message topic as target

    public static SchedulerFactoryBuilder EnableMessageTopicAsTarget(this SchedulerFactoryBuilder builder)
    {
        return builder.SetMessageTopicAsTarget(true);
    }

    public static SchedulerFactoryBuilder DisableMessageTopicAsTarget(this SchedulerFactoryBuilder builder)
    {
        return builder.SetMessageTopicAsTarget(false);
    }

    #endregion

    #region On conflict
    public static SchedulerFactoryBuilder ThrowOnConflict(this SchedulerFactoryBuilder builder)
    {
        return builder.SetOnConflict(OnSchedulerConflict.Throw);
    }

    public static SchedulerFactoryBuilder OverwriteOnConflict(this SchedulerFactoryBuilder builder)
    {
        return builder.SetOnConflict(OnSchedulerConflict.Overwrite);
    }

    #endregion

    #region On missing role
    public static SchedulerFactoryBuilder AssumeRoleIfMissing(this SchedulerFactoryBuilder builder)
    {
        return builder.SetOnMissingRole(OnMissingRole.Assume);
    }

    public static SchedulerFactoryBuilder CreateRoleIfMissing(this SchedulerFactoryBuilder builder)
    {
        return builder.SetOnMissingRole(OnMissingRole.Create);
    }

    public static SchedulerFactoryBuilder ValidateRoleIfMissing(this SchedulerFactoryBuilder builder)
    {
        return builder.SetOnMissingRole(OnMissingRole.Validate);
    }

    #endregion
}
