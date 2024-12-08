using System.Net;

namespace Graphite.Eventing.Sources.Server;

public sealed class Starting(Graphite.Server server) : Event<Graphite.Server>(server)
{
	public IPEndPoint EndPoint { get; set; } = new(IPAddress.Any, 25565);
}