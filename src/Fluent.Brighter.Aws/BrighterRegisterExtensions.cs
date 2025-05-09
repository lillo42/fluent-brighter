using System;

namespace Fluent.Brighter.Aws;

public static class BrighterRegisterExtensions
{
    public static IBrighterConfigurator UsingAws(this IBrighterConfigurator register, Action<AwsConfigurator> configure)
    {
        var configurator = new AwsConfigurator();
        configure(configurator);
        configurator.Register(register);
        return register;
    }
}