using Paramore.Brighter;

namespace SqlServerSample.Commands;

public class FarewellEvent(string farewell) : Event(Id.Random())
{
    public FarewellEvent() : this(string.Empty)
    {
    }

    public string Farewell { get; set; } = farewell;
}