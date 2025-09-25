using System;

using Paramore.Brighter;
using Paramore.Brighter.MessageScheduler.AWS.V4;
using Paramore.Brighter.MessagingGateway.AWSSQS.V4;

namespace Fluent.Brighter.AWS.V4;

/// <summary>
/// Builder class for fluently configuring an AWS scheduler factory in Paramore.Brighter.
/// Provides methods to set AWS connection, role, scheduling behavior, conflict resolution,
/// and message targeting for scheduled message delivery using AWS EventBridge Scheduler.
/// </summary>
public sealed class SchedulerFactoryBuilder
{
    private AWSMessagingGatewayConnection? _connection;

    /// <summary>
    /// Sets the AWS connection configuration for the scheduler factory.
    /// </summary>
    /// <param name="connection">The AWS connection configuration</param>
    /// <returns>The builder instance for method chaining</returns>
    public SchedulerFactoryBuilder SetConnection(AWSMessagingGatewayConnection connection)
    {
        _connection = connection;
        return this;
    }

    private string _role = "";

    /// <summary>
    /// Sets the IAM role that the scheduler will use to execute scheduled actions.
    /// </summary>
    /// <param name="role">The IAM role name or ARN</param>
    /// <returns>The builder instance for method chaining</returns>
    public SchedulerFactoryBuilder SetRole(string role)
    {
        _role = role;
        return this;
    }

    private TimeProvider _timeProvider = TimeProvider.System;
    
    /// <summary>
    /// Sets the time provider for scheduling operations, allowing for custom time sources
    /// in testing scenarios or for time synchronization requirements.
    /// </summary>
    /// <param name="timeProvider">The time provider implementation</param>
    /// <returns>The builder instance for method chaining</returns>
    public SchedulerFactoryBuilder SetTimeProvider(TimeProvider timeProvider)
    {
        _timeProvider = timeProvider;
        return this;
    }

    private SchedulerGroup _group = new();
    
    /// <summary>
    /// Sets the scheduler group for organizing and managing related schedules.
    /// </summary>
    /// <param name="group">The scheduler group configuration</param>
    /// <returns>The builder instance for method chaining</returns>
    public SchedulerFactoryBuilder SetGroup(SchedulerGroup group)
    {
        _group = group;
        return this;
    }

    private Func<Message, string> _getOrCreteMessageSchedulerId = static _ => Uuid.New().ToString("N");
    
    /// <summary>
    /// Sets the function to generate or retrieve a unique scheduler ID for messages.
    /// </summary>
    /// <param name="getOrCreateMessageSchedulerId">Function to create message scheduler IDs</param>
    /// <returns>The builder instance for method chaining</returns>
    public SchedulerFactoryBuilder SetGetOrCreateMessageSchedulerId(Func<Message, string> getOrCreateMessageSchedulerId)
    {
        _getOrCreteMessageSchedulerId = getOrCreateMessageSchedulerId;
        return this;
    }

    private Func<IRequest, string> _getOrCreteRequestSchedulerId = static _ => Uuid.New().ToString("N");
    
    /// <summary>
    /// Sets the function to generate or retrieve a unique scheduler ID for requests.
    /// </summary>
    /// <param name="getOrCreateRequestSchedulerId">Function to create request scheduler IDs</param>
    /// <returns>The builder instance for method chaining</returns>
    public SchedulerFactoryBuilder SetGetOrCreateRequestSchedulerId(Func<IRequest, string> getOrCreateRequestSchedulerId)
    {
        _getOrCreteRequestSchedulerId = getOrCreateRequestSchedulerId;
        return this;
    }

    private int? _flexibleTimeWindowMinutes;
    
    /// <summary>
    /// Sets the flexible time window in minutes for schedule execution, which allows
    /// AWS to execute the schedule within the specified window for better resource utilization.
    /// </summary>
    /// <param name="flexibleTimeWindowMinutes">The flexible time window in minutes</param>
    /// <returns>The builder instance for method chaining</returns>
    public SchedulerFactoryBuilder SetFlexibleTimeWindowMinutes(int? flexibleTimeWindowMinutes)
    {
        _flexibleTimeWindowMinutes = flexibleTimeWindowMinutes;
        return this;
    }

    private RoutingKey _schedulerTopicOrQueue = RoutingKey.Empty;
    
    /// <summary>
    /// Sets the topic or queue where scheduled messages will be delivered.
    /// </summary>
    /// <param name="schedulerTopicOrQueue">The target topic or queue routing key</param>
    /// <returns>The builder instance for method chaining</returns>
    public SchedulerFactoryBuilder SetSchedulerTopicOrQueue(RoutingKey schedulerTopicOrQueue)
    {
        _schedulerTopicOrQueue = schedulerTopicOrQueue;
        return this;
    }

    private bool _useMessageTopicAsTarget = true;
    
    /// <summary>
    /// Sets whether to use the message's topic as the target for scheduled delivery.
    /// </summary>
    /// <param name="useMessageTopicAsTarget">True to use the message topic as target</param>
    /// <returns>The builder instance for method chaining</returns>
    public SchedulerFactoryBuilder SetMessageTopicAsTarget(bool useMessageTopicAsTarget)
    {
        _useMessageTopicAsTarget = useMessageTopicAsTarget;
        return this;
    }

    private OnSchedulerConflict _onConflict;
    
    /// <summary>
    /// Sets the conflict resolution strategy when scheduling conflicts occur.
    /// </summary>
    /// <param name="onConflict">The conflict resolution strategy</param>
    /// <returns>The builder instance for method chaining</returns>
    public SchedulerFactoryBuilder SetOnConflict(OnSchedulerConflict onConflict)
    {
        _onConflict = onConflict;
        return this;
    }

    private OnMissingRole _missingRole = OnMissingRole.Assume;
    
    /// <summary>
    /// Sets the behavior when the specified IAM role is missing.
    /// </summary>
    /// <param name="onMissingRole">The missing role handling strategy</param>
    /// <returns>The builder instance for method chaining</returns>
    public SchedulerFactoryBuilder SetOnMissingRole(OnMissingRole onMissingRole)
    {
        _missingRole = onMissingRole;
        return this;
    }

    internal AwsSchedulerFactory Build()
    {
        if (_connection == null)
        {
            throw new ConfigurationException("You must provide a connection");
        }

        if (string.IsNullOrWhiteSpace(_role))
        {
            throw new ConfigurationException("You must provide a role");
        }

        return new AwsSchedulerFactory(_connection, _role)
        {
            TimeProvider = _timeProvider,
            Group = _group,
            GetOrCreateMessageSchedulerId = _getOrCreteMessageSchedulerId,
            GetOrCreateRequestSchedulerId = _getOrCreteRequestSchedulerId,
            FlexibleTimeWindowMinutes = _flexibleTimeWindowMinutes,
            SchedulerTopicOrQueue = _schedulerTopicOrQueue,
            UseMessageTopicAsTarget = _useMessageTopicAsTarget,
            OnConflict = _onConflict,
            MakeRole = _missingRole
        };
    }
}