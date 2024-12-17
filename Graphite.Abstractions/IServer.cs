namespace Graphite.Abstractions;

public interface IServer
{
    public IReadOnlyDictionary<string, IPlayer> Players { get; }

    public void Stop(string reason);
}