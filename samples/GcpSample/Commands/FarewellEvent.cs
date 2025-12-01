using Paramore.Brighter;

namespace GcpSample.Commands;

public class FarewellEvent(string farewell) : Event(Uuid.New())
{
    public FarewellEvent() : this(string.Empty)
    {
    }

    public string Farewell { get; set; } = farewell;
}