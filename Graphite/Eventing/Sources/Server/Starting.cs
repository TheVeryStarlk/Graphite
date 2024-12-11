using System.Net;

namespace Graphite.Eventing.Sources.Server;

public sealed record Starting(Graphite.Server Server) : Event<Graphite.Server>(Server)
{
	public IPEndPoint EndPoint { get; set; } = new(IPAddress.Any, 25565);
}