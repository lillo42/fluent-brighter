using Paramore.Brighter;

namespace RmqTaskQueue.Commands;

public class FarewellEvent(string farewell) : Event(Guid.NewGuid())
{
    public FarewellEvent() : this(string.Empty)
    {
    }

    public string Farewell { get; set; } = farewell;
}