using Graphite.Abstractions;

namespace Graphite;

internal sealed class Player(Client client, string name) : IPlayer
{
	public IClient Client => client;

	public string Name => name;

	public void Kick(string reason)
	{
		client.Stop(reason);
	}
}