namespace Graphite.Eventing.Sources.Server;

public sealed class Stopping(Graphite.Server server) : Event<Graphite.Server>(server)
{
	public string Reason { get; set; } = "Server stopped.";
}