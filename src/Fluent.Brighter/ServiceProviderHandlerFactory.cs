
using System;

using Microsoft.Extensions.DependencyInjection;

using Paramore.Brighter;

namespace Fluent.Brighter;

public class ServiceProviderHandlerFactory(IServiceProvider provider) : IAmAHandlerFactorySync, IAmAHandlerFactoryAsync
{
    /// <inheritdoc />
    IHandleRequests IAmAHandlerFactorySync.Create(Type handlerType)
    {
        return (IHandleRequests)provider.GetRequiredService(handlerType);
    }

    /// <inheritdoc />
    IHandleRequestsAsync IAmAHandlerFactoryAsync.Create(Type handlerType)
    {
        return (IHandleRequestsAsync)provider.GetRequiredService(handlerType);
    }

    /// <inheritdoc />
    public void Release(IHandleRequests handler)
    {
    }

    /// <inheritdoc />
    public void Release(IHandleRequestsAsync handler)
    {
    }


}