
using System;

namespace Fluent.Brighter.RMQ;

public static class BrighterRegisterExtensions
{
    public static IBrighterRegister UsingRabbitMQ(this IBrighterRegister brighterRegister, Action<RmqConfigurator> configure)
    {
        var configurator = new RmqConfigurator();
        configure(configurator);
        return configurator.AddRabbitMq(brighterRegister);
    }
}