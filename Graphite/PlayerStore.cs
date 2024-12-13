using Graphite.Abstractions;

namespace Graphite;

internal sealed class PlayerStore : IPlayerStore
{
    public IReadOnlyDictionary<string, IPlayer> Players => players;

    private readonly Dictionary<string, IPlayer> players = [];

    public void Add(Player player)
    {
        players[player.Username] = player;
    }

    public void Remove(string? username)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            return;
        }

        players.Remove(username);
    }
}