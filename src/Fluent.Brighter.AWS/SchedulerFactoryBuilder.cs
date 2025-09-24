using System;

using Paramore.Brighter;
using Paramore.Brighter.MessageScheduler.Aws;
using Paramore.Brighter.MessagingGateway.AWSSQS;

namespace Fluent.Brighter.AWS;

public sealed class SchedulerFactoryBuilder
{
    private AWSMessagingGatewayConnection? _connection;

    public SchedulerFactoryBuilder SetConnection(AWSMessagingGatewayConnection connection)
    {
        _connection = connection;
        return this;
    }

    private string _role = "";

    public SchedulerFactoryBuilder SetRole(string role)
    {
        _role = role;
        return this;
    }

    private TimeProvider _timeProvider = TimeProvider.System;
    public SchedulerFactoryBuilder SetTimeProvider(TimeProvider timeProvider)
    {
        _timeProvider = timeProvider;
        return this;
    }

    private SchedulerGroup _group = new();
    public SchedulerFactoryBuilder SetGroup(SchedulerGroup group)
    {
        _group = group;
        return this;
    }

    private Func<Message, string> _getOrCreteMessageSchedulerId = static _ => Uuid.New().ToString("N");
    public SchedulerFactoryBuilder SetGetOrCreateMessageSchedulerId(Func<Message, string> getOrCreateMessageSchedulerId)
    {
        _getOrCreteMessageSchedulerId = getOrCreateMessageSchedulerId;
        return this;
    }

    private Func<IRequest, string> _getOrCreteRequestSchedulerId = static _ => Uuid.New().ToString("N");
    public SchedulerFactoryBuilder SetGetOrCreateRequestSchedulerId(Func<IRequest, string> getOrCreateRequestSchedulerId)
    {
        _getOrCreteRequestSchedulerId = getOrCreateRequestSchedulerId;
        return this;
    }

    private int? _flexibleTimeWindowMinutes;
    public SchedulerFactoryBuilder SetFlexibleTimeWindowMinutes(int? flexibleTimeWindowMinutes)
    {
        _flexibleTimeWindowMinutes = flexibleTimeWindowMinutes;
        return this;
    }

    private RoutingKey _schedulerTopicOrQueue = RoutingKey.Empty;
    public SchedulerFactoryBuilder SetSchedulerTopicOrQueue(RoutingKey schedulerTopicOrQueue)
    {
        _schedulerTopicOrQueue = schedulerTopicOrQueue;
        return this;
    }

    private bool _useMessageTopicAsTarget = true;
    public SchedulerFactoryBuilder SetMessageTopicAsTarget(bool useMessageTopicAsTarget)
    {
        _useMessageTopicAsTarget = useMessageTopicAsTarget;
        return this;
    }

    private OnSchedulerConflict _onConflict;
    public SchedulerFactoryBuilder SetOnConflict(OnSchedulerConflict onConflict)
    {
        _onConflict = onConflict;
        return this;
    }

    private OnMissingRole _missingRole = OnMissingRole.Assume;
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