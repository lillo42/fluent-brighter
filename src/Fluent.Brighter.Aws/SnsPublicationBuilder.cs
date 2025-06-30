
using System;
using System.Collections.Generic;

using Amazon;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.AWSSQS;

namespace Fluent.Brighter.Aws;

public class SnsPublicationBuilder
{
    private SnsAttributes? _snsAttributes;
    public SnsPublicationBuilder SnsAttributes(Action<SnsAttributesBuilder> configure)
    {
        var builder = new SnsAttributesBuilder();
        configure(builder);
        return SnsAttributes(builder.Build());
    }

    public SnsPublicationBuilder SnsAttributes(SnsAttributes attributes)
    {
        _snsAttributes = attributes;
        return this;
    }

    private TopicFindBy _findTopicBy = TopicFindBy.Convention;
    public SnsPublicationBuilder FindTopicByName()
    {
        _findTopicBy = TopicFindBy.Name;
        return this;
    }

    public SnsPublicationBuilder FindTopicByArn()
    {
        _findTopicBy = TopicFindBy.Arn;
        return this;
    }

    public SnsPublicationBuilder FindTopicByConvention()
    {
        _findTopicBy = TopicFindBy.Convention;
        return this;
    }

    public SnsPublicationBuilder FindTopicBy(TopicFindBy findBy)
    {
        _findTopicBy = findBy;
        return this;
    }

    private RoutingKey? _topicName;
    public SnsPublicationBuilder TopicName(string topicName)
    {
        _topicName = new RoutingKey(topicName);
        return this;
    }

    private string? _topicArn;
    public SnsPublicationBuilder TopicArn(string topicArn)
    {
        _topicArn = topicArn.ToString();
        _findTopicBy = TopicFindBy.Arn;
        if (RoutingKey.IsNullOrEmpty(_topicName) && Arn.TryParse(topicArn, out var arn))
        {
            _topicName = new RoutingKey(arn.Resource);
        }

        return this;
    }

    private int _maxOutStandingMessages = -1;
    public SnsPublicationBuilder MaxOutStandingMessages(int maxOutStandingMessages)
    {
        _maxOutStandingMessages = maxOutStandingMessages;
        return this;
    }

    private int _maxOutStandingCheckIntervalMilliSeconds;
    public SnsPublicationBuilder MaxOutStandingCheckInterval(TimeSpan maxOutStandingCheckInterval)
    {
        return MaxOutStandingCheckIntervalMilliSeconds(Convert.ToInt32(maxOutStandingCheckInterval.TotalMilliseconds));
    }

    public SnsPublicationBuilder MaxOutStandingCheckIntervalMilliSeconds(int maxOutStandingCheckIntervalMilliSeconds)
    {
        _maxOutStandingCheckIntervalMilliSeconds = maxOutStandingCheckIntervalMilliSeconds;
        return this;
    }

    private Dictionary<string, object>? _outboxBag;
    public SnsPublicationBuilder OutboxBag(string key, object value)
    {
        _outboxBag ??= [];
        _outboxBag[key] = value;
        return this;
    }

    public SnsPublicationBuilder OutboxBag(IEnumerable<KeyValuePair<string, object>> bag)
    {
        _outboxBag ??= [];
        foreach (var item in bag)
        {
            _outboxBag[item.Key] = item.Value;
        }

        return this;
    }


    internal SnsPublication Build()
    {
        return null!;
        /*new SnsPublication
        {
            FindTopicBy = _findTopicBy,
            SnsAttributes = _snsAttributes,
            Topic = _topicName,
            TopicArn = _topicArn,
            MaxOutStandingMessages = _maxOutStandingMessages,
            MaxOutStandingCheckIntervalMilliSeconds = _maxOutStandingCheckIntervalMilliSeconds,
            OutBoxBag = _outboxBag
        };*/
    }
}