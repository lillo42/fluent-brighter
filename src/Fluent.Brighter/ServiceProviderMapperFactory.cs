
using System;

using Microsoft.Extensions.DependencyInjection;

using Paramore.Brighter;

namespace Fluent.Brighter;

public class ServiceProviderMapperFactory(IServiceProvider provider) : IAmAMessageMapperFactory
{
    /// <inheritdoc />
    public IAmAMessageMapper Create(Type messageMapperType)
    {
        return (IAmAMessageMapper)provider.GetRequiredService(messageMapperType);
    }
}