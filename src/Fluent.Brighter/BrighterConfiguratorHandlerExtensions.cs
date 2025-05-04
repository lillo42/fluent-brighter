using Paramore.Brighter;

namespace Fluent.Brighter;

public static class BrighterConfiguratorHandlerExtensions
{
    public static IBrighterConfigurator AddHandler<TRequest, THandler>(this IBrighterConfigurator register)
        where TRequest : class, IRequest
        where THandler : class, IHandleRequests<TRequest>
    {
        register.HandlerRegistry += static registry => registry.Register<TRequest, THandler>();
        return register;
    }

    public static IBrighterConfigurator AddHandlerAsync<TRequest, THandler>(this IBrighterConfigurator register)
            where TRequest : class, IRequest
            where THandler : class, IHandleRequestsAsync<TRequest>
    {
        register.AsyncHandlerRegistry += static registry => registry.RegisterAsync<TRequest, THandler>();
        return register;
    }
}