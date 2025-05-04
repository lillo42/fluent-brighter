
using System;

namespace Fluent.Brighter.RMQ;

public static class BrighterRegisterExtensions
{
    public static IBrighterConfigurator UsingRabbitMQ(this IBrighterConfigurator brighterRegister, Action<RmqConfigurator> configure)
    {
        var configurator = new RmqConfigurator();
        configure(configurator);
        return configurator.AddRabbitMq(brighterRegister);
    }
}