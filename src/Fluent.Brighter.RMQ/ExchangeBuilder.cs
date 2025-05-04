
using Paramore.Brighter.MessagingGateway.RMQ;

namespace Fluent.Brighter.RMQ;

public class ExchangeBuilder
{
    private string? _name;
    public ExchangeBuilder Name(string name)
    {
        _name = name;
        return this;
    }

    private string _type = "direct";
    public ExchangeBuilder Type(string type)
    {
        _type = type;
        return this;
    }

    private bool _durable = false;
    public ExchangeBuilder EnableDurable()
    {
        return Durable(true);
    }

    public ExchangeBuilder DisableDurable()
    {
        return Durable(false);
    }

    public ExchangeBuilder Durable(bool durable)
    {
        _durable = durable;
        return this;
    }

    private bool _supportDelay = false;
    public ExchangeBuilder EnableSupportDelay()
    {
        return SupportDelay(true);
    }

    public ExchangeBuilder DisableSupportDelay()
    {
        return SupportDelay(false);
    }

    public ExchangeBuilder SupportDelay(bool supportDelay)
    {
        _supportDelay = supportDelay;
        return this;
    }

    internal Exchange Build()
    {
        return new Exchange(_name, _type, _durable, _supportDelay);
    }
}