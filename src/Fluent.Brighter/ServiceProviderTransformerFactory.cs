
using System;

using Microsoft.Extensions.DependencyInjection;

using Paramore.Brighter;

namespace Fluent.Brighter;

public class ServiceProviderTransformerFactory(IServiceProvider provider) : IAmAMessageTransformerFactory
{
    /// <inheritdoc/>
    public IAmAMessageTransformAsync Create(Type transformerType)
    {
        return (IAmAMessageTransformAsync)provider.GetRequiredService(transformerType);
    }

    /// <inheritdoc/>
    public void Release(IAmAMessageTransformAsync transformer)
    {
    }
}