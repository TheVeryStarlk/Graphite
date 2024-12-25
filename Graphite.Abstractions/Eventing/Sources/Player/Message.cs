namespace Graphite.Abstractions.Eventing.Sources.Player;

public sealed class Message : Event<IPlayer>
{
    public required string Content { get; set; }
}