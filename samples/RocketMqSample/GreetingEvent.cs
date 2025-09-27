using Paramore.Brighter;

namespace RocketMqSample;

public class GreetingEvent(string greeting) : Event(Id.Random())
{
    public GreetingEvent() : this(string.Empty) { }

    public string Greeting { get; set; } = greeting;
}