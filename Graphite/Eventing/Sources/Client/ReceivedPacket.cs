using Graphite.Networking;

namespace Graphite.Eventing.Sources.Client;

public sealed class ReceivedPacket(Graphite.Client client, IPacket packet) : Event<Graphite.Client>(client)
{
	public IPacket Packet => packet;
}