using System;

using Paramore.Brighter;
using Paramore.Brighter.Extensions.DependencyInjection;

namespace Fluent.Brighter;

/// <summary>
/// Builder class for configuring message scheduling in Paramore.Brighter.
/// Provides methods to set up scheduler factories for delayed message delivery
/// using various scheduling implementations.
/// </summary>
public sealed class SchedulerBuilder
{
    private Action<IBrighterBuilder> _scheduler = static _ => { };

    /// <summary>
    /// Configures a specific scheduler factory instance for both message and request scheduling.
    /// </summary>
    /// <typeparam name="TSchedulerFactory">The type of scheduler factory implementing both IAmAMessageSchedulerFactory and IAmARequestSchedulerFactory</typeparam>
    /// <param name="factory">The scheduler factory instance to use</param>
    /// <returns>The builder instance for method chaining</returns>
    /// <remarks>
    /// This method allows you to provide a pre-configured scheduler factory instance
    /// that will handle both message scheduling and request scheduling operations.
    /// </remarks>
    public SchedulerBuilder UseScheduler<TSchedulerFactory>(TSchedulerFactory factory)
        where TSchedulerFactory : IAmAMessageSchedulerFactory, IAmARequestSchedulerFactory
    {
        _scheduler = fluent => fluent.UseScheduler(factory);
        return this;
    }

    /// <summary>
    /// Configures a scheduler factory using a factory function that resolves the factory
    /// from the dependency injection container.
    /// </summary>
    /// <typeparam name="TSchedulerFactory">The type of scheduler factory implementing both IAmAMessageSchedulerFactory and IAmARequestSchedulerFactory</typeparam>
    /// <param name="factory">Factory function to create the scheduler factory from the service provider</param>
    /// <returns>The builder instance for method chaining</returns>
    /// <remarks>
    /// This method allows for deferred resolution of the scheduler factory,
    /// enabling dependency injection and more complex factory creation logic.
    /// </remarks>
    public SchedulerBuilder UseScheduler<TSchedulerFactory>(Func<IServiceProvider, TSchedulerFactory> factory)
            where TSchedulerFactory : IAmAMessageSchedulerFactory, IAmARequestSchedulerFactory
    {
        _scheduler = fluent => fluent.UseScheduler(factory);
        return this;
    }

    internal void SetScheduler(IBrighterBuilder builder)
    {
        _scheduler(builder);
    }
}