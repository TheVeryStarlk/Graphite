using Graphite.Worlds;

namespace Graphite;

public sealed class Player(Client client, Server server, string username)
{
	public Client Client => client;

	public Server Server => server;

	public string Username => username;

	public World? World { get; private set; }

	public ValueTask SpawnAsync(World world)
	{
		return ValueTask.CompletedTask;
	}
}