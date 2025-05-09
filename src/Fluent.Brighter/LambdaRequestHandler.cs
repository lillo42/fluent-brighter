
using System;

using Paramore.Brighter;

namespace Fluent.Brighter;

public class LambdaRequestHandler<T>(Action<T> action) : RequestHandler<T>
    where T : class, IRequest
{
    public override T Handle(T command)
    {
        action(command);
        return base.Handle(command);
    }
}

