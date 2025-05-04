using System;

namespace Fluent.Brighter.Aws;

public static class BrighterRegisterExtensions
{
    public static IBrighterRegister UsingAws(this IBrighterRegister register, Action<AwsConfigurator> configure)
    {
        var configurator = new AwsConfigurator();
        configure(configurator);
        configurator.Register(register);
        return register;
    }
}