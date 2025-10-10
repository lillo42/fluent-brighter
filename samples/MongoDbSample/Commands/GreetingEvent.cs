using Paramore.Brighter;

namespace MongoDbSample.Commands;

public class GreetingEvent(string greeting) : Event(Guid.NewGuid())
{
    public GreetingEvent() : this(string.Empty) { }

    public string Greeting { get; set; } = greeting;
}