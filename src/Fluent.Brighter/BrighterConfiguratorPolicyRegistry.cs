
using Polly;
using Polly.Registry;

namespace Fluent.Brighter;

public static class BrighterConfiguratorPolicyRegistryExtensions
{
    public static IBrighterConfigurator PolicyRegistry(IBrighterConfigurator configurator, IPolicyRegistry<string> registry)
    {
        configurator.Options.PolicyRegistry = registry;
        return configurator;
    }

    public static IBrighterConfigurator AddPolicyRegistry(IBrighterConfigurator configurator, string key, IsPolicy policy)
    {
        if (!(configurator.Options.PolicyRegistry is PolicyRegistry))
        {
            configurator.Options.PolicyRegistry = new PolicyRegistry();
        }

        var policyRegistry = (PolicyRegistry)configurator.Options.PolicyRegistry;
        policyRegistry.Add(key, policy);
        return configurator;
    }
}