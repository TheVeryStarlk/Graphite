using System.Net;

namespace Graphite.Eventing.Sources.Server;

public sealed class Starting(Graphite.Server server, IPEndPoint endPoint) : Event<Graphite.Server>(server)
{
	public IPEndPoint EndPoint { get; set; } = endPoint;
}