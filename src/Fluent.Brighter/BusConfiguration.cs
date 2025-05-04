
namespace Fluent.Brighter;

internal class BusConfiguration
{
    public ExternalBusType Type { get; set; }
}


internal enum ExternalBusType
{
    None = 0,
    FireAndForget = 1,
    RPC = 2
}