
using System;
using System.Collections.Generic;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.RMQ;

namespace Fluent.Brighter.RMQ;

public class RmqPublicationBuilder
{
    private int _waitForConfirmsTimeOutInMilliseconds = 500;
    public RmqPublicationBuilder WaitForConfirmsTimeOut(TimeSpan waitForConfirmsTimeOut)
    {
        return WaitForConfirmsTimeOutInMilliseconds(Convert.ToInt32(waitForConfirmsTimeOut.TotalMilliseconds));
    }

    public RmqPublicationBuilder WaitForConfirmsTimeOutInMilliseconds(int waitForConfirmsTimeOutInMilliseconds)
    {
        _waitForConfirmsTimeOutInMilliseconds = waitForConfirmsTimeOutInMilliseconds;
        return this;
    }

    private OnMissingChannel _makeChannel = OnMissingChannel.Create;
    public RmqPublicationBuilder CreateExchangeIfMissing()
    {
        return MakeExchange(OnMissingChannel.Create);
    }

    public RmqPublicationBuilder ValidateIfExchangeExists()
    {
        return MakeExchange(OnMissingChannel.Validate);
    }

    public RmqPublicationBuilder AssumeExchangeExists()
    {
        return MakeExchange(OnMissingChannel.Assume);
    }

    public RmqPublicationBuilder MakeExchange(OnMissingChannel makeChannel)
    {
        _makeChannel = makeChannel;
        return this;
    }

    private int _maxOutStandingMessages = -1;
    public RmqPublicationBuilder MaxOutStandingMessages(int maxOutStandingMessages)
    {
        _maxOutStandingMessages = maxOutStandingMessages;
        return this;
    }

    private int _maxOutStandingCheckIntervalMilliSeconds;
    public RmqPublicationBuilder MaxOutStandingCheckInterval(TimeSpan maxOutStandingCheckInterval)
    {
        return MaxOutStandingCheckIntervalMilliSeconds(Convert.ToInt32(maxOutStandingCheckInterval.TotalMilliseconds));
    }

    public RmqPublicationBuilder MaxOutStandingCheckIntervalMilliSeconds(int maxOutStandingCheckIntervalMilliSeconds)
    {
        _maxOutStandingCheckIntervalMilliSeconds = maxOutStandingCheckIntervalMilliSeconds;
        return this;
    }

    private Dictionary<string, object>? _outboxBag;
    public RmqPublicationBuilder OutboxBag(string key, object value)
    {
        _outboxBag ??= [];
        _outboxBag[key] = value;
        return this;
    }

    public RmqPublicationBuilder OutboxBag(IEnumerable<KeyValuePair<string, object>> bag)
    {
        _outboxBag ??= [];
        foreach (var item in bag)
        {
            _outboxBag[item.Key] = item.Value;
        }

        return this;
    }

    internal RmqPublication Build()
    {
        return new RmqPublication
        {
            WaitForConfirmsTimeOutInMilliseconds = _waitForConfirmsTimeOutInMilliseconds,
            MakeChannels = _makeChannel,
            MaxOutStandingMessages = _maxOutStandingMessages,
            MaxOutStandingCheckIntervalMilliSeconds = _maxOutStandingCheckIntervalMilliSeconds,
            OutBoxBag = _outboxBag
        };
    }
}