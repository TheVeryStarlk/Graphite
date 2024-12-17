using System.Net;

namespace Graphite.Abstractions.Eventing.Sources.Listener;

public sealed class Starting : Event<IServer>
{
    public IPEndPoint EndPoint { get; set; } = new(IPAddress.Any, 25565);
}