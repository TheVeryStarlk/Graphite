namespace Graphite.Eventing.Sources.Player;

public sealed record Leaving(Graphite.Player Player) : Event<Graphite.Player>(Player);