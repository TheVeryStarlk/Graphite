namespace Graphite.Eventing.Sources.Player;

public sealed class Leaving(Graphite.Player player) : Event<Graphite.Player>(player);