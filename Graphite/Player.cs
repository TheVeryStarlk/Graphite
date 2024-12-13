using Graphite.Abstractions;
using Graphite.Abstractions.Worlds;

namespace Graphite;

internal sealed class Player(Client client, string username) : IPlayer
{
	public IClient Client => client;

	public string Username => username;

    public IWorld? World { get; private set; }

    public ValueTask SpawnAsync(IWorld world)
    {
        World = world;
        return ValueTask.CompletedTask;
    }

    public void Kick(string reason)
	{
		client.Stop(reason);
	}
}