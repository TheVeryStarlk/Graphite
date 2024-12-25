namespace Graphite.Abstractions.Eventing.Sources.Player;

public sealed class Moved : Event<IPlayer>
{
    public required float X { get; init; }

    public required float Y { get; init; }

    public required float Z { get; init; }

    public required byte Yaw { get; init; }

    public required byte Pitch { get; init; }
}