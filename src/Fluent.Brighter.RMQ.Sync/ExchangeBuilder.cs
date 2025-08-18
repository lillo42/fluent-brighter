using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.RMQ.Sync;

using RabbitMQ.Client;

namespace Fluent.Brighter.RMQ.Sync;

/// <summary>
/// Fluent builder for configuring RabbitMQ exchange settings
/// </summary>
/// <remarks>
/// Provides a fluent interface to define exchange parameters including name, type, durability, 
/// and delayed message support. Default values are applied where not explicitly set.
/// </remarks>
public class ExchangeBuilder
{
    private string? _name;

    /// <summary>
    /// Sets the name of the RabbitMQ exchange
    /// </summary>
    /// <param name="name">Exchange name (must not be null or empty)</param>
    /// <returns>This builder for fluent chaining</returns>
    public ExchangeBuilder SetName(string name)
    {
        _name = name;
        return this;
    }
    
    private string _type = ExchangeType.Direct;

    /// <summary>
    /// Sets the exchange type (default: Direct)
    /// </summary>
    /// <param name="type">
    /// RabbitMQ exchange type (e.g. Direct, Fanout, Topic, Headers)
    /// </param>
    /// <returns>This builder for fluent chaining</returns>
    public ExchangeBuilder SetType(string type)
    {
        _type = type;
        return this;
    }
    private bool _durable;
    
    /// <summary>
    /// Configures exchange durability (default: non-durable)
    /// </summary>
    /// <param name="durable">
    /// True if the exchange should survive broker restarts, false otherwise
    /// </param>
    /// <returns>This builder for fluent chaining</returns>
    public ExchangeBuilder SetDurable(bool durable)
    {
        _durable = durable;
        return this;
    }

    private bool _supportDelay;
    
    /// <summary>
    /// Enables delayed message support for the exchange
    /// </summary>
    /// <param name="supportDelay">
    /// True to enable RabbitMQ delayed message plugin features, false otherwise
    /// </param>
    /// <returns>This builder for fluent chaining</returns>
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