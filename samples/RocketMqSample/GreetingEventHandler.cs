using Paramore.Brighter;

namespace RocketMqSample;

public class GreetingEventHandler : RequestHandler<GreetingEvent>
{
    public override GreetingEvent Handle(GreetingEvent @event)
    {
        Console.WriteLine("===== Hello, {0}", @event.Greeting);
        return base.Handle(@event);
    }
}