namespace Graphite.Eventing.Sources.Server;

public sealed class Stopping(Graphite.Server server) : Event<Graphite.Server>(server)
{
	public string Reason { get; set; } = "No reason provided.";

	public TimeSpan Timeout { get; set; } = TimeSpan.FromMinutes(1);
}