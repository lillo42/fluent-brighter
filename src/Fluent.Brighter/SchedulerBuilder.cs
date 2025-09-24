using System;

using Paramore.Brighter;
using Paramore.Brighter.Extensions.DependencyInjection;

namespace Fluent.Brighter;

public sealed class SchedulerBuilder
{
    private Action<IBrighterBuilder> _scheduler = static _ => { };

    public SchedulerBuilder UseScheduler<TSchedulerFactory>(TSchedulerFactory factory)
        where TSchedulerFactory : IAmAMessageSchedulerFactory, IAmARequestSchedulerFactory
    {
        _scheduler = fluent => fluent.UseScheduler(factory);
        return this;
    }

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