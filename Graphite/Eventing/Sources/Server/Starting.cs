using System.Net;

namespace Graphite.Eventing.Sources.Server;

public sealed class Starting : Event<Graphite.Server>
{
	public IPEndPoint EndPoint { get; set; } = new(IPAddress.Any, 25565);
}