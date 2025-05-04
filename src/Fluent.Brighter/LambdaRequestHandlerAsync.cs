
using System;
using System.Threading;
using System.Threading.Tasks;

using Paramore.Brighter;

namespace Fluent.Brighter;

public class LambdaRequestHandlerAsync<T>(Func<T, CancellationToken, Task> action) : RequestHandlerAsync<T>
    where T : class, IRequest
{
    public override async Task<T> HandleAsync(T command, CancellationToken cancellationToken = default)
    {
        await action(command, cancellationToken);
        return await base.HandleAsync(command, cancellationToken);
    }
}
