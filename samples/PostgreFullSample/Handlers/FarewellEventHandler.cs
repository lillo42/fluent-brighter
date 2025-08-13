using Paramore.Brighter;

using PostgreFullSample.Commands;

namespace PostgreFullSample.Handlers;

public class FarewellEventHandler : RequestHandler<FarewellEvent>
{
    public override FarewellEvent Handle(FarewellEvent @event)
    {
        Console.BackgroundColor = ConsoleColor.Blue;
        Console.ForegroundColor = ConsoleColor.White;

        Console.WriteLine("Received Farewell. Message Follows");
        Console.WriteLine("----------------------------------");
        Console.WriteLine(@event.Farewell);
        Console.WriteLine("----------------------------------");
        Console.WriteLine("Message Ends");

        Console.ResetColor();
        return base.Handle(@event);
    }
}