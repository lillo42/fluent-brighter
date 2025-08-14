using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.RMQ.Sync;

using RabbitMQ.Client;

namespace Fluent.Brighter.RMQ.Sync;

public class ExchangeBuilder
{
    private string? _name;

    public ExchangeBuilder SetName(string name)
    {
        _name = name;
        return this;
    }
    
    private string _type = ExchangeType.Direct;

    public ExchangeBuilder SetType(string type)
    {
        _type = type;
        return this;
    }
    private bool _durable;
    public ExchangeBuilder SetDurable(bool durable)
    {
        _durable = durable;
        return this;
    }

    private bool _supportDelay;
    public ExchangeBuilder SetSupportDelay(bool supportDelay)
    {
        _supportDelay = supportDelay;
        return this;
    }

    internal Exchange Build()
    {
        if (string.IsNullOrEmpty(_name))
        {
            throw new ConfigurationException("Exchange name cannot be null or empty.");
        }
        
        return new Exchange(_name!, _type, _durable, _supportDelay);
    }
}