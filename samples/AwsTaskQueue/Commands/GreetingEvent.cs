using Paramore.Brighter;

namespace AwsTaskQueue.Commands;

public class GreetingEvent(string greeting) : Event(Uuid.New())
{
    public GreetingEvent() : this(string.Empty) { }

    public string Greeting { get; set; } = greeting;
}