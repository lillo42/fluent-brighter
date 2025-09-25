using System;

using Fluent.Brighter.AWS.V4;

using Paramore.Brighter;
using Paramore.Brighter.MessageScheduler.AWS.V4;

namespace Fluent.Brighter;

/// <summary>
/// Extension methods for SchedulerFactoryBuilder to provide fluent configuration
/// for AWS EventBridge Scheduler connections, groups, and scheduling behavior.
/// </summary>
public static class SchedulerFactoryBuilderExtensions
{
    /// <summary>
    /// Sets the AWS connection configuration using a builder pattern for the scheduler factory.
    /// </summary>
    /// <param name="builder">The scheduler factory builder instance</param>
    /// <param name="configure">Action to configure the AWS connection builder</param>
    /// <returns>The scheduler factory builder instance for method chaining</returns>
    public static SchedulerFactoryBuilder SetConnection(this SchedulerFactoryBuilder builder,
            Action<AWSMessagingGatewayConnectionBuilder> configure)
    {
        var connection = new AWSMessagingGatewayConnectionBuilder();
        configure(connection);
        return builder.SetConnection(connection.Build());
    }

    #region Group 
    
    /// <summary>
    /// Sets the scheduler group configuration using a builder pattern for fluent configuration.
    /// </summary>
    /// <param name="builder">The scheduler factory builder instance</param>
    /// <param name="configure">Action to configure the scheduler group builder</param>
    /// <returns>The scheduler factory builder instance for method chaining</returns>
    public static SchedulerFactoryBuilder SetGroup(this SchedulerFactoryBuilder builder,
        Action<SchedulerGroupBuilder> configure)
    {
        var groupBuilder = new SchedulerGroupBuilder();
        configure(groupBuilder);
        return builder.SetGroup(groupBuilder.Build());
    }

    /// <summary>
    /// Sets the scheduler group name directly.
    /// </summary>
    /// <param name="builder">The scheduler factory builder instance</param>
    /// <param name="name">The name of the scheduler group</param>
    /// <returns>The scheduler factory builder instance for method chaining</returns>
    public static SchedulerFactoryBuilder SetGroup(this SchedulerFactoryBuilder builder, string name)
    {
        return builder.SetGroup(x => x.SetName(name));
    }

    #endregion

    #region Message topic as target

    /// <summary>
    /// Enables using the message's topic as the target for scheduled delivery.
    /// </summary>
    /// <param name="builder">The scheduler factory builder instance</param>
    /// <returns>The scheduler factory builder instance for method chaining</returns>
    public static SchedulerFactoryBuilder EnableMessageTopicAsTarget(this SchedulerFactoryBuilder builder)
    {
        return builder.SetMessageTopicAsTarget(true);
    }

    /// <summary>
    /// Disables using the message's topic as the target, requiring explicit target configuration.
    /// </summary>
    /// <param name="builder">The scheduler factory builder instance</param>
    /// <returns>The scheduler factory builder instance for method chaining</returns>
    public static SchedulerFactoryBuilder DisableMessageTopicAsTarget(this SchedulerFactoryBuilder builder)
    {
        return builder.SetMessageTopicAsTarget(false);
    }

    #endregion

    #region On conflict
    
    /// <summary>
    /// Configures the scheduler to throw an exception when scheduling conflicts occur.
    /// </summary>
    /// <param name="builder">The scheduler factory builder instance</param>
    /// <returns>The scheduler factory builder instance for method chaining</returns>
    public static SchedulerFactoryBuilder ThrowOnConflict(this SchedulerFactoryBuilder builder)
    {
        return builder.SetOnConflict(OnSchedulerConflict.Throw);
    }

    /// <summary>
    /// Configures the scheduler to overwrite existing schedules when conflicts occur.
    /// </summary>
    /// <param name="builder">The scheduler factory builder instance</param>
    /// <returns>The scheduler factory builder instance for method chaining</returns>
    public static SchedulerFactoryBuilder OverwriteOnConflict(this SchedulerFactoryBuilder builder)
    {
        return builder.SetOnConflict(OnSchedulerConflict.Overwrite);
    }

    #endregion

    #region On missing role
    
    /// <summary>
    /// Configures the scheduler to assume the IAM role exists (no validation or creation).
    /// </summary>
    /// <param name="builder">The scheduler factory builder instance</param>
    /// <returns>The scheduler factory builder instance for method chaining</returns>
    public static SchedulerFactoryBuilder AssumeRoleIfMissing(this SchedulerFactoryBuilder builder)
    {
        return builder.SetOnMissingRole(OnMissingRole.Assume);
    }

    /// <summary>
    /// Configures the scheduler to create the IAM role if it doesn't exist.
    /// </summary>
    /// <param name="builder">The scheduler factory builder instance</param>
    /// <returns>The scheduler factory builder instance for method chaining</returns>
    public static SchedulerFactoryBuilder CreateRoleIfMissing(this SchedulerFactoryBuilder builder)
    {
        return builder.SetOnMissingRole(OnMissingRole.Create);
    }

    /// <summary>
    /// Configures the scheduler to validate the IAM role exists and throw an exception if not found.
    /// </summary>
    /// <param name="builder">The scheduler factory builder instance</param>
    /// <returns>The scheduler factory builder instance for method chaining</returns>
    public static SchedulerFactoryBuilder ValidateRoleIfMissing(this SchedulerFactoryBuilder builder)
    {
        return builder.SetOnMissingRole(OnMissingRole.Validate);
    }

    #endregion
}
