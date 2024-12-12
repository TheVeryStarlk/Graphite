using Graphite.Abstractions;

namespace Graphite;

internal sealed class PlayerStore : IPlayerStore
{
	public IReadOnlyDictionary<string, IPlayer> Players => players;

	private readonly Dictionary<string, IPlayer> players = [];

	public void Add(Player player)
	{
		players[player.Name] = player;
	}

	public void Remove(Player player)
	{
		players.Remove(player.Name);
	}
}