namespace Graphite;

public sealed class Player(Client client)
{
	public void Disconnect(string reason)
	{
		client.Stop(reason);
	}
}