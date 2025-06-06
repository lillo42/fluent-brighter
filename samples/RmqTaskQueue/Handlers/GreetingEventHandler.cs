﻿using Paramore.Brighter;

using RmqTaskQueue.Commands;

namespace RmqTaskQueue.Handlers;

public class GreetingEventHandler : RequestHandler<GreetingEvent>
{
    public override GreetingEvent Handle(GreetingEvent @event)
    {
        Console.BackgroundColor = ConsoleColor.Green;
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("Received Greeting. Message Follows");
        Console.WriteLine("----------------------------------");
        Console.WriteLine(@event.Greeting);
        Console.WriteLine("----------------------------------");
        Console.WriteLine("Message Ends");
        Console.ResetColor();

        return base.Handle(@event);
    }
}