using System;

using Polly;
using Polly.Registry;

namespace Fluent.Brighter;

/// <summary>
/// Provides extension methods for configuring policy registries through <see cref="IBrighterConfigurator"/>.
/// Supports both direct registry assignment and incremental policy registration.
/// </summary>
public static class BrighterConfiguratorPolicyRegistryExtensions
{
    /// <summary>
    /// Sets a custom policy registry for the Brighter configuration.
    /// </summary>
    /// <param name="configurator">The configurator instance to extend.</param>
    /// <param name="registry">The policy registry implementation to use.</param>
    /// <returns>The configurator instance for fluent chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="configurator"/> is <see langword="null"/>.</exception>
    public static IBrighterConfigurator PolicyRegistry(IBrighterConfigurator configurator, IPolicyRegistry<string> registry)
    {
        if (configurator == null)
        {
            throw new ArgumentNullException(nameof(configurator));
        }

        configurator.Options.PolicyRegistry = registry;
        return configurator;
    }

    /// <summary>
    /// Adds a policy to the configurator's policy registry under the specified key.
    /// Creates a new <see cref="PolicyRegistry"/> if none exists.
    /// </summary>
    /// <param name="configurator">The configurator instance to extend.</param>
    /// <param name="key">The key to associate with the policy.</param>
    /// <param name="policy">The policy to register.</param>
    /// <returns>The configurator instance for fluent chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="configurator"/> is <see langword="null"/>.</exception>
    /// <remarks>
    /// If the configurator's current registry is not a <see cref="PolicyRegistry"/>,
    /// this method will replace it with a new <see cref="PolicyRegistry"/> instance.
    /// </remarks>
    public static IBrighterConfigurator AddPolicyRegistry(IBrighterConfigurator configurator, string key, IsPolicy policy)
    {
        if (configurator == null)
        {
            throw new ArgumentNullException(nameof(configurator));
        }

        if (!(configurator.Options.PolicyRegistry is PolicyRegistry))
        {
            configurator.Options.PolicyRegistry = new PolicyRegistry();
        }

        var policyRegistry = (PolicyRegistry)configurator.Options.PolicyRegistry;
        policyRegistry.Add(key, policy);
        return configurator;
    }
}