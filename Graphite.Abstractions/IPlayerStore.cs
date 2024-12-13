namespace Graphite.Abstractions;

public interface IPlayerStore
{
    public IReadOnlyDictionary<string, IPlayer> Players { get; }
}